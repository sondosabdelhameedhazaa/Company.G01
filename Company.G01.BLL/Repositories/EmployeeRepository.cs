using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G01.BLL.Interfaces;
using Company.G01.DAL.Data.Contexts;
using Company.G01.DAL.Models;

namespace Company.G01.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CompanyDbContext context) : base(context) // Ask CLR To Create Object From CompanyDbContext
        {

        }

    }

}