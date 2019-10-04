using System;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CoreKit.Extensions.Collection;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents entity of mongo table <see cref="MongoTable{T}"/>
    /// </summary>
    /// <typeparam name="T">Entity key type</typeparam>
    public class MongoTableEntity<T> : IMongoEntity
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public MongoTableEntity()
        {
            // Validating table name
            if (Table.IsEmpty())
            {
                // Throwing undefined table exceprion
                throw new NotImplementedException(
                    string.Format(EnumException.UndefinedTableName, GetType().Name)
                );
            }
            // Validating key type
            if (typeof(T) != typeof(long) && typeof(T) != typeof(string))
            {
                // Throwing not supported key type exception
                throw new NotImplementedException(
                    string.Format(EnumException.UnsupportedKeyType, typeof(T), GetType().Name)
                );
            }
            // Generating entity key
            GenerateID(Code);
        }

        /// <summary>
        /// Entity key generation
        /// </summary>
        internal void GenerateID(T value)
        {
            // Initial granting 
            CodeValue = value;
            // In case of long, when value is not requested by user ...
            if (typeof(T) == typeof(long) && Convert.ToInt64(value) <= 0)
            {
                // Generating long key from current date
                CodeValue = (T)(object)Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssfffffff"));
            }
            // In case of string, when value is not requested by user ...
            if (typeof(T) == typeof(string) && Convert.ToString(value).IsEmpty())
            {
                // Generating string key from current date
                CodeValue = (T)(object)ObjectId.GenerateNewId(DateTime.Now).ToString();
            }
        }

        /// <summary>
        /// Unique key warehouse.
        /// This property will not be stored in databaze or in json transport.
        /// </summary>
        [BsonIgnore]
        [JsonIgnore]
        internal T CodeValue { get; set; }

        /// <summary>
        /// Name of the table to which entity belongs.
        /// By default it is not defined.
        /// </summary>
        [BsonIgnore]
        [JsonIgnore]
        public virtual string Table => "";

        /// <summary>
        /// Table entity unique key or bson id.
        /// </summary>
        [BsonId]
        public T Code
        {
            get
            {
                return CodeValue;
            }
            set
            {
                // Generating entity key
                GenerateID(value);
            }
        }

        /// <summary>
        /// Either delete current entity or not.
        /// This property will not be stored in databaze.
        /// </summary>
        [BsonIgnore]
        public bool Delete { get; set; }

    }

}
