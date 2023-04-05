using System;
using System.Security.Cryptography;
using System.Text;

namespace Trivia.Auth.Security;

public static class HashingUtils
{
    public static string HashString(string payload, string salt)
    {
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

        // Concatenate both byte arrays
        byte[] saltedPayloadBytes = new byte[payloadBytes.Length + saltBytes.Length];
        Array.Copy(payloadBytes, 0, saltedPayloadBytes, 0, payloadBytes.Length);
        Array.Copy(saltBytes, 0, saltedPayloadBytes, payloadBytes.Length, saltBytes.Length);

        byte[] hashedBytes;
        using (var sha256 = SHA256.Create())
            hashedBytes = sha256.ComputeHash(saltedPayloadBytes);

        return Convert.ToHexString(hashedBytes);
    }
}