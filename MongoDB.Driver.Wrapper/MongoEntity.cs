using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents entity of any type of key for internal purposes
    /// </summary>
    internal class MongoEntity : IMongoEntityKeyable<object>
    {

        /// <summary>
        /// Unique key 
        /// </summary>
        [BsonId]
        public object Key { get; set; }

    }

    /// <summary>
    /// Represents entity of mongo table
    /// </summary>
    /// <typeparam name="T">Entity key type</typeparam>
    public class MongoEntity<T> : IMongoEntity<T>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public MongoEntity()
        {
            // Generating entity key
            MongoHelper.GenerateKey(this);
        }

        /// <summary>
        /// Table entity unique key (bson id).
        /// </summary>
        [BsonId]
        public T Key { get; set; }

        /// <summary>
        /// Either delete current entity or not.
        /// This property will not be stored in databaze.
        /// </summary>
        [BsonIgnore]
        public bool Delete { get; set; }

    }

}
