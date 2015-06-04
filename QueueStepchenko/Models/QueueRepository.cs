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

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar) { Value = clientLogin });
            parameters.Add(new SqlParameter("@operationId", System.Data.SqlDbType.Int) {Value = operationId});
            parameters.Add(new SqlParameter("@stateClientId", System.Data.SqlDbType.Int) {Value = (int)stateClient});

            return ParseQueue("GetInQueue",parameters);
        }

        public void GetOut(int queueId, StatesClient stateClient)
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

            using (connection)
            {
                command.ExecuteNonQuery();
                
            };

        }


        public Operation GetCountClientsInQueue(int queueId)
        {
            if (queueId == 0)
            {
                throw new ArgumentException("OperationQueueId is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetCountClientsInQueue", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@queueId", System.Data.SqlDbType.Int).Value = queueId;
            
            connection.Open();

            Operation operation = new Operation();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        operation.Id = Convert.ToInt32(reader["Id"]);
                        operation.CountClients = Convert.ToInt32(reader["countClients"]);
                    }
                }
            };

            return operation;
        }


        public bool isCurrentUserInQueue(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetCurrentQueueIdByLogin", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login;

            connection.Open();

            bool result;
            int queueId;

            using (connection)
            {
                queueId = (int)command.ExecuteScalar();
                result = (queueId > 0);

            };

            return result;
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

                using (reader)
                {

                    while (reader.Read())
                    {
                        listQueue.Add(new Queue()
                        {
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

                };
            };

            return listQueue;
        }


        public Queue GetQueue(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Login is null or empty");
            };

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar) { Value = login });

            return ParseQueue("GetQueueByLogin",parameters);

        }

        public StatesClient GetStateClient(int queueId)
        {
            if (queueId == 0)
            {
                throw new ArgumentException("Queue.ID is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetStateClient", connection);
            command.Parameters.Add("@queueId", System.Data.SqlDbType.Int).Value = queueId;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            StatesClient result = 0;

            using (connection)
            {
                
                 SqlDataReader  reader = command.ExecuteReader();

                 using (reader)
                 {
                     if (reader.Read())
                     {                        
                        result = (StatesClient)Convert.ToInt32(reader["StateClientId"]);
                     }
                 }

            };
            return result;
        }

        public Employee GetServicingEmployee(int queueId)
        {
            if (queueId == 0)
            {
                throw new ArgumentException("Queue.ID is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetServicingEmployee", connection);
            command.Parameters.Add("@queueId", System.Data.SqlDbType.Int).Value = queueId;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            Employee employee = new Employee();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        employee.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                        employee.Name = Convert.ToString(reader["Name"]);
                        employee.Login = Convert.ToString(reader["Login"]);
                    }
                }
            };

            return employee;
        }

        public Queue CallClient(string employeeLogin) 
        {
            if (string.IsNullOrEmpty(employeeLogin))
            {
                throw new ArgumentException("Employee's login is null or empty");
            };

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@employeeLogin", System.Data.SqlDbType.VarChar) { Value = employeeLogin });
   
            return ParseQueue("CallClient",parameters);
        }


        public Queue RedirectClient(int queueId, int newOperationId)
        {
            if (queueId == 0)
            {
                throw new ArgumentException("Queue.ID is empty");
            };

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@queueId", System.Data.SqlDbType.Int) { Value = queueId });
            parameters.Add(new SqlParameter("@newOperationId", System.Data.SqlDbType.Int) { Value = newOperationId });
            parameters.Add(new SqlParameter("@stateClientId", System.Data.SqlDbType.Int) { Value = (int) StatesClient.GetOut});

            return ParseQueue("RedirectClient", parameters);
        }


        public Queue Accept(int queueId)
        {
            if (queueId == 0)
            {
                throw new ArgumentException("Queue.ID is empty");
            };

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@queueId", System.Data.SqlDbType.Int) { Value = queueId });
           
            return ParseQueue("Accept", parameters);
        }   

        
        private Queue ParseQueue(string procedureName, List<SqlParameter> parameters)
        {
            string conString = Methods.GetStringConnection();
            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand(procedureName, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            connection.Open();

            Queue queue = new Queue();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        queue.Id = Convert.ToInt32(reader["Id"]);
                        queue.StateClient = (StatesClient)Convert.ToInt32(reader["StateClientId"]);
                        queue.Number = Convert.ToInt32(reader["Number"]);
                        queue.MaxNumberCall = Convert.ToInt32(reader["MaxNumberCall"]);
                        queue.TimeCall = Convert.ToInt32(reader["TimeCall"]);
                        queue.PrevId = Convert.ToInt32(reader["prevId"]);
                        if (Convert.ToInt32(reader["ClientId"]) > 0)
                        {
                            queue.Client = new Client()
                            {
                                ClientId = Convert.ToInt32(reader["ClientId"]),
                                Name = Convert.ToString(reader["ClientName"]),
                                Login = Convert.ToString(reader["ClientLogin"])
                            };
                        };

                        if (Convert.ToInt32(reader["EmployeeId"]) > 0)
                        {
                            queue.Employee = new Employee()
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                Name = Convert.ToString(reader["EmployeeName"]),
                                Login = Convert.ToString(reader["EmployeeLogin"])
                            };
                        };

                        if (Convert.ToInt32(reader["OperationId"]) > 0)
                        {
                            queue.Operation = new Operation()
                            {
                                Id = Convert.ToInt32(reader["OperationId"]),
                                Name = Convert.ToString(reader["OperationName"]),
                                CountClients = Convert.ToInt32(reader["CountClients"]),
                                CountEmployees = Convert.ToInt32(reader["CountEmployees"])
                            };
                        }
                    }
                }
            };

            return queue;
        }

    }
}