using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{
    public static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        return BitConverter.ToString(saltBytes)
            .Replace("-", "")
            .ToLower();
    }

    public static string HashPassword(string password, string salt)
    {
        using (var sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(
                Encoding.UTF8.GetBytes(salt + password)
            );

            return BitConverter.ToString(bytes)
                .Replace("-", "")
                .ToLower();
        }
    }
}