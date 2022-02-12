using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Minitwit_BE.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PwHash { get; set; }

        public Boolean compareHash(string passedPwd) {
            byte[] hashBytes = Convert.FromBase64String(this.PwHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(passedPwd, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i=0; i < 20; i++)
            {
                if (hashBytes[i+16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class UserInput
    {
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string applyHash() {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(this.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }
    }
}