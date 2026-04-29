namespace Frontend
{
    internal static class Session
    {
        internal const string ConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BasketballLeague560;" +
            @"Integrated Security=True;Persist Security Info=False;Pooling=False;" +
            @"MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;" +
            @"Application Name=""SQL Server Management Studio"";Command Timeout=0";

        public static int UserID { get; set; }
        public static string Username { get; set; } = "";
        public static bool IsAdmin { get; set; }
    }
}
