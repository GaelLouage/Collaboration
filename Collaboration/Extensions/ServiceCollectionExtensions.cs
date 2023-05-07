using Collaboration.Constants;
using Collaboration.Entities;
using Collaboration.Repositories.Classes;
using Collaboration.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver.Core.Configuration;

namespace Collaboration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServicesExtensions(this IServiceCollection services, IConfiguration config)
        {
            return
                services
                .AddScoped<IMongoRepository<User>>(sp => Scope<User>(Collections.USER, config))
                .AddScoped<IMongoRepository<Collaborator>>(sp => Scope<Collaborator>(Collections.COLLABORATOR, config))
                .AddScoped<IMongoRepository<Itinerary>>(sp => Scope<Itinerary>(Collections.ITINERARY, config))
                .AddScoped<IMongoRepository<Destination>>(sp => Scope<Destination>(Collections.DESTINATION, config));
        }
        public static MongoRepository<T> Scope<T>(string collectionName, IConfiguration config) where T : class
        {
            return new MongoRepository<T>(config.GetConnectionString("MongoConnection"), Database.DATABASENAME, collectionName);
        }
    }
}
