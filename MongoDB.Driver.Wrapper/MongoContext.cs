﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace MongoDB.Driver.Wrapper
{


    /// <summary>
    /// Represents synchronous wrapper of IMongoDatabase.
    /// </summary>
    public partial class MongoContext : MongoContextBase, IMongoContext
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration <see cref="MongoConfiguration"/></param>
        /// <param name="mappings">Entity mappings: class type to mongo table connection</param>
        public MongoContext(IOptions<MongoConfiguration> configuration, Dictionary<Type, string> mappings = null) : this(configuration.Value, mappings)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration <see cref="MongoConfiguration"/></param>
        /// <param name="mappings">Entity mappings: class type to mongo table connection</param>
        public MongoContext(MongoConfiguration configuration, Dictionary<Type, string> mappings = null) : base(configuration, mappings)
        {
        }

    }

}