using System;
using System.Collections.Generic;
using System.Data;
using DataAccess;
using Backend.Models;

namespace Backend.Repositories
{
    public class SqlUserRepository(SqlCommandExecutor executor) : IUserRepository
    {
        private readonly SqlCommandExecutor _executor = executor;

        public User? FetchUserByCredentials(string username, string passwordHash)
        {
            return _executor.ExecuteReader(
                new FetchUserByUsernameDelegate(username, passwordHash));
        }

        public User CreateUser(string username, string passwordHash, bool isAdmin)
        {
            return _executor.ExecuteNonQuery(
                new CreateUserDelegate(username, passwordHash, isAdmin));
        }

        public IReadOnlyList<User> RetrieveUsers()
        {
            return _executor.ExecuteReader(new RetrieveUsersDelegate());
        }

        public void UpdateUserAdminStatus(int userID, bool isAdmin)
        {
            _executor.ExecuteNonQuery(
                new UpdateUserAdminStatusDelegate(userID, isAdmin));
        }

        public void UpdateUserCredentials(int userID, string username, string? passwordHash)
        {
            _executor.ExecuteNonQuery(
                new UpdateUserCredentialsDelegate(userID, username, passwordHash));
        }

        // ── Delegates ──────────────────────────────────────────

        private class FetchUserByUsernameDelegate(string username, string passwordHash)
            : DataReaderDelegate<User?>("Basketball.FetchUserByUsername")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("Username", username);
            }

            public override User? Translate(Command command, IDataRowReader reader)
            {
                if (!reader.Read()) return null;
                string stored = reader.GetString("PasswordHash");
                if (!string.Equals(stored, passwordHash, StringComparison.OrdinalIgnoreCase))
                    return null;
                return new User(
                    reader.GetInt32("UserID"),
                    reader.GetString("Username"),
                    Convert.ToBoolean(reader.GetValue<object>("IsAdmin")));
            }
        }

        private class CreateUserDelegate(string username, string passwordHash, bool isAdmin)
            : NonQueryDataDelegate<User>("Basketball.CreateUser")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("Username", username);
                command.Parameters.AddWithValue("PasswordHash", passwordHash);
                command.Parameters.AddWithValue("IsAdmin", isAdmin);
                var p = command.Parameters.Add("UserID", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;
            }

            public override User Translate(Command command)
            {
                int userID = command.GetParameterValue<int>("UserID");
                return new User(userID, username, isAdmin);
            }
        }

        private class RetrieveUsersDelegate()
            : DataReaderDelegate<IReadOnlyList<User>>("Basketball.RetrieveUsers")
        {
            public override void PrepareCommand(Command command) { }

            public override IReadOnlyList<User> Translate(Command command, IDataRowReader reader)
            {
                var list = new List<User>();
                while (reader.Read())
                    list.Add(new User(
                        reader.GetInt32("UserID"),
                        reader.GetString("Username"),
                        Convert.ToBoolean(reader.GetValue<object>("IsAdmin"))));
                return list;
            }
        }

        private class UpdateUserAdminStatusDelegate(int userID, bool isAdmin)
            : DataDelegate("Basketball.UpdateUserAdminStatus")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("UserID", userID);
                command.Parameters.AddWithValue("IsAdmin", isAdmin);
            }
        }

        private class UpdateUserCredentialsDelegate(int userID, string username, string? passwordHash)
            : DataDelegate("Basketball.UpdateUserCredentials")
        {
            public override void PrepareCommand(Command command)
            {
                command.Parameters.AddWithValue("UserID", userID);
                command.Parameters.AddWithValue("Username", username);
                command.Parameters.AddWithValue("PasswordHash",
                    passwordHash as object ?? DBNull.Value);
            }
        }
    }
}
