using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStepchenko.Models
{
    public interface IRepositorySetting
    {
        Setting Get();
        void Save(int NextNumberQueue, int NumberCall, int TimeCall);
    }
}
