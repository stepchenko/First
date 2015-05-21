using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public bool isActive { get; set; }

        public List<Operation> Operations { get; set; }

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
                        Name = Convert.ToString(reader["Name"])

                    });

                };
                reader.Close();
            };

            return list;   

        }
    }
}