

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents basic description of mongo table deletable entity.
    /// </summary>
    public interface IMongoEntityDeletable
    {

        /// <summary>
        /// Either delete current entity or not.
        /// </summary>
        bool Delete { get; set; }

    }

}