using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoreKit.Sync;
using CoreKit.Extension.Collection;

namespace MongoDB.Driver.Wrapper.CRUD
{

    /// <summary>
    /// Represents CRUD update functionality for <see cref="MongoContext"/>
    /// </summary>
    public static class MongoContextUpdate
    {

        #region Sync

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Saved entities</returns>
        public static List<E> Save<E>(this MongoContext context, List<E> entities) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Save and return the data
            return SyncKit.Run(() => context.SaveAsync(entities));
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Saved entity</returns>
        public static E Save<E>(this MongoContext context, E entity) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Save and return the data
            return SyncKit.Run(() => context.SaveAsync(entity));
        }

        #endregion

        #region Async

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Saved entities</returns>
        public static async Task<List<E>> SaveAsync<E>(this MongoContext context, List<E> entities) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Validating entities
            if (entities.TrimEmpty().HasValue())
            {
                // First remove all existing records
                await context.DeleteAsync(entities);
                // Then insert the data
                await context.InsertAsync(entities.Where(e => !e.Delete).ToList());
            }
            // Retunring entities
            return entities;
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="context">Mongo context</param>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Saved entity</returns>
        public static async Task<E> SaveAsync<E>(this MongoContext context, E entity) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Save and return the entity
            var saveds = await context.SaveAsync(new List<E> { entity });
            return saveds.FirstOrDefault();
        }

        #endregion

    }

}
