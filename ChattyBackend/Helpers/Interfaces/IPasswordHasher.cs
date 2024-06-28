namespace ChattyBackend.Helpers.Interfaces;

public interface IPasswordHasher
{
    string Hash(string input);

    bool Verify(string encoded, string input);
}
