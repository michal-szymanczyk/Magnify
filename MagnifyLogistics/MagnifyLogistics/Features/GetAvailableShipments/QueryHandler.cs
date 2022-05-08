using AutoMapper;
using MagnifyLogistics.Models;
using MagnifyLogistics.Persistence;
using MediatR;

namespace MagnifyLogistics.Features.GetAvailableShipments
{
    // too keep better intellisense experience it's a class, but depending on project agreements it could be a namespace
    public partial class GetAvailableShipmentsFeature
    {
        public class QueryHandler : RequestHandler<Query, IList<Model>>
        {
            private readonly IRepository<Shipment> _shipmentRepo;
            private readonly IMapper _mapper;

            public QueryHandler(IRepository<Shipment> shipmentRepo, IMapper mapper)
            {
                _mapper = mapper;
                _shipmentRepo = shipmentRepo;
            }

            protected override IList<Model> Handle(Query request)
                => _mapper.Map<List<Model>>(_shipmentRepo.Find(s => !s.IsBooked));
        }
    }
}
