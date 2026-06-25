using System.Collections.Generic;
using System.Threading.Tasks;
using ControleTeste.Models;
using ControleTeste.Repositories;

namespace ControleTeste.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<Usuario> CreateAsync(string username, string password, bool isAdmin)
    {
        var existing = await _repo.GetByUsernameAsync(username);
        if (existing != null) throw new InvalidOperationException("Usuário já existe.");

        var usuario = new Usuario
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsAdmin = isAdmin
        };

        return await _repo.AddAsync(usuario);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repo.DeleteAsync(id);
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        return await _repo.GetByUsernameAsync(username);
    }

    public async Task<Usuario?> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
        return user;
    }

    public async Task<bool> UpdateAsync(int id, string username, string? password, bool isAdmin)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return false;

        user.Username = username;
        if (!string.IsNullOrWhiteSpace(password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
        user.IsAdmin = isAdmin;

        return await _repo.UpdateAsync(user);
    }
}
