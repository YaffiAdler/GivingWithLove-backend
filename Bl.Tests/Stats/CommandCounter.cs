using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bl.Tests.Stats
{
    // Counts the SQL commands sent to the database, to check that the number of queries per call is constant.
    public sealed class CommandCounter : DbCommandInterceptor
    {
        public int Count { get; private set; }

        public void Reset()
        {
            Count = 0;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            Count++;
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}
