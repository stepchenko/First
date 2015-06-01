using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStepchenko.Models
{
    public interface IRepositoryOperation
    {

        void Save(int id, string name);
        List<OperationViewModel> Delete(int id);
        List<Operation> GetList();
        List<OperationViewModel> GetListForView();
        OperationViewModel Get(int id);
    }
}
