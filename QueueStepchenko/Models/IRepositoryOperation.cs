using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStepchenko.Models
{
    public interface IRepositoryOperation
    {

        void Save(Operation element);
        void Delete(Operation element);
        List<Operation> GetList();
        Operation Get(int id);
    }
}
