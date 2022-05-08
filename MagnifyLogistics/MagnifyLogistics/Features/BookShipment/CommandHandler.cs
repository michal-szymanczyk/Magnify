using MagnifyLogistics.Common;
using MagnifyLogistics.Models;
using MagnifyLogistics.Models.Identity;
using MagnifyLogistics.Persistence;
using MediatR;

namespace MagnifyLogistics.Features.BookShipment
{
    public partial class BookShipmentFeature
    {
        public class CommandHandler : RequestHandler<Command>
        {
            private readonly IRepository<Customer> _customerRepo;
            private readonly IRepository<Shipment> _shipmentRepo;
            private readonly IRepository<ShipmentOrder> _ordersRepo;
            private readonly ISystemTime _systemTime;

            public CommandHandler(IRepository<Customer> customerRepo, IRepository<Shipment> shipmentRepo,
                IRepository<ShipmentOrder> ordersRepo, ISystemTime systemTime)
            {
                _systemTime = systemTime;
                _ordersRepo = ordersRepo;
                _shipmentRepo = shipmentRepo;
                _customerRepo = customerRepo;
            }

            protected override void Handle(Command request)
            {
                // validation part could be moved to separate validator class (requires common implementation of MediatR pipelines)
                // but because of limited time for that exercise it's done in the handler
                
                var shipment = _shipmentRepo.Find(s => s.Id == request.ShipmentId).SingleOrDefault()
                               ?? throw new ValidationException("Invalid shipment");

                if (shipment.IsBooked)
                {
                    throw new ValidationException("Shipment already booked");
                }

                var carrier = _customerRepo.Find(c => c.Id == request.CarrierCustomerId).SingleOrDefault()
                              ?? throw new ValidationException("Invalid customer");

                if (!carrier.Roles.Contains(CustomerRoles.Carrier) || request.CarrierCustomerId == shipment.OwnerId)
                {
                    throw new ValidationException("You are not allowed to book that shipment");
                }


                // Update part should be placed in database transaction and check once more if it has not changed in mean time
                // for simplicity, transactional updates are skipped
                // in overall more complex queries/updates could be extracted to custom repository and maintained in separate "data access" project


                // following code is boilerplate with ProcessOfferFeature code, so that part can be extracted to separate Command or shared in custom repository, simplified by validation part
                shipment.IsBooked = true;
                _shipmentRepo.Update(shipment);

                _ordersRepo.Add(new ShipmentOrder
                {
                    ShipmentId = shipment.Id,
                    CarrierCustomerId = carrier.Id,
                    CreatedOn = _systemTime.GetNow(),
                    Price = shipment.PreferredBudgetAmount
                });
            }
        }
    }
}
