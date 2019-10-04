using System;
using Microsoft.Extensions.Options;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents wrapper of IMongoDatabase.
    /// Contains mongo typebased table manager <see cref="MongoTable{T}"/>
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
        public MongoContext(IOptions<MongoConfiguration> configuration) : this(configuration.Value)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration <see cref="MongoConfiguration"/></param>
        public MongoContext(MongoConfiguration configuration)
        {
            // ...
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration object
        /// </summary>
        private MongoConfiguration Configuration { get; set; }

        /// <summary>
        /// Database object
        /// </summary>
        private IMongoDatabase DatabaseObject { get; set; }

        /// <summary>
        /// Gets configured database
        /// </summary>
        public IMongoDatabase Database
        {
            get
            {
                if (DatabaseObject == null)
                {
                    //new MongoClient(new MongoClientSettings { WaitQueueSize = 1000, MaxConnectionPoolSize = 1000, Server = new MongoServerAddress(Config.Server, Config.Port), Credential = MongoCredential.CreateCredential(  Config.Database,  "cloud_user", "XhrPysS7wra&Ef#2" ) });
                    // Creating client
                    var client = new MongoClient(Configuration.Server);
                    // Creating database reference
                    DatabaseObject = client.GetDatabase(Configuration.Database);
                }
                // Returning as singleton
                return DatabaseObject;
            }
        }

        /// <summary>
        /// Gets database typebased table manager <see cref="MongoTable{T}"/>
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <returns>Typebased table manager</returns>
        public MongoTable<T> Table<T>() where T : IMongoEntity, new()
        {
            return new MongoTable<T>(Database.GetCollection<T>(new T() { }.Table));
        }

    }

}
