using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public enum StatesClient: int 
    {
        Servicing=1,
        Welcom,
        WaitExtra,
        Wait,
        Serviced,
        GetOut,
        NoClient
    }

    public class Queue
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Employee Employee {get; set;}
        public Operation Operation { get; set; }
        public int Number { get; set; }
        public int MaxNumberCall { get; set; }
        public int TimeCall { get; set; }
        public StatesClient StateClient { get; set; }
        public int PrevId { get; set; }

    }
}