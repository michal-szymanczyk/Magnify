using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagnifyLogistics.Common;
using MagnifyLogistics.Features.BookShipment;
using MagnifyLogistics.Models;
using MagnifyLogistics.Models.Identity;
using MagnifyLogistics.Persistence;
using MediatR;
using Moq;
using Xunit;

namespace MagnifyLogistics.UnitTests.Features
{
    public class BookShipmentTests
    {

        [Fact]
        public async Task ShouldThrowValidationExceptionForBookingShipmentTwice()
        {
            // arrange
            var shipmentRepo = Mock.Of<IRepository<Shipment>>(x => 
                x.Find(It.Is<Func<Shipment, bool>>(f => f(new Shipment { Id = 1 })))
                == new List<Shipment> { new() { IsBooked = true} });

            var customerRepo = Mock.Of<IRepository<Customer>>();
            var orderRepo = Mock.Of<IRepository<ShipmentOrder>>();
            var systemTime = Mock.Of<ISystemTime>();

            IRequestHandler<BookShipmentFeature.Command> handler = new BookShipmentFeature.CommandHandler(customerRepo, shipmentRepo, orderRepo, systemTime);
            var command = new BookShipmentFeature.Command
            {
                CarrierCustomerId = 2,
                ShipmentId = 1
            };

            // act & assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, default));

            Mock.Get(shipmentRepo).Verify(x => x.Find(It.IsAny<Func<Shipment, bool>>()), Times.Once);
        }

        [Fact]
        public async Task ShouldBookShipmentForValidData()
        {
            // arrange
            var shipmentRepo = Mock.Of<IRepository<Shipment>>(x =>
                x.Find(It.Is<Func<Shipment, bool>>(f => f(new Shipment { Id = 1 })))
                == new List<Shipment> { new() { Id = 1, IsBooked = false, PreferredBudgetAmount = 100, OwnerId = 10 } });
            
            var customerRepo = Mock.Of<IRepository<Customer>>(x =>
                x.Find(It.Is<Func<Customer, bool>>(f => f(new Customer { Id = 2 })))
                == new List<Customer> { new() { Id = 2, Roles = new List<CustomerRoles> { CustomerRoles.Carrier }} });

            var orderRepo = Mock.Of<IRepository<ShipmentOrder>>();

            var systemTime = Mock.Of<ISystemTime>(x => x.GetNow() == new DateTime(2022, 1, 2, 10, 20, 30));

            IRequestHandler<BookShipmentFeature.Command> handler = new BookShipmentFeature.CommandHandler(customerRepo, shipmentRepo, orderRepo, systemTime);
            var command = new BookShipmentFeature.Command
            {
                CarrierCustomerId = 2,
                ShipmentId = 1
            };

            // act
            await handler.Handle(command, default);

            // assert
            Mock.Get(shipmentRepo).Verify(x => x.Find(It.IsAny<Func<Shipment, bool>>()), Times.Once);
            Mock.Get(shipmentRepo).Verify(x => x.Update(It.Is<Shipment>(s => s.IsBooked == true && s.Id == 1)), Times.Once);

            Mock.Get(customerRepo).Verify(x => x.Find(It.IsAny<Func<Customer, bool>>()), Times.Once);

            Mock.Get(orderRepo).Verify(x => x.Add(It.Is<ShipmentOrder>(s => s.CarrierCustomerId == 2
                                                                            && s.CreatedOn.Equals(new DateTime(2022, 1, 2, 10, 20, 30))
                                                                            && s.Price == 100
                                                                            && s.ShipmentId == 1)), Times.Once);
        }
    }
}
