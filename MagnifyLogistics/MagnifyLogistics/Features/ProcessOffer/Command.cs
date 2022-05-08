using MagnifyLogistics.Models;
using MediatR;

namespace MagnifyLogistics.Features.ProcessOffer
{
    // too keep better intellisense experience it's a class, but depending on project agreements it could be a namespace
    public partial class ProcessOfferFeature
    {
        public class Command : IRequest
        {
            // ShipperCustomerId should be set and injected with identity context object (and indicate id of customer, who is executing that command)
            public int ShipperCustomerId { get; set; }
            public int OfferId { get; set; }
            public OfferStatus Status { get; set; }
        }
    }
}
