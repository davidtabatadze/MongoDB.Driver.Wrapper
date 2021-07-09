using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CoreKit.Sync;
using CoreKit.Extension.Collection;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents CRUD create functionality for <see cref="MongoContext"/>
    /// </summary>
    public partial class MongoContext
    {

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Inserted entities</returns>
        public new List<E> Insert<E>(List<E> entities) where E : IMongoEntityKeyable
        {
            // Inserting the data
            return SyncKit.Run(() => base.Insert(entities));
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Inserted entity</returns>
        public new E Insert<E>(E entity) where E : IMongoEntityKeyable
        {
            // Inserting the data
            return SyncKit.Run(() => base.Insert(entity));
        }

    }

    /// <summary>
    /// Represents CRUD create functionality for <see cref="MongoContextAsync"/>
    /// </summary>
    public partial class MongoContextAsync
    {

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Inserted entities</returns>
        public new async Task<List<E>> Insert<E>(List<E> entities) where E : IMongoEntityKeyable
        {
            // Inserting the data
            return await base.Insert(entities);
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Inserted entity</returns>
        public new async Task<E> Insert<E>(E entity) where E : IMongoEntityKeyable
        {
            // Inserting the data
            return await base.Insert(entity);
        }

    }

    /// <summary>
    /// Represents CRUD create functionality for <see cref="MongoContextBase"/>
    /// </summary>
    public partial class MongoContextBase
    {

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Inserted entities</returns>
        internal protected async Task<List<E>> Insert<E>(List<E> entities) where E : IMongoEntityKeyable
        {
            // Validating E
            MongoHelper.ValidateMongoEntity<E>();
            // Validating entities
            if (entities.TrimEmpty().HasValue())
            {
                // Inserting the data
                var result = Session == null ?
                             Collection<E>().InsertManyAsync(entities) :
                             Collection<E>().InsertManyAsync(Session, entities);
                await result;
            }
            // Retunring entities
            return entities;
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Inserted entity</returns>
        internal protected async Task<E> Insert<E>(E entity) where E : IMongoEntityKeyable
        {
            // Inserting and returning the entity
            var inserteds = await Insert(new List<E> { entity });
            return inserteds.FirstOrDefault();
        }

    }

}
