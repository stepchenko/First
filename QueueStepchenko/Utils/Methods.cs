using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace QueueStepchenko.Utils
{
    public static class Methods
    {
        public static string GetStringConnection()
        {
            string stringConnection = WebConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
            if (string.IsNullOrEmpty(stringConnection))
            {
                throw new ArgumentException("String connection is null or empty");
            }
            return stringConnection;
        }

    }
}