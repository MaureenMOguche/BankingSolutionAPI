using BS.Domain;
using BS.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Identity
{
    public class BSIdentityDbContext : IdentityDbContext
    {

        public BSIdentityDbContext(DbContextOptions<BSIdentityDbContext> options)
            :base(options)
        {
        }

        public DbSet<BankUser> BankUsers { get; set; }

    }
}
