using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using SqlEditor.Annotations;
using SqlEditor.DatabaseExplorer;


namespace SqlEditor
{
    public static class Extensions
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>) s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }
            return await task;
        }

        public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }
            await task.ConfigureAwait(false);
        }

        public static async Task<SqlQueryResult> ExecuteQueryAsync([NotNull] this IDbConnection dbConnection,
                                                                   [NotNull] string sql, int maxResults, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            if (sql == null) throw new ArgumentNullException("sql");

            var dataResults = await Task.Run(() =>
                {
                    SqlQueryResult queryResults;
                    using (dbConnection)
                    {
                        dbConnection.OpenIfRequired();
                        ReportProgress(progress, 0, "Running query ...");
                        using (var dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.CommandText = sql;
                            using (var dr = dbCommand.ExecuteReader())
                            {
                                queryResults = FetchDataTable(dr, maxResults, cancellationToken, progress);
                            }
                        }
                    }
                    return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteQueryAsync([NotNull] this IDbCommand command,
                                                                   [NotNull] string sql, int maxResults, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (sql == null) throw new ArgumentNullException("sql");

            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                using (command)
                {
                    ReportProgress(progress, 0, "Running query ...");
                    using (command)
                    {
                        command.CommandText = sql;
                        using (var dr = command.ExecuteReader())
                        {
                            queryResults = FetchDataTable(dr, maxResults, cancellationToken, progress);
                        }
                    }
                }
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteQueryKeepAliveAsync([NotNull] this IDbConnection dbConnection,
                                                                   [NotNull] string sql, int maxResults, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            if (sql == null) throw new ArgumentNullException("sql");

            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                dbConnection.OpenIfRequired();
                ReportProgress(progress, 0, "Running query ...");
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = sql;
                    using (var dr = dbCommand.ExecuteReader())
                    {
                        queryResults = FetchDataTable(dr, maxResults, cancellationToken, progress);
                    }
                }
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteQueryKeepAliveAsync([NotNull] this IDbCommand dbCommand,
                                                                   [NotNull] string sql, int maxResults, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbCommand == null) throw new ArgumentNullException("dbCommand");
            if (sql == null) throw new ArgumentNullException("sql");

            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                ReportProgress(progress, 0, "Running query ...");
                using (dbCommand)
                {
                    dbCommand.CommandText = sql;
                    using (var dr = dbCommand.ExecuteReader())
                    {
                        queryResults = FetchDataTable(dr, maxResults, cancellationToken, progress);
                    }
                }
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteQueryKeepAliveAsync([NotNull] this IDbConnection dbConnection, IDbTransaction transaction,
                                                                   [NotNull] string sql, int maxResults, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            if (sql == null) throw new ArgumentNullException("sql");

            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                dbConnection.OpenIfRequired();
                ReportProgress(progress, 0, "Running query ...");
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.Transaction = transaction;
                    dbCommand.CommandText = sql;
                    using (var dr = dbCommand.ExecuteReader())
                    {
                        queryResults = FetchDataTable(dr, maxResults, cancellationToken, progress);
                    }
                }
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteNonQueryAsync([NotNull] this IDbConnection dbConnection, string sql, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                using (dbConnection)
                {
                    dbConnection.OpenIfRequired();
                    ReportProgress(progress, 0, "Running query ...");
                    using (var dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = sql;
                        queryResults = new SqlQueryResult(dbCommand.ExecuteNonQuery());
                    }
                }
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteNonQueryAsync([NotNull] this IDbCommand dbCommand, string sql, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbCommand == null) throw new ArgumentNullException("dbCommand");
            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                ReportProgress(progress, 0, "Running query ...");
                using (dbCommand)
                {
                    dbCommand.CommandText = sql;
                    queryResults = new SqlQueryResult(dbCommand.ExecuteNonQuery());
                }
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteNonQueryKeepAliveAsync([NotNull] this IDbConnection dbConnection, string sql, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                dbConnection.OpenIfRequired();
                ReportProgress(progress, 0, "Running query ...");
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = sql;
                    queryResults = new SqlQueryResult(dbCommand.ExecuteNonQuery());
                }
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteNonQueryTransactionAsync([NotNull] this IDbConnection dbConnection, string sql, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                dbConnection.OpenIfRequired();
                ReportProgress(progress, 0, "Creating transaction ...");
                var transaction = dbConnection.BeginTransaction();
                ReportProgress(progress, 0, "Running query ...");
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.Transaction = transaction;
                    dbCommand.CommandText = sql;
                    queryResults = new SqlQueryResult(dbCommand.ExecuteNonQuery());
                    queryResults.Transaction = transaction;
                }
                ReportProgress(progress, 100, "Done.");
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteNonQueryTransactionAsync([NotNull] this IDbCommand dbCommand, string sql, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbCommand == null) throw new ArgumentNullException("dbCommand");
            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                ReportProgress(progress, 0, "Creating transaction ...");
                var transaction = dbCommand.Connection.BeginTransaction();
                ReportProgress(progress, 0, "Running query ...");
                using (dbCommand)
                {
                    dbCommand.Transaction = transaction;
                    dbCommand.CommandText = sql;
                    queryResults = new SqlQueryResult(dbCommand.ExecuteNonQuery());
                    queryResults.Transaction = transaction;
                }
                ReportProgress(progress, 100, "Done.");
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteNonQueryTransactionAsync([NotNull] this IDbConnection dbConnection, IDbTransaction transaction, string sql, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                dbConnection.OpenIfRequired();
                ReportProgress(progress, 0, "Running query ...");
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.Transaction = transaction;
                    dbCommand.CommandText = sql;
                    queryResults = new SqlQueryResult(dbCommand.ExecuteNonQuery());
                    queryResults.Transaction = transaction;
                }
                ReportProgress(progress, 100, "Done.");
                return queryResults;
            });
            return dataResults;
        }

        public static async Task<SqlQueryResult> ExecuteNonQueryTransactionAsync([NotNull] this IDbCommand dbCommand, IDbTransaction transaction, string sql, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInformation> progress = null)
        {
            if (dbCommand == null) throw new ArgumentNullException("dbCommand");
            var dataResults = await Task.Run(() =>
            {
                SqlQueryResult queryResults;
                ReportProgress(progress, 0, "Running query ...");
                using (dbCommand)
                {
                    dbCommand.Transaction = transaction;
                    dbCommand.CommandText = sql;
                    queryResults = new SqlQueryResult(dbCommand.ExecuteNonQuery());
                    queryResults.Transaction = transaction;
                }
                ReportProgress(progress, 100, "Done.");
                return queryResults;
            });
            return dataResults;
        }

        private static void ReportProgress(IProgress<ProgressInformation> progress, int progressPercentage, string action = "")
        {
            if (progress == null) return;
            if (progressPercentage < 0 || progressPercentage > 100)  throw new ArgumentException("Progress percentage " + progressPercentage + " is not between 0 and 100", "progressPercentage");
            progress.Report(new ProgressInformation(progressPercentage, action));
        }

        public static SqlQueryResult FetchDataTable([JetBrains.Annotations.NotNull] this IDataReader dataReader, int maxRows, CancellationToken cancellationToken = default(CancellationToken),
                                                     IProgress<ProgressInformation> progress = null)
        {
            if (dataReader == null) throw new ArgumentNullException("dataReader");
            const string action = "Fetching data ...";
            var hasMoreRows = true;
            var table = dataReader.CreateEmptyTable();
            var rowValues = new object[dataReader.FieldCount];
            var rowNumber = 0;
            ReportProgress(progress, 0, action);
            while (rowNumber < maxRows)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (dataReader.Read())
                {
                    var newRow = table.NewRow();
                    dataReader.GetValues(rowValues);
                    newRow.ItemArray = rowValues;
                    table.Rows.Add(newRow);
                    ++rowNumber;
                    ReportProgress(progress, rowNumber*100/maxRows, action);
                }
                else
                {
                    hasMoreRows = false;
                    break;
                }
            }
            ReportProgress(progress, 100, action);
            return new SqlQueryResult(table, hasMoreRows);
        }

        public static DataTable CreateEmptyTable([NotNull] this IDataRecord dataReader)
        {
            if (dataReader == null) throw new ArgumentNullException("dataReader");
            var resultsTable = new DataTable();
            for (var i = 0; i < dataReader.FieldCount; ++i)
            {
                var columnName = dataReader.GetName(i);
                var distinctColumnName = columnName;
                var index = 1;
                while (resultsTable.Columns.Contains(distinctColumnName))
                {
                    distinctColumnName = columnName + "_" + index;
                    ++index;
                }
                resultsTable.Columns.Add(distinctColumnName, dataReader.GetFieldType(i));
            }
            return resultsTable;
        }

        public static int InRange(this int x, int lo, int hi)
        {
            Debug.Assert(lo <= hi);
            return x < lo ? lo : (x > hi ? hi : x);
        }
        public static bool IsInRange(this int x, int lo, int hi)
        {
            return x >= lo && x <= hi;
        }
        public static Color HalfMix(this Color one, Color two)
        {
            return Color.FromArgb(
                (one.A + two.A) >> 1,
                (one.R + two.R) >> 1,
                (one.G + two.G) >> 1,
                (one.B + two.B) >> 1);
        }
    }
}
