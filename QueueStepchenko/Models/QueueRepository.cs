using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class QueueRepository:IRepositoryQueue
    {
        public Queue GetIn(string clientLogin, int operationId, StatesClient stateClient)
        {
            if (string.IsNullOrEmpty(clientLogin))
            {
                throw new ArgumentException("Client's login is null or empty");
            };

            if (operationId == 0)
            {
                throw new ArgumentException("OperationId is empty");
            };

            if ((int) stateClient == 0)
            {
                throw new ArgumentException("State client is not undefined");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetInQueue", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = clientLogin;
            command.Parameters.Add("@operationId", System.Data.SqlDbType.Int).Value = operationId;
            command.Parameters.Add("@stateClientId", System.Data.SqlDbType.Int).Value = (int)stateClient;

            connection.Open();

            Queue queue = new Queue();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    queue.Id = Convert.ToInt32(reader["Id"]);
                    queue.StateClient = (StatesClient)Convert.ToInt32(reader["StateClientId"]);
                    queue.Number = Convert.ToInt32(reader["Number"]);
                    queue.NumberCall = Convert.ToInt32(reader["NumberCall"]);
                    queue.PrevId = Convert.ToInt32(reader["prevId"]);
                    queue.Client = new Client()
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Name = Convert.ToString(reader["ClientName"])
                    };
                    queue.Operation = new Operation()
                    {
                        Id = Convert.ToInt32(reader["OperationId"]),
                        Name = Convert.ToString(reader["OperationName"]),
                        CountClients = Convert.ToInt32(reader["CountClients"]),
                        CountEmployees = Convert.ToInt32(reader["CountEmployees"])
                    };
                }
            };

            return queue;
        }

        public Operation GetOut(int queueId, StatesClient stateClient)
        {
            if (queueId == 0)
            {
                throw new ArgumentException("OperationQueueId is empty");
            };

            if ((int)stateClient == 0)
            {
                throw new ArgumentException("State client is not undefined");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetOutQueue", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@queueId", System.Data.SqlDbType.Int).Value = queueId;
            command.Parameters.Add("@stateClientId", System.Data.SqlDbType.Int).Value = (int)stateClient;
         
            connection.Open();

            Operation operation = new Operation();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    operation.Id = Convert.ToInt32(reader["Id"]);
                    operation.Name = Convert.ToString(reader["Name"]);
                    operation.CountClients = Convert.ToInt32(reader["countClients"]);
                    operation.CountEmployees = Convert.ToInt32(reader["countEmployees"]);
                }
            };

            return operation;
        }

        public List<Queue> GetListInQueue()
        {
            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetListInQueue", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            List<Queue> listQueue = new List<Queue>();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listQueue.Add(new Queue(){
                                                Id = Convert.ToInt32(reader["Id"]),
                                                StateClient = (StatesClient)Convert.ToInt32(reader["StateClientId"]),
                                                Number = Convert.ToInt32(reader["Number"]),
                                                Client = new Client()
                                                {
                                                    ClientId = Convert.ToInt32(reader["ClientId"]),
                                                    Name = Convert.ToString(reader["ClientName"])
                                                },
                                                Operation = new Operation()
                                                {
                                                    Id = Convert.ToInt32(reader["OperationId"]),
                                                    Name = Convert.ToString(reader["OperationName"])
                                                }
                    });
                };

                reader.Close();
            };

            return listQueue;
        }

        public Queue GetQueue(string clientLogin)
        {
            if (string.IsNullOrEmpty(clientLogin))
            {
                throw new ArgumentException("Client's login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetQueue", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = clientLogin;
            
            connection.Open();

            Queue queue = new Queue();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    queue.Id = Convert.ToInt32(reader["Id"]);
                    queue.StateClient = (StatesClient)Convert.ToInt32(reader["StateClientId"]);
                    queue.Number = Convert.ToInt32(reader["Number"]);
                    queue.NumberCall = Convert.ToInt32(reader["NumberCall"]);
                    queue.Client = new Client()
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Name = Convert.ToString(reader["ClientName"])
                    };
                    queue.Operation = new Operation()
                    {
                        Id = Convert.ToInt32(reader["OperationId"]),
                        Name = Convert.ToString(reader["OperationName"]),
                        CountClients = Convert.ToInt32(reader["CountClients"]),
                        CountEmployees = Convert.ToInt32(reader["CountEmployees"])
                    };
                }
            };

            return queue;

        }
    }
}