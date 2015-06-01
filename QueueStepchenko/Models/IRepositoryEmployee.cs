using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueStepchenko.Models
{
    public interface IRepositoryEmployee
    {

        void Save(Employee element);
        void Delete(Employee element);
        List<Employee> GetList();
        Employee Get(int id);


        List<Operation> GetOperationsForChoice(string login);
        void SaveEmployeeOperations(string login, string[] checkedValues);

        List<Operation> GetOperationsById(int id);
    }
}