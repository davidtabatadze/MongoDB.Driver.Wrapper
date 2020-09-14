using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoreKit.Extension.String;
using CoreKit.Extension.Collection;
using Microsoft.Extensions.Options;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents wrapper of IMongoDatabase.
    /// </summary>
    public class MongoContext : IDisposable
    {

        /// <summary>
        /// Ending class lifecycle
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration <see cref="MongoConfiguration"/></param>
        /// <param name="entities">Entity mappings: class type to mongo table connection</param>
        public MongoContext(IOptions<MongoConfiguration> configuration, Dictionary<Type, string> entities) : this(configuration.Value, entities)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration <see cref="MongoConfiguration"/></param>
        /// <param name="entities">Entity mappings: class type to mongo table connection</param>
        public MongoContext(MongoConfiguration configuration, Dictionary<Type, string> entities)
        {
            // ...
            Configuration = configuration;
            Entities = entities;
        }

        /// <summary>
        /// Configuration object
        /// </summary>
        internal protected MongoConfiguration Configuration { get; set; }

        /// <summary>
        /// Class type to mongo table connection dictionary
        /// </summary>
        internal protected Dictionary<Type, string> Entities { get; set; }

        /// <summary>
        /// Client object
        /// </summary>
        internal protected MongoClient Client { get; set; }

        /// <summary>
        /// Database object
        /// </summary>
        private IMongoDatabase DatabaseObject { get; set; }

        /// <summary>
        /// Configured database
        /// </summary>
        internal protected IMongoDatabase Database
        {
            get
            {
                if (DatabaseObject == null)
                {
                    // Creating client
                    Client = Client ?? new MongoClient(Configuration.Server);
                    // Alternate client creation
                    ////Client = new MongoClient(
                    ////    new MongoClientSettings
                    ////    {
                    ////        MaxConnectionPoolSize = 1000,
                    ////        Server = new MongoServerAddress(Configuration.Server, Configuration.Port),
                    ////        Credential = MongoCredential.CreateCredential(Configuration.Database, "user", "pass")
                    ////    }
                    ////);
                    // Creating database reference
                    DatabaseObject = Client.GetDatabase(Configuration.Database);
                }
                // Returning as singleton
                return DatabaseObject;
            }
        }

        /// <summary>
        /// Session object
        /// </summary>
        internal protected IClientSessionHandle Session { get; set; }

        /// <summary>
        /// Counter for sequenced transactions
        /// </summary>
        internal protected int TransactionCounter { get; set; }

        /// <summary>
        /// Gets collection name
        /// </summary>
        /// <param name="type">Collection type</param>
        /// <returns>Collection name</returns>
        private string CollectionName(Type type)
        {
            // Seeking mongo collection
            Entities.TryGetValue(type, out string name);
            // Validating table name
            if (name.IsEmpty())
            {
                // Throwing undefined table exception
                throw new NotImplementedException(
                    string.Format("Destination mongo table is not defined for entity '{0}'.", type.Name)
                );
            }
            // Returning collection name
            return name;
        }

        /// <summary>
        /// Gets typebased collection
        /// </summary>
        /// <typeparam name="E">Entity type</typeparam>
        /// <param name="type">Collection type</param>
        /// <returns>Mongo collection</returns>
        internal protected IMongoCollection<E> Collection<E>(Type type)
        {
            // Seeking name and returning collection
            var name = CollectionName(type);
            return Database.GetCollection<E>(name);
        }

        /// <summary>
        /// Gets typebased collection
        /// </summary>
        /// <typeparam name="E">Entity type</typeparam>
        /// <returns>Mongo collection</returns>
        public IMongoCollection<E> Collection<E>()
        {
            // Returning collection
            return Collection<E>(typeof(E));
        }

    }

}
