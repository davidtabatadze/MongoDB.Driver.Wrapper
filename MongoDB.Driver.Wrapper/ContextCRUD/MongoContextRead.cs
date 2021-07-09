using CoreKit.Sync;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents CRUD read functionality for <see cref="MongoContext"/>
    /// </summary>
    public partial class MongoContext
    {

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo records</returns>
        public new List<E> Load<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Read and return the data
            return SyncKit.Run(() => base.Load(filter, kit));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        public new List<E> Load<E>(List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return SyncKit.Run(() => base.Load<E>(keys));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        public new List<E> Load<E>(List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return SyncKit.Run(() => base.Load<E>(keys));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo record</returns>
        public new E Get<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Read and return the data
            return SyncKit.Run(() => base.Get(filter, kit));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        public new E Get<E>(long key) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return SyncKit.Run(() => base.Get<E>(key));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        public new E Get<E>(string key) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return SyncKit.Run(() => base.Get<E>(key));
        }

        /// <summary>
        /// Get data count
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Data count</returns>
        public new long Count<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable
        {
            // Return the data count
            return SyncKit.Run(() => base.Count(filter));
        }

    }

    /// <summary>
    /// Represents CRUD read functionality for <see cref="MongoContextAsync"/>
    /// </summary>
    public partial class MongoContextAsync
    {

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo records</returns>
        public new async Task<List<E>> Load<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Read and return the data
            return await base.Load(filter, kit);
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        public new async Task<List<E>> Load<E>(List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return await base.Load<E>(keys);
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        public new async Task<List<E>> Load<E>(List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return await base.Load<E>(keys);
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo record</returns>
        public new async Task<E> Get<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Read and return the data
            return await base.Get(filter, kit);
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        public new async Task<E> Get<E>(long key) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return await base.Get<E>(key);
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        public new async Task<E> Get<E>(string key) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return await base.Get<E>(key);
        }

        /// <summary>
        /// Get data count
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Data count</returns>
        public new async Task<long> Count<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable
        {
            // Return the data count
            return await base.Count(filter);
        }

    }

    /// <summary>
    /// Represents CRUD read functionality for <see cref="MongoContextBase"/>
    /// </summary>
    public partial class MongoContextBase
    {

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo records</returns>
        internal protected async Task<List<E>> Load<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Validating E
            MongoHelper.ValidateMongoEntity<E>();
            // Fixing filter
            filter ??= Builders<E>.Filter.Empty;
            // Fixing kit
            kit ??= new MongoFilterKit { };
            // Defining options according kit
            var options = kit.ToFindOptions<E>(Configuration.UseLowerCamelCaseProperties);
            // Generating data query
            var data = Session == null ?
                       await Collection<E>().FindAsync(filter, options) :
                       await Collection<E>().FindAsync(Session, filter, options);
            // Returning materialized data
            return data.ToList();
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        internal protected async Task<List<E>> Load<E>(List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return await Load(Builders<E>.Filter.Where(i => keys.Contains(i.Key)));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        internal protected async Task<List<E>> Load<E>(List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return await Load(Builders<E>.Filter.Where(i => keys.Contains(i.Key)));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo record</returns>
        internal protected async Task<E> Get<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Fixing kit
            kit ??= new MongoFilterKit { };
            kit.Limit = 1;
            // Loading data with only one record kit
            var data = await Load(filter, kit);
            // Returning that single record
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        internal protected async Task<E> Get<E>(long key) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return await Get(Builders<E>.Filter.Where(i => i.Key == key));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        internal protected async Task<E> Get<E>(string key) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return await Get(Builders<E>.Filter.Where(i => i.Key == key));
        }

        /// <summary>
        /// Get data count
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Data count</returns>
        internal protected async Task<long> Count<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable
        {
            // Fixing filter
            filter ??= Builders<E>.Filter.Empty;
            // Return the data count
            return Session == null ?
                   await Collection<E>().CountDocumentsAsync(filter) :
                   await Collection<E>().CountDocumentsAsync(Session, filter);
        }

    }

}
