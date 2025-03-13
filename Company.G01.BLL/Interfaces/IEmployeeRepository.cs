using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G01.DAL.Models;

namespace Company.G01.BLL.Interfaces
{
   public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        //IEnumerable<Emplyee> GetAll();

        //Emplyee? Get(int id);

        //int Add(Emplyee model);
        //int Update(Emplyee model);
        //int Delete(Emplyee model);

    }
}
