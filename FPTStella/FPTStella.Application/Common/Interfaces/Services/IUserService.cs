using FPTStella.Contracts.DTOs.Users;


namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> GetUserByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task UpdateUserAsync(string id, CreateUserDto updateUserDto);
        Task DeleteUserAsync(string id);
        Task<UserDto> FindOrCreateGoogleUserAsync(string email, string fullName);
    }
}
