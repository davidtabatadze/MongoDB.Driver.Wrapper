using System;
using System.Threading.Tasks;
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
        /// Client object
        /// </summary>
        private MongoClient Client { get; set; }

        /// <summary>
        /// Session object
        /// </summary>
        private IClientSessionHandle Session { get; set; }

        /// <summary>
        /// Counter for sequenced transactions
        /// </summary>
        private int TransactionCounter { get; set; }

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
        /// Begins session transaction
        /// </summary>
        public void BeginTransaction()
        {
            // Begin...
            BeginTransactionAsync().Wait();
        }

        /// <summary>
        /// Begins session transaction asynchronously
        /// </summary>
        /// <returns>Empty</returns>
        public async Task BeginTransactionAsync()
        {
            // Only if transactions are permitted
            if (Configuration.DisableTransactions)
            {
                // Only if session is empty
                if (Session == null)
                {
                    // We do create it and then begin the transaction
                    Session = await Client.StartSessionAsync();
                    Session.StartTransaction();
                }
                // Every time, when transaction is requested, we increase counter
                TransactionCounter++;
            }
        }

        /// <summary>
        /// Commits session transaction
        /// </summary>
        public void CommitTransaction()
        {
            // Commit...
            CommitTransactionAsync().Wait();
        }

        /// <summary>
        /// Commits session transaction asynchronously
        /// </summary>
        /// <returns>Empty</returns>
        public async Task CommitTransactionAsync()
        {
            // Only if transactions are permitted
            if (!Configuration.DisableTransactions)
            {
                // Every time, when transaction commit is requested, we decrease counter
                TransactionCounter--;
                // Only if we dont have any sequenced transaction
                if (TransactionCounter == 0)
                {
                    // We do commit existing session
                    await Session.CommitTransactionAsync();
                    Session.Dispose();
                    Session = null;
                }
            }
        }

        /// <summary>
        /// Rollbacks session transaction
        /// </summary>
        public void RollbackTransaction()
        {
            // Rollback...
            RollbackTransactionAsync().Wait();
        }

        /// <summary>
        /// Rollbacks session transaction asynchronously
        /// </summary>
        /// <returns>Empty</returns>
        public async Task RollbackTransactionAsync()
        {
            // Only if transactions are permitted
            if (!Configuration.DisableTransactions)
            {
                // Only if session exists
                if (Session != null)
                {
                    // We do abort it
                    await Session.AbortTransactionAsync();
                    Session.Dispose();
                    Session = null;
                }
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
