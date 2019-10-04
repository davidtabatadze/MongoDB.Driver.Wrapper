

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents basic description of mongo table entity <see cref="MongoTableEntity{T}"/>
    /// </summary>
    public interface IMongoEntity
    {

        /// <summary>
        /// Name of the table to which entity belongs
        /// </summary>
        string Table { get; }

    }

}
