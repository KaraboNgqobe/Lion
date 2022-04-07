using System;
using Microsoft.EntityFrameworkCore;
using POC_Leave.Models;

namespace POC_Leave.Data
{
    public class MyDbContext: DbContext
    {
        public MyDbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employee { get; set; }
    }
}
