using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using FPTStella.Domain.Enums;
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
            // Index duy nhất cho username
            _collection.Indexes.CreateOne(new CreateIndexModel<Account>(
                Builders<Account>.IndexKeys.Ascending(a => a.Username),
                new CreateIndexOptions { Unique = true }));

            // Index duy nhất cho email
            _collection.Indexes.CreateOne(new CreateIndexModel<Account>(
                Builders<Account>.IndexKeys.Ascending(a => a.Email),
                new CreateIndexOptions { Unique = true }));
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await FindOneAsync(a => a.Username == username);
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await FindOneAsync(a => a.Email == email);
        }

        public async Task<Account> FindOrCreateGoogleUserAsync(string email, string fullName)
        {
            var account = await GetByEmailAsync(email);
            if (account != null) return account;

            account = new Account
            {
                Username = email.Split('@')[0],
                Email = email,
                FullName = fullName,
                Role = Role.Student
            };

            await InsertAsync(account);
            return account;
        }
    }
}


