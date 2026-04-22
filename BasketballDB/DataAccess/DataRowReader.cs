using Microsoft.Data.SqlClient;

namespace DataAccess
{
   /// <summary>
   /// Simple wrapper class for more concise code by the callers.
   /// Other "getters" supported by <see cref="SqlDataReader"/> can easily be added.
   /// </summary>
   internal class DataRowReader(SqlDataReader reader) : IDataRowReader
   {
      public bool Read()
      {
         return reader.Read();
      }

      public int GetInt32(string name)
      {
         return GetValue(name, reader.GetInt32);
      }

      public byte GetByte(string name)
      {
         return GetValue(name, reader.GetByte);
      }

      public bool GetBoolean(string name)
      {
         return GetValue(name, reader.GetBoolean);
      }

      public string GetString(string name)
      {
         return GetValue(name, reader.GetString);
      }

      public DateTime GetDateTime(string name, DateTimeKind kind = DateTimeKind.Unspecified)
      {
         DateTime dateTime = GetValue(name, reader.GetDateTime);
         
         return DateTime.SpecifyKind(dateTime, kind);
      }

      public DateTimeOffset GetDateTimeOffset(string name)
      {
         return GetValue(name, reader.GetDateTimeOffset);
      }

      public bool IsDbNull(string name)
      {
         return reader.IsDBNull(GetOrdinal(name));
      }

      public T GetValue<T>(string name)
      {
         return (T)reader.GetValue(GetOrdinal(name));
      }

      private int GetOrdinal(string name)
      {
         try
         {
            return reader.GetOrdinal(name);
         }
         catch (IndexOutOfRangeException ex)
         {
            throw new ColumnNotFoundException(name, ex);
         }
      }

      private T GetValue<T>(string name, Func<int, T> getter)
      {
         return getter(GetOrdinal(name));
      }

      public T GetValue<T>(string name, T ifDbNull)
      {
         var ord = GetOrdinal(name);

         return reader.IsDBNull(ord) ? ifDbNull: (T)reader.GetValue(ord);
      }
   }
}