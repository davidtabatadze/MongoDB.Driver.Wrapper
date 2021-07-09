using System.Collections.Generic;

namespace MongoDB.Driver.Wrapper
{

    // <summary>
    /// Represents basic description of synchronous wrapper of IMongoDatabase.
    /// </summary>
    public interface IMongoContext : IMongoContextBase
    {

        #region CRUD - Create

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Inserted entities</returns>
        List<E> Insert<E>(List<E> entities) where E : IMongoEntityKeyable;

        /// <summary>
        /// Insert data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Inserted entity</returns>
        E Insert<E>(E entity) where E : IMongoEntityKeyable;

        #endregion

        #region CRUD - Read

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo records</returns>
        List<E> Load<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        List<E> Load<E>(List<long> keys) where E : IMongoEntityKeyable<long>;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Record keys</param>
        /// <returns>Mongo records</returns>
        List<E> Load<E>(List<string> keys) where E : IMongoEntityKeyable<string>;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <param name="kit">Mongo kit</param>
        /// <returns>Mongo record</returns>
        E Get<E>(FilterDefinition<E> filter = null, MongoFilterKit kit = null) where E : IMongoEntityKeyable;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        E Get<E>(long key) where E : IMongoEntityKeyable<long>;

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Record key</param>
        /// <returns>Mongo record</returns>
        E Get<E>(string key) where E : IMongoEntityKeyable<string>;

        /// <summary>
        /// Get data count
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        /// <returns>Data count</returns>
        long Count<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable;

        #endregion

        #region CRUD - Update

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        /// <returns>Saved entities</returns>
        List<E> Save<E>(List<E> entities) where E : IMongoEntityKeyable, IMongoEntityDeletable;

        /// <summary>
        /// Save data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        /// <returns>Saved entity</returns>
        E Save<E>(E entity) where E : IMongoEntityKeyable, IMongoEntityDeletable;

        #endregion

        #region CRUD - Delete

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="filter">Mongo filter</param>
        void Delete<E>(FilterDefinition<E> filter = null) where E : IMongoEntityKeyable;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entities">Mongo entities</param>
        void Delete<E>(List<E> entities) where E : IMongoEntityKeyable;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="entity">Mongo entity</param>
        void Delete<E>(E entity) where E : IMongoEntityKeyable;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        void Delete<E>(List<long> keys) where E : IMongoEntityKeyable<long>;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        void Delete<E>(long key) where E : IMongoEntityKeyable<long>;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="keys">Mongo entity keys</param>
        void Delete<E>(List<string> keys) where E : IMongoEntityKeyable<string>;

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="E">Type of data</typeparam>
        /// <param name="key">Mongo entity key</param>
        void Delete<E>(string key) where E : IMongoEntityKeyable<string>;

        #endregion

        #region Queries

        /// <summary>
        /// Executes plain mongo shell query
        /// </summary>
        /// <param name="query">Query</param>
        void Query(string query);

        #endregion

        #region Sessions (transactions)

        /// <summary>
        /// Starts session transaction
        /// </summary>
        void SessionStart();

        /// <summary>
        /// Commits session transaction
        /// </summary>
        void SessionCommit();

        /// <summary>
        /// Aborts session transaction
        /// </summary>
        void SessionAbort();

        #endregion

    }

}
