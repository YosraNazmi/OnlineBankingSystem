using System;
using ABC_Bank.Models;
using Microsoft.EntityFrameworkCore;


namespace ABC_Bank.Data
{
    public class ABCbankContext : DbContext
    {
        public ABCbankContext()
        {
        }

        public ABCbankContext(DbContextOptions<ABCbankContext> options) : base(options)
        {
        }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

      
    }


}
