using AutoMapper;
using MagnifyLogistics.Models;

namespace MagnifyLogistics.Features.RequestShipment
{
    public partial class RequestShipmentFeature
    {
        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Command, Shipment>(MemberList.Source);
            }
        }
    }
}
