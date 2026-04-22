namespace DataAccess
{
   public interface INonQueryDataDelegate<out T> : IDataDelegate
   {
      T Translate(Command command);
   }
}