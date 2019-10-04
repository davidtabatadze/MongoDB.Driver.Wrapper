

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents mongo configuration <see cref="MongoContext"/>
    /// </summary>
    public class MongoConfiguration
    {

        /// <summary>
        /// Server address (including user, password and port).
        /// Example: "mongodb://username:userpassword@mongoserver.com:27017"
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Actual databaze name
        /// </summary>
        public string Database { get; set; }

    }

}
