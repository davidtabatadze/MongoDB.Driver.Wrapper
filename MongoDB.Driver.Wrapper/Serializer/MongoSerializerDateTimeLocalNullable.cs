using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents bson transformer of nullable datetime.
    /// Turning Utc to Unspecified original.
    /// </summary>
    public class MongoSerializerDateTimeLocalNullable : IBsonSerializer
    {

        /// <summary>
        /// Represents local datetime serializer object
        /// </summary>
        private DateTimeSerializer Serializer = new MongoSerializerDateTimeLocal();

        /// <summary>
        /// Represents value type of object to be derialized.
        /// This case (DateTime?).
        /// </summary>
        public Type ValueType => typeof(DateTime?);

        /// <summary>
        /// Executes deserialization
        /// </summary>
        /// <param name="context">Bson deserialization context</param>
        /// <param name="args">Bson deserialization arguments</param>
        /// <returns>DateTimeKind.Unspecified value</returns>
        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Null)
            {
                context.Reader.ReadNull();
                return null;
            }
            else
            {
                return Serializer.Deserialize(context, args);
            }
        }

        /// <summary>
        /// Executes serialization
        /// </summary>
        /// <param name="context">Bson serialization context</param>
        /// <param name="args">Bson serialization arguments</param>
        /// <param name="value">Stored DateTimeKind.Unspecified value</param>
        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value is null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                Serializer.Serialize(context, args, (DateTime)value);
            }
        }

    }

}
