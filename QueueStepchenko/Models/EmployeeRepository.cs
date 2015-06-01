using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace QueueStepchenko.Models
{
    public class EmployeeRepository: IRepositoryEmployee
    {
 
        public void Save(Employee employee)
        {
            throw new NotImplementedException();
        }

        public void Delete(Employee employee)
        {
            throw new NotImplementedException();
        }

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


        public Employee Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}