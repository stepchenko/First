using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueStepchenko.Models
{
    public interface IRepositoryUser
    {
    
        User GetUserByLogin(string login);
        int LogOffUser(string login);
        int CheckUser(string login, string password);
        bool isFreeLogin(string login);
        string[] GetRolesForUser(string login);
        bool isUserInRole(string login, string roleName);
    }
}
