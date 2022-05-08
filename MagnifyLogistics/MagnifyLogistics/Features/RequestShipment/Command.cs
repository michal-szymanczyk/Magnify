using MediatR;

namespace MagnifyLogistics.Features.RequestShipment
{
    // too keep better intellisense experience it's a class, but depending on project agreements it could be a namespace
    public partial class RequestShipmentFeature
    {
        public class Command : IRequest
        {
            public int OwnerId { get; set; }
            public string PickupAddress { get; set; }
            public string DestinationAddress { get; set; }
            public decimal PreferredBudgetAmount { get; set; }
            public string AdditionalInfo { get; set; }
        }
    }
}
