

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents basic description of mongo table entity.
    /// </summary>
    /// <typeparam name="T">Type of key</typeparam>
    public interface IMongoEntity<T> : IMongoEntityKeyable<T>, IMongoEntityDeletable
    {
    }

}
