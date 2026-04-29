using Backend.Models;

namespace Backend.Repositories
{
    public interface IUserRepository
    {
        User? FetchUserByCredentials(string username, string passwordHash);
        User CreateUser(string username, string passwordHash, bool isAdmin);
        IReadOnlyList<User> RetrieveUsers();
        void UpdateUserAdminStatus(int userID, bool isAdmin);
        void UpdateUserCredentials(int userID, string username, string? passwordHash);
    }
}
