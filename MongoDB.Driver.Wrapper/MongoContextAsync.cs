using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents asynchronous wrapper of IMongoDatabase.
    /// </summary>
    public partial class MongoContextAsync : MongoContextBase, IMongoContextAsync
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration <see cref="MongoConfiguration"/></param>
        /// <param name="mappings">Entity mappings: class type to mongo table connection</param>
        public MongoContextAsync(IOptions<MongoConfiguration> configuration, Dictionary<Type, string> mappings = null) : this(configuration.Value, mappings)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration <see cref="MongoConfiguration"/></param>
        /// <param name="mappings">Entity mappings: class type to mongo table connection</param>
        public MongoContextAsync(MongoConfiguration configuration, Dictionary<Type, string> mappings = null) : base(configuration, mappings)
        {
        }

    }

}
