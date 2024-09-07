using System.ComponentModel.DataAnnotations;

namespace ChattyBackend.Models.IncomingRequest;

public sealed record RegisterRequest(
    [Required] [EmailAddress] string Email,
    [Required] [StringLength(64, MinimumLength = 8)] string Password,
    [Required] [StringLength(16, MinimumLength = 1)] string Username,
    [Required] string Secret
);
