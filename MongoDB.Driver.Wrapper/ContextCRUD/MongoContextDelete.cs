using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoreKit.Sync;
using CoreKit.Extension.Collection;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents CRUD delete functionality for <see cref="MongoContext"/>
    /// </summary>
    public partial class MongoContext
    {

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        public new void Delete<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable
        {
            // Delete the data
            SyncKit.Run(() => base.Delete(filter));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        public new void Delete<E>(List<E> entities) where E : IMongoEntityKeyable
        {
            // Delete the data
            SyncKit.Run(() => base.Delete(entities));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        public new void Delete<E>(E entity) where E : IMongoEntityKeyable
        {
            // Delete the data
            SyncKit.Run(() => base.Delete(entity));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        public new void Delete<E>(List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            SyncKit.Run(() => base.Delete<E>(keys));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        public new void Delete<E>(long key) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            SyncKit.Run(() => base.Delete<E>(key));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        public new void Delete<E>(List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            SyncKit.Run(() => base.Delete<E>(keys));
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        public new void Delete<E>(string key) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            SyncKit.Run(() => base.Delete<E>(key));
        }

    }

    /// <summary>
    /// Represents CRUD delete functionality for <see cref="MongoContextAsync"/>
    /// </summary>
    public partial class MongoContextAsync
    {

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Empty</returns>
        public new async Task Delete<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable
        {
            // Delete the data
            await base.Delete(filter);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Empty</returns>
        public new async Task Delete<E>(List<E> entities) where E : IMongoEntityKeyable
        {
            // Delete the data
            await base.Delete(entities);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Empty</returns>
        public new async Task Delete<E>(E entity) where E : IMongoEntityKeyable
        {
            // Delete the data
            await base.Delete(entity);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        /// <returns>Empty</returns>
        public new async Task Delete<E>(List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            await base.Delete<E>(keys);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        /// <returns>Empty</returns>
        public new async Task Delete<E>(long key) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            await base.Delete<E>(key);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        /// <returns>Empty</returns>
        public new async Task Delete<E>(List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            await base.Delete<E>(keys);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        /// <returns>Empty</returns>
        public new async Task Delete<E>(string key) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            await base.Delete<E>(key);
        }

    }

    /// <summary>
    /// Represents CRUD delete functionality for <see cref="MongoContextBase"/>
    /// </summary>
    public partial class MongoContextBase
    {

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Type of delete portion</typeparam>
        /// <typeparam name="E">Type of actual data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<T, E>(FilterDefinition<T> filter = null)
        {
            // Validating E
            MongoHelper.ValidateMongoEntity<E>();
            // Fixing filter
            filter ??= Builders<T>.Filter.Empty;
            // Delete data by filter
            var result = Session == null ?
                         await Collection<T>(typeof(E)).DeleteManyAsync(filter) :
                         await Collection<T>(typeof(E)).DeleteManyAsync(Session, filter);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Deletion keys</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<E>(List<object> keys)
        {
            // Validating keys
            if (keys.TrimEmptyOrLTE0().HasValue())
            {
                // Delete the data
                await Delete<MongoEntity, E>(Builders<MongoEntity>.Filter.Where(i => keys.Contains(i.Key)));
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable
        {
            // Delete the data
            await Delete<E, E>(filter);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<E>(List<E> entities) where E : IMongoEntityKeyable
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
                await Delete<E>(keys);
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<E>(E entity) where E : IMongoEntityKeyable
        {
            // Delete the data
            await Delete(new List<E> { entity });
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<E>(List<long> keys) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            await Delete<E>(keys.Cast<object>().ToList());
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<E>(long key) where E : IMongoEntityKeyable<long>
        {
            // Delete the data
            await Delete<E>(new List<long> { key });
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<E>(List<string> keys) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            await Delete<E>(keys.Cast<object>().ToList());
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        /// <returns>Empty</returns>
        internal protected async Task Delete<E>(string key) where E : IMongoEntityKeyable<string>
        {
            // Delete the data
            await Delete<E>(new List<string> { key });
        }

    }

}
