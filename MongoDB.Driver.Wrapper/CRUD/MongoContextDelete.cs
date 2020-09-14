using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoreKit.Sync;
using CoreKit.Extension.Collection;

namespace MongoDB.Driver.Wrapper.CRUD
{

    /// <summary>
    /// Represents CRUD delete functionality for <see cref="MongoContext"/>
    /// </summary>
    public static class MongoContextDelete
    {

        #region Sync

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Type of delete portion</typeparam>
        /// <typeparam name="E">Type of actual data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="filter">Mongo filter</param>
        private static void Delete<T, E>(this MongoContext context, FilterDefinition<T> filter = null)
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync<T, E>(filter));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Deletion keys</param>
        private static void Delete<E>(this MongoContext context, List<object> keys)
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync<E>(keys));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="filter">Mongo filter</param>
        public static void Delete<E>(this MongoContext context, FilterDefinition<E> filter = null) where E : IMongoEntityKeyable
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync<E, E>(filter));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entities">Mongo entities</param>
        public static void Delete<E>(this MongoContext context, List<E> entities) where E : IMongoEntityKeyable
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync(entities));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entity">Mongo entity</param>
        public static void Delete<E>(this MongoContext context, E entity) where E : IMongoEntityKeyable
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync(entity));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Mongo entity keys</param>
        public static void Delete<E>(this MongoContext context, List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync<E>(keys.Cast<object>().ToList()));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="key">Mongo entity key</param>
        public static void Delete<E>(this MongoContext context, long key) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync<E>(key));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Mongo entity keys</param>
        public static void Delete<E>(this MongoContext context, List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync<E>(keys.Cast<object>().ToList()));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="key">Mongo entity key</param>
        public static void Delete<E>(this MongoContext context, string key) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            SyncKit.Run(() => context.DeleteAsync<E>(key));
        }

        #endregion

        #region Async

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Type of delete portion</typeparam>
        /// <typeparam name="E">Type of actual data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Nothing</returns>
        private static async Task DeleteAsync<T, E>(this MongoContext context, FilterDefinition<T> filter = null)
        {
            // Validating E
            MongoHelper.ValidateMongoEntity<E>();
            // Fixing filter
            filter = filter ?? Builders<T>.Filter.Empty;
            // Delete data by filter
            var result = context.Session == null ?
                         await context.Collection<T>(typeof(E)).DeleteManyAsync(filter) :
                         await context.Collection<T>(typeof(E)).DeleteManyAsync(context.Session, filter);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Deletion keys</param>
        /// <returns>Nothing</returns>
        private static async Task DeleteAsync<E>(this MongoContext context, List<object> keys)
        {
            // Validating keys
            if (keys.TrimEmptyOrLTE0().HasValue())
            {
                // Delete the data
                await context.DeleteAsync<MongoEntity, E>(Builders<MongoEntity>.Filter.Where(i => keys.Contains(i.Key)));
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteAsync<E>(this MongoContext context, FilterDefinition<E> filter = null) where E : IMongoEntityKeyable
        {
            // Delete the data
            await context.DeleteAsync<E, E>(filter);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteAsync<E>(this MongoContext context, List<E> entities) where E : IMongoEntityKeyable
        {
            // Validating entities
            if (entities.TrimEmpty().HasValue())
            {
                // Defining the actual keys
                var keys = new List<object> { };
                var typeLong = typeof(IMongoEntityKeyable<long>).IsAssignableFrom(typeof(E));
                var typeString = typeof(IMongoEntityKeyable<string>).IsAssignableFrom(typeof(E));
                foreach (var entity in entities)
                {
                    keys.Add(
                        typeLong ? (entity as IMongoEntityKeyable<long>).Key :
                        typeString ? (entity as IMongoEntityKeyable<string>).Key :
                        default(object)
                    );
                }
                // Delete the data
                await context.DeleteAsync<E>(keys);
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteAsync<E>(this MongoContext context, E entity) where E : IMongoEntityKeyable
        {
            // Delete the data
            await context.DeleteAsync(new List<E> { entity });
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Mongo entity keys</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteAsync<E>(this MongoContext context, List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            await context.DeleteAsync<E>(keys.Cast<object>().ToList());
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="key">Mongo entity key</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteAsync<E>(this MongoContext context, long key) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            await context.DeleteAsync<E>(new List<long> { key });
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="keys">Mongo entity keys</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteAsync<E>(this MongoContext context, List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            await context.DeleteAsync<E>(keys.Cast<object>().ToList());
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="key">Mongo entity key</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteAsync<E>(this MongoContext context, string key) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            await context.DeleteAsync<E>(new List<string> { key });
        }

        #endregion

    }

}
