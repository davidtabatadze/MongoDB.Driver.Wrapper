

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents mongo configuration.
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

        /// <summary>
        /// Prevents transaction support if true
        /// </summary>
        public bool UseTransactions { get; set; }

        /// <summary>
        /// Use lower camel case (Dromedary case) for mongo entity properties
        /// </summary>
        public bool UseLowerCamelCaseProperties { get; set; }

        /// <summary>
        /// Enables simple terminal logging for posted requests
        /// </summary>
        public bool EnableCommandLog { get; set; }

    }

}
