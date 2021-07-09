using System;
using System.Collections.Generic;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents basic description of wrapper of IMongoDatabase.
    /// </summary>
    public interface IMongoContextBase
    {

        /// <summary>
        /// Sets/Adds class type to mongo table connection mapping
        /// </summary>
        /// <param name="etype">Entity type</param>
        /// <param name="collection">Mongo collection</param>
        void Mappings(Type etype, string collection);

        /// <summary>
        /// Sets/Adds class type to mongo table connection mapping
        /// </summary>
        /// <param name="mappings">Entity mappings: class type to mongo table connection</param>
        void Mappings(Dictionary<Type, string> mappings);

        /// <summary>
        /// Gets typebased collection
        /// </summary>
        /// <typeparam name="E">Entity type</typeparam>
        /// <returns>Mongo collection</returns>
        IMongoCollection<E> Collection<E>();

    }

}
