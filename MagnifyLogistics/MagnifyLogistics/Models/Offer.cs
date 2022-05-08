using MagnifyLogistics.Persistence;

namespace MagnifyLogistics.Models
{
    public class Offer : BaseModel
    {
        public int ShipmentId { get; set; }
        public int OwnerId { get; set; }
        public decimal Price { get; set; }
        public OfferStatus Status { get; set; }
    }
}
