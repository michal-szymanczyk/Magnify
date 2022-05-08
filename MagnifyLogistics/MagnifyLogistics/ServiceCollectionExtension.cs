using AutoMapper;
using MagnifyLogistics.Common;
using MagnifyLogistics.Models;
using MagnifyLogistics.Models.Identity;
using MagnifyLogistics.Persistence;
using MagnifyLogistics.Persistence.InMemory;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MagnifyLogistics
{
    internal static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencies(this IServiceCollection sc)
        {
            sc.AddSingleton<ISystemTime, SystemTime>();

            sc.AddSingleton<InMemoryUniOfWork>();

            // for simplicity, registering all repositories by hand
            // with reflection it could be handled implicitly 
            // registered as singletons because of poor locking mechanism, with ef core these could be registered as Scoped or Transient
            sc.AddSingleton<IRepository<Customer>, Repository<Customer>>();
            sc.AddSingleton<IRepository<Offer>, Repository<Offer>>();
            sc.AddSingleton<IRepository<Shipment>, Repository<Shipment>>();
            sc.AddSingleton<IRepository<ShipmentOrder>, Repository<ShipmentOrder>>();

            //AutoMapper
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddMaps(typeof(ServiceCollectionExtension)));
            var mapper = mapperConfig.CreateMapper();
            sc.AddSingleton(mapper);
            
            sc.AddMediatR(typeof(ServiceCollectionExtension));

            return sc;
        }
    }
}
