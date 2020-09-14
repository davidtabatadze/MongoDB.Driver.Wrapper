using System;
using CoreKit.Extension.String;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents additinal functionality for the wrapper
    /// </summary>
    public class MongoHelper
    {

        /// <summary>
        /// Validates mongo entity for acceptable key type
        /// </summary>
        /// <typeparam name="T">Type of validation object</typeparam>
        /// <param name="entity">Validation object</param>
        public static void ValidateMongoEntity<T>()
        {
            // Only long and string are acceptable
            if (
                typeof(T) != typeof(long) &&
                typeof(T) != typeof(string) &&
                (!typeof(IMongoEntityKeyable<long>).IsAssignableFrom(typeof(T))) &&
                (!typeof(IMongoEntityKeyable<string>).IsAssignableFrom(typeof(T)))
            )
            {
                // Otherwise throwing exception
                throw new NotImplementedException(
                    string.Format(
                        "Presented key type is not implemented for entity '{0}'. " +
                        "Only 'long' and 'string' are acceptable.",
                        typeof(T).Name
                    )
                );
            }
        }

        /// <summary>
        /// Generates fresh key for keyable entity
        /// </summary>
        /// <typeparam name="T">Type of key</typeparam>
        /// <param name="entity">keyable entity</param>
        public static void GenerateKey<T>(IMongoEntityKeyable<T> entity)
        {
            // Validating key type
            ValidateMongoEntity<T>();
            // In case of long, when value is not requested by user ...
            if (typeof(T) == typeof(long) && Convert.ToInt64(entity.Key) <= 0)
            {
                // Generating long key from current date
                entity.Key = (T)(object)Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssfffffff"));
            }
            // In case of string, when value is not requested by user ...
            if (typeof(T) == typeof(string) && Convert.ToString(entity.Key).IsEmpty())
            {
                // Generating string key from current date
                entity.Key = (T)(object)DateTime.Now.ToString("yyMMddHHmmssfffffff");//ObjectId.GenerateNewId(DateTime.Now).ToString();
            }
        }

    }

}
