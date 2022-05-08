using MediatR;

namespace MagnifyLogistics.Features.BookShipment
{
    // too keep better intellisense experience it's a class, but depending on project agreements it could be a namespace
    public partial class BookShipmentFeature
    {
        public class Command : IRequest
        {
            // CarrierCustomerId should be set and injected with identity context object (and indicate id of customer, who is executing that command)
            public int CarrierCustomerId { get; set; }
            public int ShipmentId { get; set; }
        }
    }
}
