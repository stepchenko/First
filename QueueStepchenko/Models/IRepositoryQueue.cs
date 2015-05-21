using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStepchenko.Models
{
    public interface IRepositoryQueue
    {
        Queue GetIn(string clientLogin, int id, StatesClient stateClient);
        Operation GetOut(int queueId, StatesClient stateClient);
        List<Queue> GetListInQueue();
        Queue GetQueue(string clientLogin);
    }
}
