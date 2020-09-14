using MongoDB.Bson;
using CoreKit.Sync;
using System.Threading.Tasks;
using CoreKit.Extension.String;

namespace MongoDB.Driver.Wrapper.Query
{

    /// <summary>
    /// Represents querying functionality for <see cref="MongoContext"/>
    /// </summary>
    public static class MongoContextQuery
    {

        #region Sync

        /// <summary>
        /// Executes plain mongo shell query
        /// </summary>
        /// <param name="query">query</param>
        public static void ExecuteQuery(this MongoContext context, string query)
        {
            // RunCommand..
            SyncKit.Run(() => context.ExecuteQueryAsync(query));
        }

        #endregion

        #region Async

        /// <summary>
        /// Executes plain mongo shell query
        /// </summary>
        /// <param name="query">query</param>
        /// <returns>Empty</returns>
        public static async Task ExecuteQueryAsync(this MongoContext context, string query)
        {
            // Only if query exists
            if (query.HasValue())
            {
                // We do execute it
                var result = context.Session == null ?
                             await context.Database.RunCommandAsync(new JsonCommand<BsonDocument>(query)) :
                             await context.Database.RunCommandAsync(context.Session, new JsonCommand<BsonDocument>(query));
            }
        }

        #endregion

    }

}
