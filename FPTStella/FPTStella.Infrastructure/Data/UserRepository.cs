using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using FPTStella.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepository<User> _repository;

        public UserRepository(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork.Repository<User>();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _repository.FindOneAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _repository.FindOneAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            await _repository.InsertAsync(user);
            return user;
        }

        public async Task<User> FindOrCreateGoogleUserAsync(string email, string fullName)
        {
            var user = await GetByEmailAsync(email);
            if (user != null)
            {
                return user;
            }

            user = new User
            {
                Username = email.Split('@')[0],
                Email = email,
                FullName = fullName,
                Role = "User",
            };

            await _repository.InsertAsync(user);
            return user;
        }

        public async Task UpdateAsync(string id, User user)
        {
            await _repository.ReplaceAsync(id, user);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}


