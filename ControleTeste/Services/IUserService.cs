using ControleTeste.Models;

namespace ControleTeste.Services;

public interface IUserService
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario?> GetByUsernameAsync(string username);
    Task<Usuario> CreateAsync(string username, string password, bool isAdmin);
    Task<bool> UpdateAsync(int id, string username, string? password, bool isAdmin);
    Task<bool> DeleteAsync(int id);
    Task<Usuario?> ValidateCredentialsAsync(string username, string password);
}
