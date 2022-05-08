using MagnifyLogistics.Common;
using MagnifyLogistics.Models;
using MagnifyLogistics.Models.Identity;
using MagnifyLogistics.Persistence;
using MediatR;

namespace MagnifyLogistics.Features.ProcessOffer
{
    public partial class ProcessOfferFeature
    {
        public class CommandHandler : RequestHandler<Command>
        {
            private readonly IRepository<Customer> _customerRepo;
            private readonly IRepository<Shipment> _shipmentRepo;
            private readonly IRepository<Offer> _offersRepo;
            private readonly IRepository<ShipmentOrder> _ordersRepo;
            private readonly ISystemTime _systemTime;

            public CommandHandler(IRepository<Customer> customerRepo, IRepository<Shipment> shipmentRepo,
                IRepository<Offer> offersRepo, IRepository<ShipmentOrder> ordersRepo, ISystemTime systemTime)
            {
                _systemTime = systemTime;
                _ordersRepo = ordersRepo;
                _offersRepo = offersRepo;
                _shipmentRepo = shipmentRepo;
                _customerRepo = customerRepo;
            }

            protected override void Handle(Command request)
            {
                // validation part could be moved to separate validator class (requires common implementation of MediatR pipelines)
                // but because of limited time for that exercise it's done in the handler

                if (request.Status == OfferStatus.Pending)
                {
                    // for that kind of validation preferred to use 3rd party validation libraries
                    throw new ValidationException($"{nameof(request.Status)} invalid value");
                }

                var owner = _customerRepo.Find(c => c.Id == request.ShipperCustomerId).SingleOrDefault()
                            ?? throw new ValidationException("Invalid customer");

                var offer = _offersRepo.Find(s => s.Id == request.OfferId).SingleOrDefault()
                               ?? throw new ValidationException("Invalid offer");

                if (offer.Status != OfferStatus.Pending)
                {
                    throw new ValidationException("Offer already processed");
                }

                var shipment = _shipmentRepo.Find(s => s.Id == offer.ShipmentId).Single();

                if (shipment.OwnerId != owner.Id)
                {
                    throw new ValidationException("You are not allowed to book that shipment");
                }

                if (shipment.IsBooked && request.Status == OfferStatus.Approved)
                {
                    throw new ValidationException("Shipment already booked");
                }

                // Update part should be placed in database transaction and check once more if it has not changed in mean time
                // for simplicity, transactional updates are skipped
                // in overall more complex queries/updates could be extracted to custom repository and maintained in separate "data access" project

                offer.Status = request.Status;
                _offersRepo.Update(offer);

                if (request.Status == OfferStatus.Approved)
                {
                    // following code is boilerplate with BookShipmentFeature code, so that part can be extracted to separate Command or shared in custom repository, simplified by validation part
                    shipment.IsBooked = true;
                    _shipmentRepo.Update(shipment);

                    _ordersRepo.Add(new ShipmentOrder
                    {
                        ShipmentId = shipment.Id,
                        CarrierCustomerId = offer.OwnerId,
                        CreatedOn = _systemTime.GetNow(),
                        Price = offer.Price
                    });
                }
            }
        }
    }
}
