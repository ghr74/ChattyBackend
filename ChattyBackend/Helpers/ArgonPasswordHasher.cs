using Argon2.Interop;
using ChattyBackend.Helpers.Interfaces;

namespace ChattyBackend.Helpers;

public sealed class ArgonPasswordHasher : IPasswordHasher
{
    private readonly Argon2Interop _argon2 = new();

    public string Hash(string input)
    {
        if (input.Length < 8)
            throw new ArgumentOutOfRangeException(
                nameof(input),
                "Input must be at least 8 characters."
            );
        return _argon2.Hash(input);
    }

    public bool Verify(string encoded, string input)
    {
        if (input.Length < 8 || encoded.Length < 8)
            return false;
        return _argon2.Verify(encoded, input);
    }
}
