using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStepchenko.Models
{
    public interface IQueueHub
    {
        void CallClient();
        void Connect();
        bool isLoginUser(string login);

        string GetConnectionIdByLogin(string login);
        void GetOutQueue(string login, int queueId, int CountClients, int operationId);
    }
}
