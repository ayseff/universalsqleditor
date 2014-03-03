using System;
using System.Data;
using System.Threading.Tasks;
using SqlEditor.Annotations;

namespace SqlEditor.DatabaseExplorer
{
    public static class Extensions
    {
        public static void OpenIfRequired([NotNull] this IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public static Task OpenIfRequiredAsync([NotNull] this IDbConnection connection)
        {
            return Task.Run(() => connection.OpenIfRequired());
        }
    }
}
