using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class EmployeeRepository: IRepositoryEmployee
    {
        public List<Employee> Elements {get; set;}

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
                reader.Close();
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

                while (reader.Read())
                {

                    list.Add(new Operation()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = Convert.ToString(reader["Name"]),
                        CountEmployees = Convert.ToInt32(reader["CountEmployees"])

                    });

                };
                reader.Close();
            };

            return list;   

        }
  

        public Employee Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}