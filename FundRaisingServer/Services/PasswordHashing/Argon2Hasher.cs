namespace FundRaisingServer.Services.PasswordHashing;
using Konscious.Security.Cryptography;

public class Argon2Hasher: IArgon2Hasher
{
    public string HashPassword(byte[] password, byte[] salt)
    {
        try
        {
            // getting the hash
            var hash = Hash(password, salt);
            return Convert.ToBase64String(hash);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
    }

    public bool VerifyHash(byte[] hashedPassword, byte[] salt, byte[] inputPassword)
    {
        
        throw new NotImplementedException();
    }

    private static byte[] Hash(byte[] password, byte[] salt)
    {
        // creating argon2 instance
        var argon2 = new Argon2d(password: password);

        // setting up the props
        argon2.DegreeOfParallelism = 4;
        argon2.MemorySize = 8194;
        argon2.Iterations = 100;
        argon2.Salt = salt;

        // returning the hash
        return argon2.GetBytes(512 / 8);
    }
}