using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using BCrypt.Net;
using FPTStella.Contracts.DTOs.Users;
using FPTStella.Domain.Enum;
using FPTStella.Application.Common.Interfaces.Repositories;

namespace FPTStella.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private static UserDto MapToUserDto(Account user)
        {
            return new UserDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Role = user.Role.ToString(),
                FullName = user.FullName,
                Email = user.Email,
            };
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var accountRepository = _unitOfWork.Repository<Account>();

            // Kiểm tra username đã tồn tại
            var existingUser = await accountRepository.FindOneAsync(u => u.Username == createUserDto.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var user = new Account
            {
                Username = createUserDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                Role = Enum.Parse<Role>(createUserDto.Role),
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
            };

            await accountRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();

            return MapToUserDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var accountRepository = _unitOfWork.Repository<Account>();
            var users = await accountRepository.GetAllAsync();

            return users.Select(MapToUserDto);
        }

        public async Task<UserDto> FindOrCreateGoogleUserAsync(string email, string fullName)
        {
            var accountRepository = _unitOfWork.Repository<Account>();
            var user = await accountRepository.FindOneAsync(u => u.Email == email);

            if (user == null)
            {
                user = new Account
                {
                    Username = email.Split('@')[0],
                    Email = email,
                    FullName = fullName,
                    Role = Role.Student,
                };

                await accountRepository.InsertAsync(user);
                await _unitOfWork.SaveAsync();
            }

            return MapToUserDto(user);
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var accountRepository = _unitOfWork.Repository<Account>();
            var user = await accountRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return MapToUserDto(user);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var accountRepository = _unitOfWork.Repository<Account>();
            var user = await accountRepository.FindOneAsync(u => u.Username == username);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return MapToUserDto(user);
        }

        public async Task UpdateUserAsync(string id, CreateUserDto updateUserDto)
        {
            var accountRepository = _unitOfWork.Repository<Account>();
            var user = await accountRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Username = updateUserDto.Username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            user.Role = Enum.Parse<Role>(updateUserDto.Role);
            user.FullName = updateUserDto.FullName;
            user.Email = updateUserDto.Email;

            await accountRepository.ReplaceAsync(id, user);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteUserAsync(string id)
        {
            var accountRepository = _unitOfWork.Repository<Account>();
            await accountRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
