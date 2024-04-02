namespace FundRaisingServer.Services.PasswordHashing;

public interface IArgon2Hasher
{
    // this method is for registration i.e it produces the hash
    public byte[] HashPassword(byte[] password, byte[] salt);
    // this method is for login i.e it compares the hashes
    public bool VerifyHash(byte[] hashedPassword, byte[] salt, byte[] inputPassword);
}