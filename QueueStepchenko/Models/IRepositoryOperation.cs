using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStepchenko.Models
{
    public interface IRepositoryOperation
    {
        List<Operation> Elements { get; set; }

        void Save(Operation element);
        void Delete(Operation element);
        List<Operation> GetList(string login);
        bool isCurrentClientInQueue(string login);
        Operation Get(int id);
    }
}
