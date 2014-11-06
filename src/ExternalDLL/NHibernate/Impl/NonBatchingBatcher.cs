using System.Data;
using NHibernate.AdoNet;
using NHibernate.Engine;

namespace NHibernate.Impl
{
	/// <summary>
	/// An implementation of the <see cref="IBatcher" /> 
	/// interface that does no batching.
	/// </summary>
	internal class NonBatchingBatcher : BatcherImpl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NonBatchingBatcher"/> class.
		/// </summary>
		/// <param name="connectionManager">The <see cref="ConnectionManager"/> for this batcher.</param>
		public NonBatchingBatcher(ConnectionManager connectionManager)
			: base(connectionManager)
		{
		}

		/// <summary>
		/// Executes the current <see cref="IDbCommand"/> and compares the row Count
		/// to the <c>expectedRowCount</c>.
		/// </summary>
		/// <param name="expectation">
		/// The expected number of rows affected by the query.  A value of less than <c>0</c>
		/// indicates that the number of rows to expect is unknown or should not be a factor.
		/// </param>
		/// <exception cref="HibernateException">
		/// Thrown when there is an expected number of rows to be affected and the
		/// actual number of rows is different.
		/// </exception>
		public override void AddToBatch(IExpectation expectation)
		{
			IDbCommand cmd = CurrentCommand;
			int rowCount = ExecuteNonQuery(cmd);
			expectation.VerifyOutcomeNonBatched(rowCount, cmd);
		}

		/// <summary>
		/// This Batcher implementation does not support batching so this is a no-op call.  The
		/// actual execution of the <see cref="IDbCommand"/> is run in the <c>AddToBatch</c> 
		/// method.
		/// </summary>
		/// <param name="ps"></param>
		protected override void DoExecuteBatch(IDbCommand ps)
		{
		}
	}
}