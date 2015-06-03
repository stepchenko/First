using DevOne.Security.Cryptography.BCrypt;
using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class UserRepository: IRepositoryUser
    {

        public User GetUserByLogin(string login)
        {

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetUserByLogin", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;
            
            connection.Open();

            User user = new User();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    if (reader.Read())
                    {
                        user.Login = login;
                        user.Id = Convert.ToInt32(reader["Id"]);
                        user.Name = Convert.ToString(reader["Name"]);
                        user.RoleName = Convert.ToString(reader["RoleName"]);

                    };

                };
                
            }

            return user;
        }

        public int LogOffUser(string login)
        {

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("LogOffUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;

            connection.Open();

            int userId;

            using (connection)
            {
                var result = command.ExecuteScalar();
                if (result == null)
                {
                    userId = 0;
                }
                else
                {
                    userId = (int) result;
                }

            };

            return userId;
        }

        public bool isVerifyPassword(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetPasswordByLogin", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;
           
            connection.Open();

            string hash = string.Empty;

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        hash = Convert.ToString(reader["Password"]);
                    }
                };
            };

            if (hash == string.Empty || !BCryptHelper.CheckPassword(password, hash))
            {
                return false;
            }
            else
            {
                return true;
            };
        }


        public User LogInUser(string login)
        {

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("LogInUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;
           
            connection.Open();

            User user = new User();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        user.Login = login;
                        user.Id = Convert.ToInt32(reader["Id"]);
                        user.Name = Convert.ToString(reader["Name"]);
                        user.RoleName = Convert.ToString(reader["RoleName"]);
                    }
                };

            };

            return user;
        }


        public void SetActiveForUser(string login)
        {

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SetActiveForUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;
           
            connection.Open();

            using (connection)
            {

                command.ExecuteNonQuery();

            }

        }


        public void SetDeActiveForUser(string login)
        {

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SetDeActiveForUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;

            connection.Open();

            using (connection)
            {

                command.ExecuteNonQuery();

            }

        }

        public bool isFreeLogin(string login)
        {

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("IsFreeLogin", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;

            bool result;
            
            connection.Open();

            using (connection)
            {

                result = (bool)command.ExecuteScalar();

            }

            return result;
        }

        public string[] GetRolesForUser(string login)
        {

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetRolesForUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;

            connection.Open();

            string[] roles = new string[1];

            using (connection)
            {

                roles[0] = (string)command.ExecuteScalar();
            }

            return roles;
        }

        public bool isUserInRole(string login, string roleName)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("RoleName is null or empty");
            };

            bool result;

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("isUserInRole", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;
            command.Parameters.Add("@roleName", System.Data.SqlDbType.VarChar).Value = roleName;

            connection.Open();

            using (connection)
            {

                result = (bool)command.ExecuteScalar();

            }

            return result;
        }


        public ClientViewModel Get(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetClient", connection);
            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            ClientViewModel client = new ClientViewModel();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    if (reader.Read())
                    {
                        client.ClientId = Convert.ToInt32(reader["ClientId"]);
                        client.Name = Convert.ToString(reader["Name"]);
                        client.Email = Convert.ToString(reader["Email"]);
                        client.Address = Convert.ToString(reader["Address"]);
                        client.Phone = Convert.ToString(reader["Phone"]);
                   }
                }
            };

            return client;

        }


        public void SaveWithPassword(ClientViewModel client)
        {
            if (string.IsNullOrEmpty(client.Login))
            {
                throw new ArgumentException("Login is empty");
            };

            if (string.IsNullOrEmpty(client.Name))
            {
                throw new ArgumentException("Name is empty");
            };

            if (string.IsNullOrEmpty(client.Password))
            {
                throw new ArgumentException("Password is empty");
            };

            if (string.IsNullOrEmpty(client.Email))
            {
               client.Email = string.Empty;
            };

            if (string.IsNullOrEmpty(client.Address))
            {
                client.Address = string.Empty;
            };

            if (string.IsNullOrEmpty(client.Phone))
            {
                client.Phone = string.Empty;
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SaveClientWithPassword", connection);
            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = client.ClientId;
            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = client.Login;
            command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = client.Name;
            command.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = client.Email;
            command.Parameters.Add("@address", System.Data.SqlDbType.VarChar).Value = client.Address;
            command.Parameters.Add("@phone", System.Data.SqlDbType.VarChar).Value = client.Phone;

            string salt = BCryptHelper.GenerateSalt();
            string hash = BCryptHelper.HashPassword(client.Password, salt);

            command.Parameters.Add("@salt", System.Data.SqlDbType.VarChar).Value = salt;
            command.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = hash;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            using (connection)
            {
                command.ExecuteNonQuery();
            };
            
        }


        public void Save(int ClientId, string Name, string Email, string Address, string Phone)
        {
            if (ClientId == 0)
            {
                throw new ArgumentException("ID is empty");
            };

            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("Name is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SaveClient", connection);
            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = ClientId;
            command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = Name;
            command.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = Email;
            command.Parameters.Add("@address", System.Data.SqlDbType.VarChar).Value = Address;
            command.Parameters.Add("@phone", System.Data.SqlDbType.VarChar).Value = Phone;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            using (connection)
            {
                command.ExecuteNonQuery();
            };
        }
    }
}