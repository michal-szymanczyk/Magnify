using MagnifyLogistics.Persistence;

namespace MagnifyLogistics.Models
{
    public class ShipmentOrder : BaseModel
    {
        public int ShipmentId { get; set; }
        public int CarrierCustomerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Price { get; set; }
    }
}
