using BS.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Persistence
{
    public class BSDbContext : IdentityDbContext
    {
        public BSDbContext(DbContextOptions<BSDbContext> options)
            : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccount>().HasIndex(x => x.AccountNumber)
                .IsUnique();
               
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BankUser> BankUsers { get; set; }
    }
}
