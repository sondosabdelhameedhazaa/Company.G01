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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) {
            _context = context;
        }
        public IEnumerable<Emplyee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public Emplyee? Get(int id)
        {
            return _context.Employees.Find(id);
        }

        public int Add(Emplyee model)
        {
            _context.Employees.Add(model);
            return _context.SaveChanges();
        }
        public int Update(Emplyee model)
        {
            _context.Employees.Update(model);
            return _context.SaveChanges();
        }

        public int Delete(Emplyee model)
        {
            _context.Employees.Remove(model);
            return _context.SaveChanges();
        }


    }
}
