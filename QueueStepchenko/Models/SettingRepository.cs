using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class SettingRepository: IRepositorySetting
    {

        public Setting Get()
        {
            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("GetSetting", connection);
           
            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            Setting setting = new Setting();

            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    if (reader.Read())
                    {
                        setting.NextNumberQueue = Convert.ToInt32(reader["NextNumberQueue"]);
                        setting.NumberCall = Convert.ToInt32(reader["NumberCall"]);
                        setting.TimeCall = Convert.ToInt32(reader["TimeCall"]);
                    }
                }
            };

            return setting;
        }

        public void Save(int NextNumberQueue, int NumberCall, int TimeCall)
        {
            if (NextNumberQueue < 0)
            {
                throw new ArgumentException("NextNumberQueue is empty");
            };

            if (NumberCall <= 0)
            {
                throw new ArgumentException("NumberCall is empty");
            };

            if (TimeCall <= 0)
            {
                throw new ArgumentException("TimeCall is empty");
            };

            string conString = Methods.GetStringConnection();

            SqlConnection connection = new SqlConnection(conString);

            SqlCommand command = new SqlCommand("SaveSetting", connection);
            command.Parameters.Add("@NextNumberQueue", System.Data.SqlDbType.Int).Value = NextNumberQueue;
            command.Parameters.Add("@NumberCall", System.Data.SqlDbType.Int).Value = NumberCall;
            command.Parameters.Add("@TimeCall", System.Data.SqlDbType.Int).Value = TimeCall;

            command.CommandType = System.Data.CommandType.StoredProcedure;

            connection.Open();

            using (connection)
            {
                command.ExecuteNonQuery();
            };

        }
    }
}