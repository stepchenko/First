using DevOne.Security.Cryptography.BCrypt;
using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace QueueStepchenko.Models
{
    public class EmployeeRepository: IRepositoryEmployee
    {
 
               
        public List<Employee> GetList()
        {
            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SelectEmployees", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            List<Employee> list = new List<Employee>();

            using (connection)
            {

                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {

                    while (reader.Read())
                    {

                        list.Add(new Employee()
                        {
                            EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                            Name = Convert.ToString(reader["Name"]),
                            Login = Convert.ToString(reader["Login"]),
                            isActive = Convert.ToBoolean(reader["isActive"])
                        });

                    };
                };
            };

            return list;   
        }


         public List<Operation> GetOperationsById(int id)
        {
            if (id <=0 )
            {
                throw new ArgumentException("Invalid argument Id");
            }

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SelectOperationsById", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

            connection.Open();

            List<Operation> list = new List<Operation>();

            using (connection)
            {

                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {

                        list.Add(new Operation()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            CountEmployees = Convert.ToInt32(reader["CountEmployees"])

                        });

                    };
                };
            };

            return list;   

        }

         public List<Operation> GetOperationsForChoice(string employeeLogin)
         {
             if (string.IsNullOrEmpty(employeeLogin))
             {
                 throw new ArgumentException("Employee's login is null or empty");
             };

             string conString = Methods.GetStringConnection();

             SqlConnection connection = new SqlConnection(conString);

             SqlCommand command = new SqlCommand("GetOperationsForChoice", connection);
             command.CommandType = System.Data.CommandType.StoredProcedure;

             command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = employeeLogin;

             connection.Open();

             List<Operation> list = new List<Operation>();

             using (connection)
             {

                 SqlDataReader reader = command.ExecuteReader();

                 using (reader)
                 {
                     while (reader.Read())
                     {

                         list.Add(new Operation()
                         {
                             Id = Convert.ToInt32(reader["Id"]),
                             Name = Convert.ToString(reader["Name"]),
                             isCheck = Convert.ToBoolean(reader["isChoice"])
                         });

                     };
                 };
             };

             return list;   
         }

         public void SaveEmployeeOperations(string employeeLogin, string[] checkedValues)
         {
             
             if (string.IsNullOrEmpty(employeeLogin))
             {
                 throw new ArgumentException("Employee's login is null or empty");
             };

             string conString = Methods.GetStringConnection();

             SqlConnection connection = new SqlConnection(conString);

             SqlCommand command = new SqlCommand("SaveEmployeeOperations", connection);
             command.CommandType = System.Data.CommandType.StoredProcedure;

             command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = employeeLogin;

             StringBuilder sb = new StringBuilder();
             sb.Append('_');
             foreach (string check in checkedValues)
             {
                 sb.Append(check);
                 sb.Append('_');
             } 
             command.Parameters.Add("@strId", System.Data.SqlDbType.VarChar).Value = sb.ToString();


             connection.Open();

             using (connection)
             {

                 command.ExecuteNonQuery();
             }

         }


         public List<EmployeeViewModel> GetListForView()
         {
             string conString = Methods.GetStringConnection();

             SqlConnection connection = new SqlConnection(conString);

             SqlCommand command = new SqlCommand("SelectEmployeesForView", connection);
             command.CommandType = System.Data.CommandType.StoredProcedure;

             connection.Open();

             List<EmployeeViewModel> list = new List<EmployeeViewModel>();

             using (connection)
             {

                 SqlDataReader reader = command.ExecuteReader();

                 using (reader)
                 {

                     while (reader.Read())
                     {

                         list.Add(new EmployeeViewModel()
                         {
                             EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                             Name = Convert.ToString(reader["Name"]),
                             Login = Convert.ToString(reader["Login"]),
                             Position = Convert.ToString(reader["Position"]),
                             ErrorMessage = string.Empty
                         });

                     };
                 };
             };

             return list;
         }

         public EmployeeViewModel Get(string login)
         {
             if (string.IsNullOrEmpty(login))
             {
                 throw new ArgumentException("Login is empty");
             };

             string conString = Methods.GetStringConnection();

             SqlConnection connection = new SqlConnection(conString);

             SqlCommand command = new SqlCommand("GetEmployee", connection);
             command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;

             command.CommandType = System.Data.CommandType.StoredProcedure;

             connection.Open();

             EmployeeViewModel employee = new EmployeeViewModel();

             using (connection)
             {
                 SqlDataReader reader = command.ExecuteReader();

                 using (reader)
                 {
                     if (reader.Read())
                     {
                         employee.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                         employee.Name = Convert.ToString(reader["Name"]);
                         employee.Position = Convert.ToString(reader["Position"]);
                         employee.ErrorMessage = String.Empty;
                     }
                 }
             };

             return employee;
         }


         public void Save(int id, string name, string position)
         {
             if (id == 0)
             {
                 throw new ArgumentException("ID is empty");
             };

             if (string.IsNullOrEmpty(name))
             {
                 throw new ArgumentException("Name is empty");
             };

             if (string.IsNullOrEmpty(position))
             {
                 throw new ArgumentException("Position is empty");
             };

             string conString = Methods.GetStringConnection();

             SqlConnection connection = new SqlConnection(conString);

             SqlCommand command = new SqlCommand("SaveEmployee", connection);
             command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
             command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = name;
             command.Parameters.Add("@position", System.Data.SqlDbType.VarChar).Value = position;

             command.CommandType = System.Data.CommandType.StoredProcedure;

             connection.Open();

             using (connection)
             {
                 command.ExecuteNonQuery();
             };

         }

         public void SaveWithPassword(int id, string login, string name, string position, string password)
         {
             if (string.IsNullOrEmpty(login))
             {
                 throw new ArgumentException("Login is empty");
             };

             if (string.IsNullOrEmpty(name))
             {
                 throw new ArgumentException("Name is empty");
             };

             if (string.IsNullOrEmpty(position))
             {
                 throw new ArgumentException("Position is empty");
             };

             if (string.IsNullOrEmpty(password))
             {
                 throw new ArgumentException("Password is empty");
             };

             string conString = Methods.GetStringConnection();

             SqlConnection connection = new SqlConnection(conString);

             SqlCommand command = new SqlCommand("SaveEmployeeWithPassword", connection);
             command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
             command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;
             command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = name;
             command.Parameters.Add("@position", System.Data.SqlDbType.VarChar).Value = position;

             string salt = BCryptHelper.GenerateSalt();
             string hash = BCryptHelper.HashPassword(password, salt);

             command.Parameters.Add("@salt", System.Data.SqlDbType.VarChar).Value = salt;
             command.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = hash;

             command.CommandType = System.Data.CommandType.StoredProcedure;

             connection.Open();

             using (connection)
             {
                 command.ExecuteNonQuery();
             };
         }
        public List<EmployeeViewModel> Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("ID is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("DeleteEmployee", connection);
            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            List<EmployeeViewModel> list = new List<EmployeeViewModel>();

            using (connection)
            {

                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {

                        list.Add(new EmployeeViewModel()
                        {
                            EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                            Name = Convert.ToString(reader["Name"]),
                            Login = Convert.ToString(reader["Login"]),
                            Position = Convert.ToString(reader["Position"]),
                            ErrorMessage = Convert.ToString(reader["ErrorMessage"])
                        });

                    }
                };

            };

            return list;
        }

        
    }
}