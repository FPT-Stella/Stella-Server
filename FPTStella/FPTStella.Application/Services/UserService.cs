using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using BCrypt.Net;
using FPTStella.Contracts.DTOs.Users;
using FPTStella.Domain.Enum;
using FPTStella.Application.Common.Interfaces.Repositories;

namespace FPTStella.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Kiểm tra username đã tồn tại
            var existingUser = await _userRepository.GetByUsernameAsync(createUserDto.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                Role = Role.Student,
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
            };

            await _userRepository.CreateAsync(user);

            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role.ToString(),
                FullName = user.FullName,
                Email = user.Email,
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role.ToString(),
                FullName = user.FullName,
                Email = user.Email,
            });
        }

        public async Task<UserDto> FindOrCreateGoogleUserAsync(string email, string fullName)
        {
            var user = await _userRepository.FindOrCreateGoogleUserAsync(email, fullName);
            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role.ToString(),
                FullName = user.FullName,
                Email = user.Email,
            };
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role.ToString(),
                FullName = user.FullName,
                Email = user.Email,
            };
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role.ToString(),
                FullName = user.FullName,
                Email = user.Email,
            };
        }

        public async Task UpdateUserAsync(string id, CreateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Username = updateUserDto.Username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            user.Role = Role.Student;
            user.FullName = updateUserDto.FullName;
            user.Email = updateUserDto.Email;

            await _userRepository.UpdateAsync(id, user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
