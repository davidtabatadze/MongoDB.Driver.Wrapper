using CoreKit.Sync;
using System.Threading.Tasks;

namespace MongoDB.Driver.Wrapper.Transaction
{

    /// <summary>
    /// Represents transaction functionality for <see cref="MongoContext"/>
    /// </summary>
    public static class MongoContextTransaction
    {

        #region Sync

        /// <summary>
        /// Begins session transaction
        /// </summary>
        /// <param name="context">Mongo context</param>
        public static void BeginTransaction(this MongoContext context)
        {
            // Begin...
            SyncKit.Run(() => context.BeginTransactionAsync());
        }

        /// <summary>
        /// Commits session transaction
        /// </summary>
        /// <param name="context">Mongo context</param>
        public static void CommitTransaction(this MongoContext context)
        {
            // Commit...
            SyncKit.Run(() => context.CommitTransactionAsync());
        }

        /// <summary>
        /// Rollbacks session transaction
        /// </summary>
        /// <param name="context">Mongo context</param>
        public static void RollbackTransaction(this MongoContext context)
        {
            // Rollback...
            SyncKit.Run(() => context.RollbackTransactionAsync());
        }

        #endregion

        #region Sync

        /// <summary>
        /// Begins session transaction
        /// </summary>
        /// <param name="context">Mongo context</param>
        /// <returns>Empty</returns>
        public static async Task BeginTransactionAsync(this MongoContext context)
        {
            // Only if transactions are permitted
            if (context.Configuration.UseTransactions)
            {
                // Only if session is empty
                if (context.Session == null)
                {
                    // We do create it and then begin the transaction
                    context.Session = await context.Client.StartSessionAsync();
                    context.Session.StartTransaction();
                }
                // Every time, when transaction is requested, we increase counter
                context.TransactionCounter++;
            }
        }

        /// <summary>
        /// Commits session transaction
        /// </summary>
        /// <param name="context">Mongo context</param>
        /// <returns>Empty</returns>
        public static async Task CommitTransactionAsync(this MongoContext context)
        {
            // Only if transactions are permitted
            if (context.Configuration.UseTransactions)
            {
                // Every time, when transaction commit is requested, we decrease counter
                context.TransactionCounter--;
                // Only if we dont have any sequenced transaction
                if (context.TransactionCounter == 0)
                {
                    // We do commit existing session
                    await context.Session.CommitTransactionAsync();
                    context.Session.Dispose();
                    context.Session = null;
                }
            }
        }

        /// <summary>
        /// Rollbacks session transaction asynchronously
        /// </summary>
        /// <param name="context">Mongo context</param>
        /// <returns>Empty</returns>
        public static async Task RollbackTransactionAsync(this MongoContext context)
        {
            // Only if transactions are permitted
            if (context.Configuration.UseTransactions)
            {
                // Only if session exists
                if (context.Session != null)
                {
                    // We do abort it
                    await context.Session.AbortTransactionAsync();
                    context.Session.Dispose();
                    context.Session = null;
                }
            }
        }

        #endregion

    }

}
