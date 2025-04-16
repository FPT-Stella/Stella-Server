using FPTStella.Contracts.DTOs.Users;


namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> FindOrCreateGoogleUserAsync(string email, string fullName);
        Task<UserDto> GetUserByIdAsync(string id);
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task UpdateUserAsync(string id, CreateUserDto updateUserDto);
        Task DeleteUserAsync(string id);
    }
}
