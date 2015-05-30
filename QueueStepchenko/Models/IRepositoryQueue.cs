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
        Queue CallClient(string login);
        bool isCurrentUserInQueue(string login);
        Queue RedirectClient(int queueId, int newOperationId);
        Queue Accept(int queueId);
        StatesClient GetStateClient(int queueId);
        Employee GetServicingEmployee(int Id);
    }
}
