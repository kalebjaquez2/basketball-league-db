namespace DataAccess
{
   public class ColumnNotFoundException(string name, Exception? innerException = null)
      : Exception($"The column [{name}] was not found in the result set.", innerException)
   {
   }
}