using Collaboration.Constants;
using Collaboration.Entities;
using Collaboration.Repositories.Classes;
using Collaboration.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Core.Configuration;
using System.Collections;

namespace Collaboration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServicesExtensions(this IServiceCollection services, IConfiguration config)
        {
            return
            services
                .AddScoped(sp => Scope<User>(Collections.USER, config))
                .AddScoped(sp => Scope<Collaborator>(Collections.COLLABORATOR, config))
                .AddScoped(sp => Scope<Itinerary>(Collections.ITINERARY, config))
                .AddScoped(sp => Scope<Destination>(Collections.DESTINATION, config))
                  .AddScoped(sp => Scope<BlacklistToken>(Collections.BLACKLISTTOKEN, config));
        }
        public static MongoRepository<T> Scope<T>(string collectionName, IConfiguration config) where T : class
        {
            return new MongoRepository<T>(config.GetConnectionString("MongoConnection"), Database.DATABASENAME, collectionName);
        }
    }
}
