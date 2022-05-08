using MagnifyLogistics;
using MagnifyLogistics.Features.BookShipment;
using MagnifyLogistics.Features.GetAvailableShipments;
using MagnifyLogistics.Features.RequestShipment;
using MagnifyLogistics.Models.Identity;
using MagnifyLogistics.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;



// overall testing code:

var diProvider = new ServiceCollection()
    .AddDependencies()
    .BuildServiceProvider();

var mediator = diProvider.GetRequiredService<IMediator>();


var customersRepo = diProvider.GetService<IRepository<Customer>>();

customersRepo.Add(new Customer
{
    Name = "Shipper",
    Roles = new List<CustomerRoles> { CustomerRoles.Shipper }
});
customersRepo.Add(new Customer
{
    Name = "Carrier",
    Roles = new List<CustomerRoles> { CustomerRoles.Carrier }
});

var shipper = customersRepo.Find(c => c.Name.Equals("Shipper")).First();
var carrier = customersRepo.Find(c => c.Name.Equals("Carrier")).First();

mediator.Send(new RequestShipmentFeature.Command
    {
        OwnerId = shipper.Id,
        AdditionalInfo = "First package",
        PreferredBudgetAmount = 1000,
        DestinationAddress = "Chicago",
        PickupAddress = "NY"
    })
    .Wait();

mediator.Send(new RequestShipmentFeature.Command
    {
        OwnerId = shipper.Id,
        AdditionalInfo = "Second package",
        PreferredBudgetAmount = 2000,
        DestinationAddress = "Chicago",
        PickupAddress = "NY"
    })
    .Wait();

int lastShipmentId = 0;
Action<IList<GetAvailableShipmentsFeature.Model>> displayShipments =
    shipments =>
    {
        foreach (var shipment in shipments)
        {
            Console.WriteLine(
                $@"{shipment.Id}# {shipment.AdditionalInfo}, {shipment.PreferredBudgetAmount}, FROM: {shipment.PickupAddress}, TO: {shipment.DestinationAddress}");
            lastShipmentId = shipment.Id;
        }
    };

Console.WriteLine("Before approving any shipment");

mediator.Send(new GetAvailableShipmentsFeature.Query())
    .ContinueWith(t => displayShipments(t.Result))
    .Wait();

// take one shipment by the carrier
mediator.Send(new BookShipmentFeature.Command
    {
        CarrierCustomerId = carrier.Id,
        ShipmentId = lastShipmentId
    })
    .Wait();

Console.WriteLine("After making one approval");

mediator.Send(new GetAvailableShipmentsFeature.Query())
    .ContinueWith(t => displayShipments(t.Result))
    .Wait();

Console.ReadKey();
