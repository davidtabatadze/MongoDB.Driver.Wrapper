using MongoDB.Bson;
using CoreKit.Sync;
using System.Threading.Tasks;
using CoreKit.Extension.String;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents querying functionality for <see cref="MongoContext"/>
    /// </summary>
    public partial class MongoContext
    {

        /// <summary>
        /// Executes plain mongo shell query
        /// </summary>
        /// <param name="query">Query</param>
        public new void Query(string query)
        {
            // Execute
            SyncKit.Run(() => base.Query(query));
        }

    }

    /// <summary>
    /// Represents querying functionality for <see cref="MongoContextAsync"/>
    /// </summary>
    public partial class MongoContextAsync
    {

        /// <summary>
        /// Executes plain mongo shell query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Empty</returns>
        public new async Task Query(string query)
        {
            // Execute
            await base.Query(query);
        }

    }

    /// <summary>
    /// Represents querying functionality for <see cref="MongoContextBase"/>
    /// </summary>
    public partial class MongoContextBase
    {

        /// <summary>
        /// Executes plain mongo shell query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Empty</returns>
        internal protected async Task Query(string query)
        {
            // Only if query exists
            if (query.HasValue())
            {
                // We do execute it
                var result = Session == null ?
                             await Database.RunCommandAsync(new JsonCommand<BsonDocument>(query)) :
                             await Database.RunCommandAsync(Session, new JsonCommand<BsonDocument>(query));
            }
        }

    }

}
