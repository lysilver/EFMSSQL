using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace ORM
{
    public class EfIntercepterLogging : DbCommandInterceptor
    {
        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            var sql = command.CommandText;//这里可以获得想要的sql语句
            base.ScalarExecuting(command, interceptionContext);
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            var sql = command.CommandText;
            base.ScalarExecuted(command, interceptionContext);
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            var sql = command.CommandText;
            base.NonQueryExecuting(command, interceptionContext);
        }

        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            var sql = command.CommandText;
            base.NonQueryExecuted(command, interceptionContext);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            var sql = command.CommandText;
            var param = command.Parameters;
            base.ReaderExecuting(command, interceptionContext);
        }

        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            var sql = command.CommandText;
            var param = command.Parameters;
            base.ReaderExecuted(command, interceptionContext);
        }
    }
}