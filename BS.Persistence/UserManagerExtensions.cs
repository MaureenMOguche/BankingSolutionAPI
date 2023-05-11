using BS.Application.Contracts.Persistence;
using BS.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BS.Persistence
{
    public static class UserManagerExtensions
    {
        public static async Task<BankUser> FindByCustomParametersAsync(this UserManager<BankUser> userManager,
            Expression<Func<BankUser, bool>> filter)
        {
            return await userManager?.Users?.Where(filter).FirstOrDefaultAsync();
        }
    }
}
