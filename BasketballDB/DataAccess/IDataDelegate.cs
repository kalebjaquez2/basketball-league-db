namespace DataAccess
{
   public interface IDataDelegate
   {
      string ProcedureName { get; }

      void PrepareCommand(Command command);
   }
}