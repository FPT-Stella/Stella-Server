using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using BCrypt.Net;
using FPTStella.Contracts.DTOs.Users;
using FPTStella.Domain.Enums;
using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Jwt;

namespace FPTStella.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public AccountService(IUnitOfWork unitOfWork, IJwtService jwtService )
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
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

        public async Task<UserWithTokenDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var accountRepository = _unitOfWork.Repository<Account>();

            // Kiểm tra username đã tồn tại
            var existingUser = await accountRepository.FindOneAsync(u => u.Username == createUserDto.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }
            var existingEmail = await accountRepository.FindOneAsync(u => u.Email == createUserDto.Email);
            if (existingEmail != null)
            {
                throw new Exception("Email already exists.");
            }

            var user = new Account
            {
                Username = createUserDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                Role = Role.Student,
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
            };

            await accountRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();

            // Pass the 'user' object to GenerateJwtToken as required by its signature
            var token = _jwtService.GenerateJwtToken(user);

            return new UserWithTokenDto
            {
                User = MapToUserDto(user),
                Token = token
            };
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

            // Convert the string 'id' to Guid for comparison
            var userIdGuid = Guid.Parse(id);
            var existingUser = await accountRepository.FindOneAsync(u => u.Username == updateUserDto.Username && u.Id != userIdGuid);

            user.Username = !string.IsNullOrWhiteSpace(updateUserDto.Username) ? updateUserDto.Username : user.Username;
            user.PasswordHash = !string.IsNullOrWhiteSpace(updateUserDto.Password) ? BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password) : user.PasswordHash;
            user.FullName = !string.IsNullOrWhiteSpace(updateUserDto.FullName) ? updateUserDto.FullName : user.FullName;
            user.Email = !string.IsNullOrWhiteSpace(updateUserDto.Email) ? updateUserDto.Email : user.Email;


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
