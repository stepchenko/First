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
        User LogInUser(string login);
        void SetActiveForUser(string login);
        void SetDeActiveForUser(string login);
        bool isFreeLogin(string login);
        bool isVerifyPassword(string login, string password);
        string[] GetRolesForUser(string login);
        bool isUserInRole(string login, string roleName);
    }
}
