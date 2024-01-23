using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPITest.Models;
using Microsoft.EntityFrameworkCore;

namespace BPITest.DB
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<User> AllUsers { get; set; }


    }
}
