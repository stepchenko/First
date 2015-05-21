using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace QueueStepchenko.Providers
{
    public class CustomRoleProvider:RoleProvider
    {
        public override string[] GetRolesForUser(string login)
        {
           string[] roles = new string[1];

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

           using (connection)
           {
               roles[0] = (string)command.ExecuteScalar();
           };

           return roles;
        }

        public override bool IsUserInRole(string login, string roleName)
        {
            bool result;

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("IsUserInRole", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;
            command.Parameters.Add("@roleName", System.Data.SqlDbType.VarChar).Value = roleName;

            connection.Open();

            using (connection)
            {
                result = (bool)command.ExecuteScalar();
            };

            return result;
        }


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}