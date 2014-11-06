

namespace NHibernate.Impl
{
    internal class OracleClientBatchingBatcher : CommandSetBatchingBatcher
    {
        internal OracleClientBatchingBatcher(ConnectionManager connectionManager)
            : base(connectionManager)
        {
        }

        protected override IDbCommandSet CreateCommandSet()
        {
            return new OracleClientCommandSet();
        }
    }
}
