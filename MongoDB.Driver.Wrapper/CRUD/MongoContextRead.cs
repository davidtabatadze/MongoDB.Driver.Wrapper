using CoreKit.Sync;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MongoDB.Driver.Wrapper.CRUD
{

    /// <summary>
    /// Represents CRUD read functionality for <see cref="MongoContext"/>
    /// </summary>
    public static class MongoContextRead
    {

        #region Sync

        /// <summary>
        /// Get data count
        /// </summary>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Data count</returns>
        public static long Count<E>(this MongoContext context, FilterDefinition<E> filter = null)
        {
            // Return the data count
            return SyncKit.Run(() => context.CountAsync(filter));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo records</returns>
        public static List<E> Load<E>(this MongoContext context, FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Read and return the data
            return SyncKit.Run(() => context.LoadAsync(filter, kit));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        public static List<E> Load<E>(this MongoContext context, List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return SyncKit.Run(() => context.LoadAsync<E>(keys));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        public static List<E> Load<E>(this MongoContext context, List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return SyncKit.Run(() => context.LoadAsync<E>(keys));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo record</returns>
        public static E Get<E>(this MongoContext context, FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Read and return the data
            return SyncKit.Run(() => context.GetAsync(filter, kit));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        public static E Get<E>(this MongoContext context, long key) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return SyncKit.Run(() => context.GetAsync<E>(key));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        public static E Get<E>(this MongoContext context, string key) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return SyncKit.Run(() => context.GetAsync<E>(key));
        }

        #endregion

        #region Async

        /// <summary>
        /// Get data count
        /// </summary>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Data count</returns>
        public static async Task<long> CountAsync<E>(this MongoContext context, FilterDefinition<E> filter = null)
        {
            // Fixing filter
            filter = filter ?? Builders<E>.Filter.Empty;
            // Return the data count
            return context.Session == null ?
                   await context.Collection<E>().CountDocumentsAsync(filter) :
                   await context.Collection<E>().CountDocumentsAsync(context.Session, filter);
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo records</returns>
        public static async Task<List<E>> LoadAsync<E>(this MongoContext context, FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Validating E
            MongoHelper.ValidateMongoEntity<E>();
            // Fixing filter
            filter = filter ?? Builders<E>.Filter.Empty;
            // Fixing kit
            kit = kit ?? new MongoFilterKit { };
            // Defining options according kit
            var options = kit.ToFindOptions<E>(context.Configuration.UseLowerCamelCaseProperties);
            // Generating data query
            var data = context.Session == null ?
                       await context.Collection<E>().FindAsync(filter, options) :
                       await context.Collection<E>().FindAsync(context.Session, filter, options);
            // Returning materialized data
            return data.ToList();
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        public static async Task<List<E>> LoadAsync<E>(this MongoContext context, List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return await context.LoadAsync(Builders<E>.Filter.Where(i => keys.Contains(i.Key)));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        public static async Task<List<E>> LoadAsync<E>(this MongoContext context, List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return await context.LoadAsync(Builders<E>.Filter.Where(i => keys.Contains(i.Key)));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo record</returns>
        public static async Task<E> GetAsync<E>(this MongoContext context, FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable
        {
            // Fixing kit
            kit = kit ?? new MongoFilterKit { };
            kit.Limit = 1;
            // Loading data with only one record kit
            var data = await context.LoadAsync(filter, kit);
            // Returning that single record
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        public static async Task<E> GetAsync<E>(this MongoContext context, long key) where E : IMongoEntityKeyable<long>
        {
            // Read and return the data
            return await context.GetAsync(Builders<E>.Filter.Where(i => i.Key == key));
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        public static async Task<E> GetAsync<E>(this MongoContext context, string key) where E : IMongoEntityKeyable<string>
        {
            // Read and return the data
            return await context.GetAsync(Builders<E>.Filter.Where(i => i.Key == key));
        }

        #endregion

    }

}
