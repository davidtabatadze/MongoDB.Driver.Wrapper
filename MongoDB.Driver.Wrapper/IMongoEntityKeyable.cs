

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents basic description of mongo keyable entity.
    /// </summary>
    public interface IMongoEntityKeyable
    {
    }

    /// <summary>
    /// Represents basic description of mongo type keyable entity.
    /// </summary>
    /// <typeparam name="T">Type of key</typeparam>
    public interface IMongoEntityKeyable<T> : IMongoEntityKeyable
    {

        /// <summary>
        /// Table entity unique key or bson id.
        /// </summary>
        T Key { get; set; }

    }

}