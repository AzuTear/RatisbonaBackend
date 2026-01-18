using System.ComponentModel.DataAnnotations;

namespace RatisbonaBackend.Presentation.Contracts.Users;

public class UserCreateRequest
{
    [Required]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Lastname { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(8)]
    [StringLength(200)]
    public string PasswordHash { get; set; } = null!;
}
