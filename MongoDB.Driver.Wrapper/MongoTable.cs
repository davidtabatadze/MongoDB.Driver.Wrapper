using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreKit.Extension.Collection;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents mongo typebased table manager
    /// </summary>
    /// <typeparam name="T">Table key type</typeparam>
    public class MongoTable<T> : IDisposable
    {

        /// <summary>
        /// Ending class lifecycle
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Table content</param>
        public MongoTable(IMongoCollection<T> content)
        {
            // Validating entity key type
            if (!(
                typeof(T).IsSubclassOf(typeof(MongoTableEntity<long>)) ||
                typeof(T).IsSubclassOf(typeof(MongoTableEntity<string>)) ||
                false
            ))
            {
                // Throwing not supported key type exception
                throw new NotImplementedException(
                    string.Format(EnumException.UnsupportedTableType, typeof(T).Name)
                );
            }
            //
            Content = content;
        }

        /// <summary>
        /// Table content.
        /// MongoDB.Driver IMongoCollection standard functionality.
        /// </summary>
        public IMongoCollection<T> Content { get; set; }

        ///// <summary>
        ///// Table filter.
        ///// MongoDB.Driver FilterDefinitionBuilder standard functionality.
        ///// </summary>
        //public FilterDefinitionBuilder<T> Filter = new FilterDefinitionBuilder<T>();

        /// <summary>
        /// Getting fixed filter
        /// </summary>
        /// <param name="filter">Raw filter</param>
        /// <returns>Fixed filter</returns>
        private FilterDefinition<T> FixFilter(FilterDefinition<T> filter = null)
        {
            // If filter is not defined
            if (filter == null)
            {
                // We are fixing it by default value
                filter = Builders<T>.Filter.Empty;
            }
            // returning fixed filter
            return filter;
        }

        /// <summary>
        /// Get data count
        /// </summary>
        /// <param name="filter">data filter</param>
        /// <returns>data count</returns>
        public long Count(FilterDefinition<T> filter = null)
        {
            // Finxing filter
            filter = FixFilter(filter);
            // Returning data count
            return Content.CountDocuments(filter);
        }

        /// <summary>
        /// Get data count
        /// </summary>
        /// <param name="filter">data filter</param>
        /// <returns>data count</returns>
        public async Task<long> CountAsync(FilterDefinition<T> filter = null)
        {
            // Finxing filter
            filter = FixFilter(filter);
            // Returning data count
            return await Content.CountDocumentsAsync(filter);
        }

        /// <summary>
        /// Loading data
        /// </summary>
        /// <param name="filter">data filter</param>
        /// <param name="kit">data filter kit</param>
        /// <returns>filtered data</returns>
        public List<T> Load(FilterDefinition<T> filter = null, MongoFilterKit kit = null)
        {
            // Finxing filter
            filter = FixFilter(filter);
            // Defining options according kit
            var options = kit == null ? new FindOptions<T> { } : kit.ToFindOptions<T>();
            // In case if limit is not requested
            if (options.Limit == 0)
            {
                // Limit will become null
                // This means that all possible records will be materialized
                options.Limit = null;
            }
            // In case if sorting is not defined
            if (options.Sort == null)
            {
                // We create default sorting parameters
                options.Sort = new BsonDocument("_id", 1);
            }
            // Generating data query
            var data = Content.FindSync(filter, options);
            // Returning materialized data
            return data.ToList();
        }

        /// <summary>
        /// Loading data
        /// </summary>
        /// <param name="filter">data filter</param>
        /// <param name="kit">data filter kit</param>
        /// <returns>filtered data</returns>
        public async Task<List<T>> LoadAsync(FilterDefinition<T> filter = null, MongoFilterKit kit = null)
        {
            // Finxing filter
            filter = FixFilter(filter);
            // Defining options according kit
            var options = kit == null ? new FindOptions<T> { } : kit.ToFindOptions<T>();
            // In case if limit is not requested
            if (options.Limit == 0)
            {
                // Limit will become null
                // This means that all possible records will be materialized
                options.Limit = null;
            }
            // In case if sorting is not defined
            if (options.Sort == null)
            {
                // We create default sorting parameters
                options.Sort = new BsonDocument("_id", 1);
            }
            // Generating data query
            var data = await Content.FindAsync(filter, options);
            // Returning materialized data
            return data.ToList();
        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <param name="filter">Record filter</param>
        /// <returns>Filtered item</returns>
        public T Get(FilterDefinition<T> filter)
        {
            // Loading data with only one record kit
            var data = Load(filter, new MongoFilterKit { Skip = 0, Limit = 1 });
            // Returning that single record
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <param name="filter">Record filter</param>
        /// <returns>Filtered item</returns>
        public async Task<T> GetAsync(FilterDefinition<T> filter)
        {
            // Loading data with only one record kit
            var data = await LoadAsync(filter, new MongoFilterKit { Skip = 0, Limit = 1 });
            // Returning that single record
            return data.FirstOrDefault();
        }

        /// <summary>
        /// Insert item
        /// </summary>
        /// <param name="item">data item</param>
        internal void Insert(T item)
        {
            // If only item exists
            if (item != null)
            {
                // We do save it
                Insert(new List<T> { item });
            }
        }

        /// <summary>
        /// Insert item
        /// </summary>
        /// <param name="item">data item</param>
        /// <returns>Nothing</returns>
        internal async Task InsertAsync(T item)
        {
            // If only item exists
            if (item != null)
            {
                // We do save it
                await InsertAsync(new List<T> { item });
            }
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="items">data items</param>
        internal void Insert(IEnumerable<T> items)
        {
            // If only collection is not empty
            if (items.HasValue())
            {
                // We do save it
                Content.InsertMany(items);
            }
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="items">data items</param>
        /// <returns>Nothing</returns>
        internal async Task InsertAsync(IEnumerable<T> items)
        {
            // If only collection is not empty
            if (items.HasValue())
            {
                // We do save it
                await Content.InsertManyAsync(items);
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="filter">Data filter</param>
        public void Delete(FilterDefinition<T> filter = null)
        {
            // Finxing filter
            filter = FixFilter(filter);
            // Delete data by filter
            Content.DeleteMany(filter);
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="filter">Data filter</param>
        /// <returns>Nothing</returns>
        public async Task DeleteAsync(FilterDefinition<T> filter = null)
        {
            // Finxing filter
            filter = FixFilter(filter);
            // Delete data by filter
            await Content.DeleteManyAsync(filter);
        }

    }

    /// <summary>
    /// Represents mongo typebased table additional functionalities
    /// </summary>
    public static class MongoTableExtension
    {

        /// <summary>
        /// Save item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entity">Item</param>
        public static void SaveEntityLong<T>(this MongoTable<T> table, T entity) where T : MongoTableEntity<long>
        {
            // Only if item exists
            if (entity != null)
            {
                // We do save the item
                table.SaveEntityLong(new List<T> { entity });
            }
        }

        /// <summary>
        /// Save item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entity">Item</param>
        /// <returns>Nothing</returns>
        public static async Task SaveEntityLongAsync<T>(this MongoTable<T> table, T entity) where T : MongoTableEntity<long>
        {
            // Only if item exists
            if (entity != null)
            {
                // We do save the item
                await table.SaveEntityLongAsync(new List<T> { entity });
            }
        }

        /// <summary>
        /// Save Data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entities">Data</param>
        public static void SaveEntityLong<T>(this MongoTable<T> table, IEnumerable<T> entities) where T : MongoTableEntity<long>
        {
            // Only if data exists
            if (entities.HasValue())
            {
                // First remove all existing records
                table.DeleteEntityLong(entities);
                // Then insert the data
                table.Insert(entities.Where(e => !e.Delete));
            }
        }

        /// <summary>
        /// Save Data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entities">Data</param>
        /// <returns>Nothing</returns>
        public static async Task SaveEntityLongAsync<T>(this MongoTable<T> table, IEnumerable<T> entities) where T : MongoTableEntity<long>
        {
            // Only if data exists
            if (entities.HasValue())
            {
                // First remove all existing records
                await table.DeleteEntityLongAsync(entities);
                // Then insert the data
                await table.InsertAsync(entities.Where(e => !e.Delete));
            }
        }

        /// <summary>
        /// Save item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entity">Item</param>
        public static void SaveEntityString<T>(this MongoTable<T> table, T entity) where T : MongoTableEntity<string>
        {
            // Only if item exists
            if (entity != null)
            {
                // We do save the item
                table.SaveEntityString(new List<T> { entity });
            }
        }

        /// <summary>
        /// Save item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entity">Item</param>
        /// <returns>Nothing</returns>
        public static async Task SaveEntityStringAsync<T>(this MongoTable<T> table, T entity) where T : MongoTableEntity<string>
        {
            // Only if item exists
            if (entity != null)
            {
                // We do save the item
                await table.SaveEntityStringAsync(new List<T> { entity });
            }
        }

        /// <summary>
        /// Save Data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="items">Data</param>
        public static void SaveEntityString<T>(this MongoTable<T> table, IEnumerable<T> items) where T : MongoTableEntity<string>
        {
            // Only if data exists
            if (items.HasValue())
            {
                // First remove all existing records
                table.DeleteEntityString(items);
                // Then insert the data
                table.Insert(items.Where(e => !e.Delete));
            }
        }

        /// <summary>
        /// Save Data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="items">Data</param>
        /// <returns>Nothing</returns>
        public static async Task SaveEntityStringAsync<T>(this MongoTable<T> table, IEnumerable<T> items) where T : MongoTableEntity<string>
        {
            // Only if data exists
            if (items.HasValue())
            {
                // First remove all existing records
                await table.DeleteEntityStringAsync(items);
                // Then insert the data
                await table.InsertAsync(items.Where(e => !e.Delete));
            }
        }

        /// <summary>
        /// Load data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="codes">Data keys</param>
        /// <returns>Filtered data</returns>
        public static List<T> LoadEntity<T>(this MongoTable<T> table, IEnumerable<long> codes) where T : MongoTableEntity<long>
        {
            return table.Load(Builders<T>.Filter.Where(i => codes.Contains(i.Code)));
        }

        /// <summary>
        /// Load data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="codes">Data keys</param>
        /// <returns>Filtered data</returns>
        public static async Task<List<T>> LoadEntityAsync<T>(this MongoTable<T> table, IEnumerable<long> codes) where T : MongoTableEntity<long>
        {
            return await table.LoadAsync(Builders<T>.Filter.Where(i => codes.Contains(i.Code)));
        }

        /// <summary>
        /// Load data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="codes">Data keys</param>
        /// <returns>Filtered data</returns>
        public static List<T> LoadEntity<T>(this MongoTable<T> table, IEnumerable<string> codes) where T : MongoTableEntity<string>
        {
            return table.Load(Builders<T>.Filter.Where(i => codes.Contains(i.Code)));
        }

        /// <summary>
        /// Load data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="codes">Data keys</param>
        /// <returns>Filtered data</returns>
        public static async Task<List<T>> LoadEntityAsync<T>(this MongoTable<T> table, IEnumerable<string> codes) where T : MongoTableEntity<string>
        {
            return await table.LoadAsync(Builders<T>.Filter.Where(i => codes.Contains(i.Code)));
        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="code">Item key</param>
        /// <returns>Filtered item</returns>
        public static T GetEntity<T>(this MongoTable<T> table, long code) where T : MongoTableEntity<long>
        {
            return table.Get(Builders<T>.Filter.Where(i => i.Code == code));
        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="code">Item key</param>
        /// <returns>Filtered item</returns>
        public static async Task<T> GetEntityAsync<T>(this MongoTable<T> table, long code) where T : MongoTableEntity<long>
        {
            return await table.GetAsync(Builders<T>.Filter.Where(i => i.Code == code));
        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="code">Item key</param>
        /// <returns>Filtered item</returns>
        public static T GetEntity<T>(this MongoTable<T> table, string code) where T : MongoTableEntity<string>
        {
            return table.Get(Builders<T>.Filter.Where(i => i.Code == code));
        }

        /// <summary>
        /// Get item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="code">Item key</param>
        /// <returns>Filtered item</returns>
        public static async Task<T> GetEntityAsync<T>(this MongoTable<T> table, string code) where T : MongoTableEntity<string>
        {
            return await table.GetAsync(Builders<T>.Filter.Where(i => i.Code == code));
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="code">Item key</param>
        public static void DeleteEntity<T>(this MongoTable<T> table, long code) where T : MongoTableEntity<long>
        {
            // Do delete the item
            table.DeleteEntity(new List<long> { code });
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="code">Item key</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteEntityAsync<T>(this MongoTable<T> table, long code) where T : MongoTableEntity<long>
        {
            // Do delete the item
            await table.DeleteEntityAsync(new List<long> { code });
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="codes">Data keys</param>
        public static void DeleteEntity<T>(this MongoTable<T> table, IEnumerable<long> codes) where T : MongoTableEntity<long>
        {
            // Only if keys exists
            if (codes.HasValue())
            {
                // We do delete the data
                table.Delete(Builders<T>.Filter.Where(i => codes.Contains(i.Code)));
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="codes">Data keys</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteEntityAsync<T>(this MongoTable<T> table, IEnumerable<long> codes) where T : MongoTableEntity<long>
        {
            // Only if keys exists
            if (codes.HasValue())
            {
                // We do delete the data
                await table.DeleteAsync(Builders<T>.Filter.Where(i => codes.Contains(i.Code)));
            }
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="code">Item key</param>
        public static void DeleteEntity<T>(this MongoTable<T> table, string code) where T : MongoTableEntity<string>
        {
            // Only if key exists
            if (code.HasValue())
            {
                // We do delete the item
                table.DeleteEntity(new List<string> { code });
            }
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="code">Item key</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteEntityAsync<T>(this MongoTable<T> table, string code) where T : MongoTableEntity<string>
        {
            // Only if key exists
            if (code.HasValue())
            {
                // We do delete the item
                await table.DeleteEntityAsync(new List<string> { code });
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="codes">Data keys</param>
        public static void DeleteEntity<T>(this MongoTable<T> table, IEnumerable<string> codes) where T : MongoTableEntity<string>
        {
            // Only if keys exists
            if (codes.HasValue())
            {
                // We do delete the data
                table.Delete(Builders<T>.Filter.Where(i => codes.Contains(i.Code)));
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="codes">Data keys</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteEntityAsync<T>(this MongoTable<T> table, IEnumerable<string> codes) where T : MongoTableEntity<string>
        {
            // Only if keys exists
            if (codes.HasValue())
            {
                // We do delete the data
                await table.DeleteAsync(Builders<T>.Filter.Where(i => codes.Contains(i.Code)));
            }
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entity">Item</param>
        public static void DeleteEntityLong<T>(this MongoTable<T> table, T entity) where T : MongoTableEntity<long>
        {
            // Only if item exists
            if (entity != null)
            {
                // We do delete the item
                table.DeleteEntity(entity.Code);
            }
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entity">Item</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteEntityLongAsync<T>(this MongoTable<T> table, T entity) where T : MongoTableEntity<long>
        {
            // Only if item exists
            if (entity != null)
            {
                // We do delete the item
                await table.DeleteEntityAsync(entity.Code);
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entities">Data</param>
        public static void DeleteEntityLong<T>(this MongoTable<T> table, IEnumerable<T> entities) where T : MongoTableEntity<long>
        {
            // Only if data exists
            if (entities.HasValue())
            {
                // We do delete the data
                table.DeleteEntity(entities.Select(e => e.Code));
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entities">Data</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteEntityLongAsync<T>(this MongoTable<T> table, IEnumerable<T> entities) where T : MongoTableEntity<long>
        {
            // Only if data exists
            if (entities.HasValue())
            {
                // We do delete the data
                await table.DeleteEntityAsync(entities.Select(e => e.Code));
            }
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entity">Item</param>
        public static void DeleteEntityString<T>(this MongoTable<T> table, T entity) where T : MongoTableEntity<string>
        {
            // Only if item exists
            if (entity != null)
            {
                // We do delete the item
                table.DeleteEntity(entity.Code);
            }
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entity">Item</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteEntityStringAsync<T>(this MongoTable<T> table, T entity) where T : MongoTableEntity<string>
        {
            // Only if item exists
            if (entity != null)
            {
                // We do delete the item
                await table.DeleteEntityAsync(entity.Code);
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entities">Data</param>
        public static void DeleteEntityString<T>(this MongoTable<T> table, IEnumerable<T> entities) where T : MongoTableEntity<string>
        {
            // Only if data exists
            if (entities.HasValue())
            {
                // We do delete the data
                table.DeleteEntity(entities.Select(e => e.Code));
            }
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <typeparam name="T">Table key type</typeparam>
        /// <param name="table">Mongo typebased table</param>
        /// <param name="entities">Data</param>
        /// <returns>Nothing</returns>
        public static async Task DeleteEntityStringAsync<T>(this MongoTable<T> table, IEnumerable<T> entities) where T : MongoTableEntity<string>
        {
            // Only if data exists
            if (entities.HasValue())
            {
                // We do delete the data
                await table.DeleteEntityAsync(entities.Select(e => e.Code));
            }
        }

    }

}
