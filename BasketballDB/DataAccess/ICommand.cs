using Microsoft.Data.SqlClient;

namespace DataAccess
{
   public interface ICommand
   {
      SqlParameterCollection Parameters { get; }

      T GetParameterValue<T>(string name);
   }
}
