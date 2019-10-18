using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Driver.Wrapper.Serializer
{

    /// <summary>
    /// Represents bson transformer of date.
    /// Turning Utc to Unspecified original.
    /// </summary>
    public class MongoSerializerDateTimeLocal : DateTimeSerializer
    {

        /// <summary>
        /// Overwrite Utc value to be Unspecified
        /// </summary>
        /// <param name="context">Bson deserialization context</param>
        /// <param name="args">Bson deserialization arguments</param>
        /// <returns>DateTimeKind.Unspecified value</returns>
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var obj = base.Deserialize(context, args);
            return new DateTime(obj.Ticks, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Overwrites non Utc value to be Utc
        /// </summary>
        /// <param name="context">Bson serialization context</param>
        /// <param name="args">Bson serialization arguments</param>
        /// <param name="value">Stored DateTimeKind.Unspecified value</param>
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            var utcValue = new DateTime(value.Ticks, DateTimeKind.Utc);
            base.Serialize(context, args, utcValue);
        }

    }

}
