using RatisbonaBackend.Business.Entities;
using RatisbonaBackend.Business.Interfaces.Repositories;
using RatisbonaBackend.Presentation.Contracts.Users;

namespace RatisbonaBackend.Business.Services;

public class UsersService(IUserRepository userRepository)
{
    public async Task<List<UserDto>> ListAsync(CancellationToken cancellationToken)
    {
        var users = await userRepository.ListAsync(cancellationToken);
        return users.Select(MapToDto).ToList();
    }

    public async Task<ServiceResult<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return ServiceResult<UserDto>.NotFound("User not found.");
        }

        return ServiceResult<UserDto>.Ok(MapToDto(user));
    }

    public async Task<ServiceResult<UserDto>> CreateAsync(UserCreateRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            return ServiceResult<UserDto>.Conflict("Email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Firstname = request.Firstname.Trim(),
            Lastname = request.Lastname.Trim(),
            Email = request.Email.Trim(),
            PasswordHash = request.PasswordHash,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await userRepository.AddAsync(user, cancellationToken);
        return ServiceResult<UserDto>.Created(MapToDto(user));
    }

    public async Task<ServiceResult> UpdateAsync(Guid id, UserUpdateRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return ServiceResult.NotFound("User not found.");
        }

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailOwner = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (emailOwner is not null && emailOwner.Id != id)
            {
                return ServiceResult.Conflict("Email already exists.");
            }
        }

        user.Firstname = request.Firstname.Trim();
        user.Lastname = request.Lastname.Trim();
        user.Email = request.Email.Trim();
        if (!string.IsNullOrWhiteSpace(request.PasswordHash))
        {
            user.PasswordHash = request.PasswordHash;
        }

        user.UpdatedAt = DateTimeOffset.UtcNow;

        await userRepository.UpdateAsync(user, cancellationToken);
        return ServiceResult.NoContent();
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return ServiceResult.NotFound("User not found.");
        }

        await userRepository.DeleteAsync(id, cancellationToken);
        return ServiceResult.NoContent();
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}

public record ServiceResult(int StatusCode, string? Error)
{
    public static ServiceResult NoContent() => new(StatusCodes.Status204NoContent, null);
    public static ServiceResult NotFound(string message) => new(StatusCodes.Status404NotFound, message);
    public static ServiceResult Conflict(string message) => new(StatusCodes.Status409Conflict, message);
}

public record ServiceResult<T>(int StatusCode, T? Value, string? Error)
{
    public static ServiceResult<T> Ok(T value) => new(StatusCodes.Status200OK, value, null);
    public static ServiceResult<T> Created(T value) => new(StatusCodes.Status201Created, value, null);
    public static ServiceResult<T> NotFound(string message) => new(StatusCodes.Status404NotFound, default, message);
    public static ServiceResult<T> Conflict(string message) => new(StatusCodes.Status409Conflict, default, message);
}
