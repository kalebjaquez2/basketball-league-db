namespace Backend.Models
{
    public class User
    {
        public int UserID { get; }
        public string Username { get; }
        public bool IsAdmin { get; set; }

        public User(int userID, string username, bool isAdmin)
        {
            UserID = userID;
            Username = username;
            IsAdmin = isAdmin;
        }
    }
}
