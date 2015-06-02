using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public interface IRepositoryEmployee
    {

        void Save(int id, string name, string position);
        void SaveWithPassword(int id, string login, string name, string position, string password);
        List<EmployeeViewModel> Delete(int id);
        List<Employee> GetList();
        EmployeeViewModel Get(string login);


        List<Operation> GetOperationsForChoice(string login);
        void SaveEmployeeOperations(string login, string[] checkedValues);

        List<Operation> GetOperationsById(int id);

        List<EmployeeViewModel> GetListForView();
    }
}