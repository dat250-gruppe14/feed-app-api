using System.Text;
using Konscious.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace FeedAppApi.Utils;

public class PasswordUtils : IPasswordUtils
{
    public string HashPassword(string password, string salt)
    {
        // Console.WriteLine("Hash no salt: {0}", BC.HashPassword(password));
        // return BC.HashPassword(password, salt);
        
        // https://www.twelve21.io/how-to-use-argon2-for-password-hashing-in-csharp/
        
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

        argon2.Salt = Encoding.UTF8.GetBytes(salt);
        argon2.DegreeOfParallelism = 4;
        argon2.Iterations = 4;
        argon2.MemorySize = 1024 * 100;

        return Convert.ToBase64String(argon2.GetBytes(16));
    }

    public bool VerifyPassword(string password, string salt, string passwordHash)
    {
        // return BC.Verify(password, passwordHash);
        
        var newHash = HashPassword(password, salt);
        return passwordHash.SequenceEqual(newHash);
    }

    public string GenerateSalt()
    {
        return BC.GenerateSalt();
    }
}