﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G01.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.G01.DAL.Data.Configurations
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Emplyee>
    {
        public void Configure(EntityTypeBuilder<Emplyee> builder)
        {
            builder.Property(E => E.Salary).HasColumnType("decimal(18,2)");
        }
    }
}
