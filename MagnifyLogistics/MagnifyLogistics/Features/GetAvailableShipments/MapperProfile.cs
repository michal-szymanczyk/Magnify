using AutoMapper;
using MagnifyLogistics.Models;

namespace MagnifyLogistics.Features.GetAvailableShipments
{
    public partial class GetAvailableShipmentsFeature
    {
        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Shipment, Model>(MemberList.Destination);
            }
        }
    }
}
