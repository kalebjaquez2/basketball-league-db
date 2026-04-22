namespace DataAccess
{
   [Serializable]
   public class RecordNotFoundException(string key)
      : Exception($"The requested record with key [{key}] does not exist.")
   {
   }
}
