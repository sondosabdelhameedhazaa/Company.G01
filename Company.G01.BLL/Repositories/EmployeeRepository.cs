﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G01.BLL.Interfaces;
using Company.G01.DAL.Data.Contexts;
using Company.G01.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.G01.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context) // Ask CLR To Create Object From CompanyDbContext
        {
            _context = context;
        }

        public async Task <List<Employee>> GetByNameAsync(string name)
        {
           return await _context.Employees.Include(E => E.Department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }

}