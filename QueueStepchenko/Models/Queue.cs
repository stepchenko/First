using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public enum StatesClient: int 
    {
        Servicing=1,
        WaitExtra,
        Wait,
        Serviced,
        GetOut
    }

    public class Queue
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Operation Operation { get; set; }
        public int Number { get; set; }
        public int NumberCall { get; set; }
        public StatesClient StateClient { get; set; }
        public int PrevId { get; set; }

    }
}