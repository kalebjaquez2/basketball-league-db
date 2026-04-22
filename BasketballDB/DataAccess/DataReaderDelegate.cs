namespace DataAccess
{
   public abstract class DataReaderDelegate<T>(string procedureName)
      : DataDelegate(procedureName), IDataReaderDelegate<T>
   {
      public abstract T Translate(Command command, IDataRowReader reader);
   }
}