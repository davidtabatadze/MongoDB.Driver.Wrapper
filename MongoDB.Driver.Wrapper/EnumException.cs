

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents all possible definitions of exception
    /// </summary>
    internal class EnumException
    {

        /// <summary>
        /// Table is not defined
        /// </summary>
        public const string UndefinedTableName = "Destination mongo table is not defined for mongo entity ({0}).";

        /// <summary>
        /// Table key type is not supported
        /// </summary>
        public const string UnsupportedKeyType = "Given key type ({0}) is not supported for mongo entity ({1}).";

        /// <summary>
        /// Table type is not supported
        /// </summary>
        public const string UnsupportedTableType = "Given type ({0}) is not supported for mongo table.";

    }

}
