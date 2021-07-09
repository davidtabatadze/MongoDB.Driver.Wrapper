using CoreKit.Sync;
using System.Threading.Tasks;

namespace MongoDB.Driver.Wrapper
{

    /// <summary>
    /// Represents session transaction functionality for <see cref="MongoContext"/>
    /// </summary
    public partial class MongoContext
    {

        /// <summary>
        /// Starts session transaction
        /// </summary>
        /// <returns>Empty</returns>
        public new void SessionStart()
        {
            // Start
            SyncKit.Run(() => base.SessionStart());
        }

        /// <summary>
        /// Commits session transaction
        /// </summary>
        /// <returns>Empty</returns>
        public new void SessionCommit()
        {
            // Commit
            SyncKit.Run(() => base.SessionCommit());
        }

        /// <summary>
        /// Aborts session transaction
        /// </summary>
        /// <returns>Empty</returns>
        public new void SessionAbort()
        {
            // Abort
            SyncKit.Run(() => base.SessionAbort());
        }

    }

    /// <summary>
    /// Represents session transaction functionality for <see cref="MongoContextAsync"/>
    /// </summary
    public partial class MongoContextAsync
    {

        /// <summary>
        /// Starts session transaction
        /// </summary>
        /// <returns>Empty</returns>
        public new async Task SessionStart()
        {
            // Start
            await base.SessionStart();
        }

        /// <summary>
        /// Commits session transaction
        /// </summary>
        /// <returns>Empty</returns>
        public new async Task SessionCommit()
        {
            // Commit
            await base.SessionCommit();
        }

        /// <summary>
        /// Aborts session transaction
        /// </summary>
        /// <returns>Empty</returns>
        public new async Task SessionAbort()
        {
            // Abort
            await base.SessionAbort();
        }

    }

    /// <summary>
    /// Represents session transaction functionality for <see cref="MongoContextBase"/>
    /// </summary
    public partial class MongoContextBase
    {

        /// <summary>
        /// Begins session transaction
        /// </summary>
        /// <returns>Empty</returns>
        internal protected async Task SessionStart()
        {
            // Only if transactions are permitted
            if (Configuration.UseTransactions)
            {
                // Only if session is empty
                if (Session == null)
                {
                    // We do create it and then begin the transaction
                    Session = await Client.StartSessionAsync();
                    Session.StartTransaction();
                }
                // Every time, when transaction is requested, we increase counter
                TransactionCounter++;
            }
        }

        /// <summary>
        /// Commits session transaction
        /// </summary>
        /// <returns>Empty</returns>
        internal protected async Task SessionCommit()
        {
            // Only if transactions are permitted
            if (Configuration.UseTransactions)
            {
                // Every time, when transaction commit is requested, we decrease counter
                TransactionCounter--;
                // Only if we dont have any sequenced transaction
                if (TransactionCounter == 0)
                {
                    // We do commit existing session
                    await Session.CommitTransactionAsync();
                    Session.Dispose();
                    Session = null;
                }
            }
        }

        /// <summary>
        /// Rollbacks session transaction
        /// </summary>
        /// <returns>Empty</returns>
        internal protected async Task SessionAbort()
        {
            // Only if transactions are permitted
            if (Configuration.UseTransactions)
            {
                // Only if session exists
                if (Session != null)
                {
                    // We do abort it
                    await Session.AbortTransactionAsync();
                    Session.Dispose();
                    Session = null;
                }
            }
        }

    }

}
