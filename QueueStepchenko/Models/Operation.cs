using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountEmployees { get; set; }
        public int CountClients { get; set; }

    }
}