using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoreKit.Sync;
using CoreKit.Extension.Collection;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents CRUD update functionality for <see cref="MongoContext"/>
    /// </summary>
    public partial class MongoContext
    {

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Saved entities</returns>
        public new List<E> Save<E>(List<E> entities) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Save and return the data
            return SyncKit.Run(() => base.Save(entities));
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Saved entity</returns>
        public new E Save<E>(E entity) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Save and return the data
            return SyncKit.Run(() => base.Save(entity));
        }

    }

    /// <summary>
    /// Represents CRUD update functionality for <see cref="MongoContextAsync"/>
    /// </summary>
    public partial class MongoContextAsync
    {

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Saved entities</returns>
        public new async Task<List<E>> Save<E>(List<E> entities) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Save and return the data
            return await base.Save(entities);
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Saved entity</returns>
        public new async Task<E> Save<E>(E entity) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Save and return the data
            return await base.Save(entity);
        }

    }

    /// <summary>
    /// Represents CRUD update functionality for <see cref="MongoContextBase"/>
    /// </summary>
    public partial class MongoContextBase
    {

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Saved entities</returns>
        internal protected async Task<List<E>> Save<E>(List<E> entities) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Validating entities
            if (entities.TrimEmpty().HasValue())
            {
                // First remove all existing records
                await Delete(entities);
                // Then insert the data
                await Insert(entities.Where(e => !e.Delete).ToList());
            }
            // Retunring entities
            return entities;
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Saved entity</returns>
        internal protected async Task<E> Save<E>(E entity) where E : IMongoEntityKeyable, IMongoEntityDeletable
        {
            // Save and return the entity
            var saveds = await Save(new List<E> { entity });
            return saveds.FirstOrDefault();
        }

    }

}
