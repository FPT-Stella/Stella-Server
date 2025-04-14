using FPTStella.Application.Common.DTOs.Users;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using BCrypt.Net;

namespace FPTStella.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var repository = _unitOfWork.Repository<User>();

            // Kiểm tra username đã tồn tại
            var existingUser = await repository.FindOneAsync(u => u.Username == createUserDto.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                Role = createUserDto.Role,
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
            };

            await repository.InsertAsync(user);
            await _unitOfWork.SaveAsync();

            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role,
                FullName = user.FullName,
                Email = user.Email,
            };
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var repository = _unitOfWork.Repository<User>();
            var user = await repository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role,
                FullName = user.FullName,
                Email = user.Email,
            };
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var repository = _unitOfWork.Repository<User>();
            var user = await repository.FindOneAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role,
                FullName = user.FullName,
                Email = user.Email,
            };
        }

        public async Task UpdateUserAsync(string id, CreateUserDto updateUserDto)
        {
            var repository = _unitOfWork.Repository<User>();
            var user = await repository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Username = updateUserDto.Username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            user.Role = updateUserDto.Role;
            user.FullName = updateUserDto.FullName;
            user.Email = updateUserDto.Email;

            await repository.ReplaceAsync(id, user);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteUserAsync(string id)
        {
            var repository = _unitOfWork.Repository<User>();
            await repository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
