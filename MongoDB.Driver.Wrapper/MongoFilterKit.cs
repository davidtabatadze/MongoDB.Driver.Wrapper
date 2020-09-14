using MongoDB.Bson;
using CoreKit.Extension.String;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents portion and order part of mongo filter <see cref="FindOptions{TDocument}"/>
    /// </summary>
    public class MongoFilterKit
    {

        /// <summary>
        /// Represents kit order direction enum
        /// </summary>
        public static class EnumOrderDirection
        {

            /// <summary>
            /// Ascending 
            /// </summary>
            public const string ASC = "asc";

            /// <summary>
            /// Descending
            /// </summary>
            public const string Desc = "desc";

        }

        /// <summary>
        /// Skip ordered data
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Data portion size
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Order field
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Order direction
        /// </summary>
        public string OrderDirection { get; set; }

        /// <summary>
        /// Converting to MongoDB.Driver native FindOptions
        /// </summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="dromedary">Use dromedary pascal case</param>
        /// <returns>MongoDB.Driver native FindOptions</returns>
        internal FindOptions<T> ToFindOptions<T>(bool dromedary = false)
        {
            // Fixing params
            OrderDirection = OrderDirection.IsEmpty() ? EnumOrderDirection.ASC : OrderDirection;
            OrderBy = OrderBy.IsEmpty() || OrderBy.ToLower().Contains("key") ? "_id" : OrderBy;
            // Generating and returning FindOptions
            return new FindOptions<T>
            {
                Skip = Skip,
                // In case if limit is not requested, will become null
                // So, all possible records will be materialized
                Limit = Limit == 0 ? (int?)null : Limit,
                Sort = new BsonDocument(
                    dromedary ? OrderBy.ToDromedary() : OrderBy.ToPascal(),
                    OrderDirection.TrimFullAndLower() == EnumOrderDirection.Desc ? -1 : 1
                )
            };
        }

    }

}
