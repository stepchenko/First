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

                using (reader)
                {
                    while (reader.Read())
                    {

                        list.Add(new Operation()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            CountEmployees = Convert.ToInt32(reader["CountEmployees"]),
                            CountClients = Convert.ToInt32(reader["CountClients"])
                        });

                    }

                }
            };

            return list;    
        }


        public List<OperationViewModel> GetListForView()
        {
            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SelectOperationsForView", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            List<OperationViewModel> list = new List<OperationViewModel>();

            using (connection)
            {

                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {

                        list.Add(new OperationViewModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            ErrorMessage = String.Empty
                        });

                    }
                };

            };

            return list;
        }



        public OperationViewModel Get(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("ID is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetOperation", connection);
            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            OperationViewModel operation = new OperationViewModel();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                
                using (reader)
                { 
                    if (reader.Read())
                    { 
                        operation.Id = Convert.ToInt32(reader["Id"]);
                        operation.Name = Convert.ToString(reader["Name"]);
                        operation.ErrorMessage = String.Empty;
                    }
                }
            };

            return operation;
        }


        public void Save(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SaveOperation", connection);
            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
            command.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = name;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            using (connection)
            {
                command.ExecuteNonQuery();
            };

        }

        public List<OperationViewModel> Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("ID is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("DeleteOperation", connection);
            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            List<OperationViewModel> list = new List<OperationViewModel>();

            using (connection)
            {

                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {

                        list.Add(new OperationViewModel()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            ErrorMessage = Convert.ToString(reader["ErrorMessage"])
                        });

                    }
                };

            };

            return list;
        }



    }

}