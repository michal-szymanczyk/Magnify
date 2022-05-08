using AutoMapper;
using MagnifyLogistics.Common;
using MagnifyLogistics.Models;
using MagnifyLogistics.Models.Identity;
using MagnifyLogistics.Persistence;
using MediatR;

namespace MagnifyLogistics.Features.RequestShipment
{
    public partial class RequestShipmentFeature
    {
        public class CommandHandler : RequestHandler<Command>
        {
            private readonly IRepository<Customer> _customerRepo;
            private readonly IRepository<Shipment> _shipmentRepo;
            private readonly IMapper _mapper;

            public CommandHandler(IRepository<Customer> customerRepo,
                IRepository<Shipment> shipmentRepo, IMapper mapper)
            {
                _mapper = mapper;
                _shipmentRepo = shipmentRepo;
                _customerRepo = customerRepo;
            }

            protected override void Handle(Command request)
            {
                // validation part could be moved to separate validator class (requires common implementation of MediatR pipelines)
                // but because of limited time for that exercise it's done in the handler

                if (request.PreferredBudgetAmount <= 0)
                {
                    // for that kind of validation preferred to use 3rd party validation libraries
                    throw new ValidationException($"{nameof(request.PreferredBudgetAmount)} is negative");
                }

                var customer = _customerRepo.Find(c => c.Id == request.OwnerId).SingleOrDefault()
                    ?? throw new ValidationException("Customer not found");

                if (!customer.Roles.Contains(CustomerRoles.Shipper))
                {
                    throw new ValidationException("Customer does not have privileges to request a shipment");
                }

                _shipmentRepo.Add(_mapper.Map<Shipment>(request));
            }
        }
    }
}
