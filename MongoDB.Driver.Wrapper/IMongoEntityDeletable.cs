

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents basic description of mongo table deletable entity <see cref="MongoTableEntity{T}"/>
    /// </summary>
    public interface IMongoEntityDeletable
    {

        /// <summary>
        /// Either delete current entity or not.
        /// </summary>
        bool Delete { get; set; }

    }

}
