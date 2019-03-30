using System;
using System.Linq;
using BC7.Business.Helpers;
using BC7.Security.PasswordUtilities;

namespace BC7.Business.Implementation.Helpers
{
    public class ReflinkHelper : IReflinkHelper
    {
        private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int Length = 16;

        public string GenerateReflink()
        {
            var random = new Random(DateTime.Now.Ticks.GetHashCode());
            
            var randomStrings = new string(Enumerable.Repeat(Chars, Length).Select(s => s[random.Next(s.Length)]).ToArray());
            var hashSalt = PasswordEncryptionUtilities.GenerateSaltedHash(randomStrings);
            var hash = hashSalt.Hash.Substring(0, Math.Min(hashSalt.Hash.Length, 32));

            return hash;
        }
    }
}
