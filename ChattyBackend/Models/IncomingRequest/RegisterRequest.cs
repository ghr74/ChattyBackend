using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ChattyBackend.Models.IncomingRequest;

public sealed record RegisterRequest(
    [Required] [EmailAddress] string Email,
    [Required] [StringLength(64, MinimumLength = 8)] string Password,
    [Required] [StringLength(16)] string Username,
    [Required] string Secret
);
