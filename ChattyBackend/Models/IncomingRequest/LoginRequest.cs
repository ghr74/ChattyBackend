using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ChattyBackend.Models.IncomingRequest;

public sealed record LoginRequest([Required] string Email, [Required] string Password);
