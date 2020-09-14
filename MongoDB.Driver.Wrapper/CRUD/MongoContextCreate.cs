using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoreKit.Sync;
using CoreKit.Extension.Collection;

namespace MongoDB.Driver.Wrapper.CRUD
{

    /// <summary>
    /// Represents CRUD create functionality for <see cref="MongoContext"/>
    /// </summary>
    public static class MongoContextCreate
    {

        #region Sync

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Inserted entities</returns>
        public static List<E> Insert<E>(this MongoContext context, List<E> entities) where E : IMongoEntityKeyable
        {
            // Inserting the data
            return SyncKit.Run(() => context.InsertAsync(entities));
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Inserted entity</returns>
        public static E Insert<E>(this MongoContext context, E entity) where E : IMongoEntityKeyable
        {
            // Inserting the data
            return SyncKit.Run(() => context.InsertAsync(entity));
        }

        #endregion

        #region Async

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Inserted entities</returns>
        public static async Task<List<E>> InsertAsync<E>(this MongoContext context, List<E> entities) where E : IMongoEntityKeyable
        {
            // Validating E
            MongoHelper.ValidateMongoEntity<E>();
            // Validating entities
            if (entities.TrimEmpty().HasValue())
            {
                // Inserting the data
                var result = context.Session == null ?
                             context.Collection<E>().InsertManyAsync(entities) :
                             context.Collection<E>().InsertManyAsync(context.Session, entities);
                await result;
            }
            // Retunring entities
            return entities;
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Inserted entity</returns>
        public static async Task<E> InsertAsync<E>(this MongoContext context, E entity) where E : IMongoEntityKeyable
        {
            // Inserting and returning the entity
            var inserteds = await context.InsertAsync(new List<E> { entity });
            return inserteds.FirstOrDefault();
        }

        #endregion

    }

}
