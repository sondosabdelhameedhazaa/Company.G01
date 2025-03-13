﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G01.DAL.Models;

namespace Company.G01.BLL.Interfaces
{
    public  interface IGenericRepository <T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();

       T? Get(int id);

        int Add(T model);
        int Update(T model);
        int Delete(T model);

    }
}
