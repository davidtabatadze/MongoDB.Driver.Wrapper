using System.Threading.Tasks;
using System.Collections.Generic;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents basic description of asynchronous wrapper of IMongoDatabase.
    /// </summary>
    public interface IMongoContextAsync : IMongoContextBase
    {

        #region CRUD - Create

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Inserted entities</returns>
        Task<List<E>> Insert<E>(List<E> entities) where E : IMongoEntityKeyable;

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Inserted entity</returns>
        Task<E> Insert<E>(E entity) where E : IMongoEntityKeyable;

        #endregion

        #region CRUD - Read

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo records</returns>
        Task<List<E>> Load<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        Task<List<E>> Load<E>(List<long> keys) where E : IMongoEntityKeyable<long>;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        Task<List<E>> Load<E>(List<string> keys) where E : IMongoEntityKeyable<string>;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo record</returns>
        Task<E> Get<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        Task<E> Get<E>(long key) where E : IMongoEntityKeyable<long>;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        Task<E> Get<E>(string key) where E : IMongoEntityKeyable<string>;

        /// <summary>
        /// Get data count
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Data count</returns>
        Task<long> Count<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable;

        #endregion

        #region CRUD - Update

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Saved entities</returns>
        Task<List<E>> Save<E>(List<E> entities) where E : IMongoEntityKeyable, IMongoEntityDeletable;

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Saved entity</returns>
        Task<E> Save<E>(E entity) where E : IMongoEntityKeyable, IMongoEntityDeletable;

        #endregion

        #region CRUD - Delete

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Empty</returns>
        Task Delete<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Empty</returns>
        Task Delete<E>(List<E> entities) where E : IMongoEntityKeyable;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Empty</returns>
        Task Delete<E>(E entity) where E : IMongoEntityKeyable;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        /// <returns>Empty</returns>
        Task Delete<E>(List<long> keys) where E : IMongoEntityKeyable<long>;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        /// <returns>Empty</returns>
        Task Delete<E>(long key) where E : IMongoEntityKeyable<long>;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        /// <returns>Empty</returns>
        Task Delete<E>(List<string> keys) where E : IMongoEntityKeyable<string>;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        /// <returns>Empty</returns>
        Task Delete<E>(string key) where E : IMongoEntityKeyable<string>;

        #endregion

        #region Queries

        /// <summary>
        /// Executes plain mongo shell query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Empty</returns>
        Task Query(string query);

        #endregion

        #region Sessions (transactions)

        /// <summary>
        /// Starts session transaction
        /// </summary>
        /// <returns>Empty</returns>
        Task SessionStart();

        /// <summary>
        /// Commits session transaction
        /// </summary>
        /// <returns>Empty</returns>
        Task SessionCommit();

        /// <summary>
        /// Aborts session transaction
        /// </summary>
        /// <returns>Empty</returns>
        Task SessionAbort();

        #endregion

    }

}
