namespace DataAccess
{
   public interface IDataReaderDelegate<out T> : IDataDelegate
   {
      T Translate(Command command, IDataRowReader reader);
   }
}