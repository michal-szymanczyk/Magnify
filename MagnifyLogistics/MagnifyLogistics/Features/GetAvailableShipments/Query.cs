using MediatR;

namespace MagnifyLogistics.Features.GetAvailableShipments
{
    // too keep better intellisense experience it's a class, but depending on project agreements it could be a namespace
    public partial class GetAvailableShipmentsFeature
    {
        public class Query : IRequest<IList<Model>>
        {
            // it could define optional properties used for filtering
        }
    }
}
