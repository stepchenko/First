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

                if (reader.Read())
                {
                    user.Login = login;
                    user.Id = Convert.ToInt32(reader["Id"]);
                    user.Name =  Convert.ToString(reader["Name"]);
                    user.RoleName =  Convert.ToString(reader["RoleName"]);
                        
                };

                reader.Close();
                
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

        public int LogInUser(string login, string password)
        {

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Passowrd is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("LogInUser", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;
            command.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = password;

            connection.Open();

            int userId;

            using (connection)
            {
                
                userId = (int) command.ExecuteScalar();

            };

            return userId;
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
    }
}