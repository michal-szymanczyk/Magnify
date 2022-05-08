using MagnifyLogistics.Persistence;

namespace MagnifyLogistics.Models
{
    public class Shipment : BaseModel
    {
        public int OwnerId { get; set; }
        public bool IsBooked { get; set; }

        public string PickupAddress { get; set; }
        public string DestinationAddress { get; set; }
        public decimal PreferredBudgetAmount { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
