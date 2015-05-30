using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueueStepchenko.Models
{
    public class RedirectClientViewModel
    {
        public Queue Queue { get; set; }
        public SelectList Operations {get; set;}
    }
}