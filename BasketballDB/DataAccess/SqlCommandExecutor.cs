using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace DataAccess
{
   public class SqlCommandExecutor
   {
      private readonly string connectionString;

      public SqlCommandExecutor(string server, string database)
          : this($"Server={server};Database={database};Integrated Security=SSPI;")
      {
      }

      public SqlCommandExecutor(string connectionString)
      {
         if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("The parameter cannot be null or empty.", nameof(connectionString));

         this.connectionString = connectionString;
      }

      public void ExecuteNonQuery(IDataDelegate dataDelegate)
      {
         ExecuteCommand(dataDelegate, sqlCommand =>
         {
            sqlCommand.ExecuteNonQuery();
            return (object?)null;
         });
      }

      public T ExecuteNonQuery<T>(INonQueryDataDelegate<T> dataDelegate)
      {
         return ExecuteCommand(dataDelegate, sqlCommand =>
         {
            sqlCommand.ExecuteNonQuery();
            return dataDelegate.Translate(new Command(sqlCommand));
         });
      }

      public T ExecuteReader<T>(IDataReaderDelegate<T> dataDelegate)
      {
         return ExecuteCommand(dataDelegate, sqlCommand =>
         {
            using var reader = sqlCommand.ExecuteReader();
            return dataDelegate.Translate(new Command(sqlCommand), new DataRowReader(reader));
         });
      }

      private T ExecuteCommand<T>(IDataDelegate dataDelegate, Func<SqlCommand, T> execute)
      {
         using var transaction = new TransactionScope();
         using var connection = new SqlConnection(connectionString);
         using var sqlCommand = GenerateSqlCommand(dataDelegate.ProcedureName, connection);

         var command = new Command(sqlCommand);
         dataDelegate.PrepareCommand(command);

         connection.Open();

         T result = execute(sqlCommand);

         transaction.Complete();

         return result;
      }

      private static SqlCommand GenerateSqlCommand(string procedureName, SqlConnection connection)
      {
         return new SqlCommand(procedureName, connection)
         {
            CommandType = CommandType.StoredProcedure
         };
      }
   }
}
