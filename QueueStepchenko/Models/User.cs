using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
    }
}