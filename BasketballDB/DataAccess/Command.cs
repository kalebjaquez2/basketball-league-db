using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccess
{
   public class Command
   {
      private readonly SqlCommand sqlCommand;

      internal Command(SqlCommand sqlCommand)
      {
         this.sqlCommand = sqlCommand;
      }

      public SqlParameterCollection Parameters => sqlCommand.Parameters;

      public CommandType CommandType
      {
         get { return sqlCommand.CommandType; }
         set { sqlCommand.CommandType = value; }
      }

      public T GetParameterValue<T>(string name) => (T)sqlCommand.Parameters[name].Value;

      public T GetParameterValue<T>(string name, T ifNull)
      {
         var value = sqlCommand.Parameters[name].Value;

         return value == DBNull.Value ? ifNull : (T)value;
      }
   }
}
