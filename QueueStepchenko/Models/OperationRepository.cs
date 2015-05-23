using System;
using System.Collections.Generic;
using System.Linq;
using QueueStepchenko.Utils;
using System.Data;
using System.Data.SqlClient;

namespace QueueStepchenko.Models
{
    public class OperationRepository : IRepositoryOperation
    {
        public List<Operation> Elements { get; set; }

        public void Save(Operation operation)
        {
            throw new NotImplementedException();
        }

        public void Delete(Operation operation)
        {
            throw new NotImplementedException();
        }

        public List<Operation> GetList()
        {
            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SelectOperations", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

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
                        CountEmployees = Convert.ToInt32(reader["CountEmployees"]),
                        CountClients = Convert.ToInt32(reader["CountClients"])
                    });

                };
                reader.Close();
            };

            return list;    
        }

        public bool isCurrentClientInQueue(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("isCurrentClientInQueue", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;

            connection.Open();

            bool result;

            using (connection)
            {
                result = (bool) command.ExecuteScalar();
                
            };

            return result;
        }

        public Operation Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}