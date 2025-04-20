using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using FPTStella.Domain.Enum;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(IMongoDatabase database)
             : base(database, nameof(Account)) 
        {
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await FindOneAsync(u => u.Username == username);
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await FindOneAsync(u => u.Email == email);
        }

        public async Task<Account> FindOrCreateGoogleUserAsync(string email, string fullName)
        {
            var user = await GetByEmailAsync(email);
            if (user != null)
            {
                return user;
            }

            user = new Account
            {
                Username = email.Split('@')[0],
                Email = email,
                FullName = fullName,
                Role = Role.Student,
            };

            await InsertAsync(user);
            return user;
        }
    }
}


