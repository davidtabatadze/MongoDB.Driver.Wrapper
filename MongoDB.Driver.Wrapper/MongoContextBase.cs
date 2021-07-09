using System;
using System.Collections.Generic;
using CoreKit.Extension.String;
using CoreKit.Extension.Collection;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents base wrapper of IMongoDatabase.
    /// </summary>
    public partial class MongoContextBase : IDisposable, IMongoContextBase
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
        /// <param name="mappings">Entity mappings: class type to mongo table connection</param>
        internal MongoContextBase(MongoConfiguration configuration, Dictionary<Type, string> mappings = null)
        {
            // ...
            Configuration = configuration;
            Entities = mappings ?? new Dictionary<Type, string> { };
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
        internal protected IMongoDatabase DatabaseObject { get; set; }

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
                    Client ??= new MongoClient(Configuration.Server);
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
        internal protected string CollectionName(Type type)
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
        /// Sets/Adds class type to mongo table connection mapping
        /// </summary>
        /// <param name="etype">Entity type</param>
        /// <param name="collection">Mongo collection</param>
        public void Mappings(Type etype, string collection)
        {
            // Set or add
            Mappings(new Dictionary<Type, string> { { etype, collection } });
        }

        /// <summary>
        /// Sets/Adds class type to mongo table connection mapping
        /// </summary>
        /// <param name="mappings">Entity mappings: class type to mongo table connection</param>
        public void Mappings(Dictionary<Type, string> mappings)
        {
            // If only mapping is present
            if (mappings.HasValue())
            {
                // We do set/add each one
                foreach (var mapping in mappings)
                {
                    // ... If mapping is not present yet
                    if (!Entities.ContainsKey(mapping.Key))
                    {
                        Entities.Add(mapping.Key, mapping.Value);
                    }
                }
            }
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
