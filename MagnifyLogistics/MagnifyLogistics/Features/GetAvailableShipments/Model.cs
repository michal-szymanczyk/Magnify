namespace MagnifyLogistics.Features.GetAvailableShipments
{
    public partial class GetAvailableShipmentsFeature
    {
        public class Model
        {
            public int Id { get; set; }
            public string PickupAddress { get; set; }
            public string DestinationAddress { get; set; }
            public decimal PreferredBudgetAmount { get; set; }
            public string AdditionalInfo { get; set; }
        }
    }
}
