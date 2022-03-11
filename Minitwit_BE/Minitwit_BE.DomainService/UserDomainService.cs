using Minitwit_BE.Domain;
using Minitwit_BE.Domain.Exceptions;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Minitwit_BE.DomainService
{
    public class UserDomainService : IUserDomainService
    {
        private readonly IPersistenceService _persistenceService;

        public UserDomainService(IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = (await _persistenceService.GetUsers(u => u.UserId.Equals(id))).FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }
            else
            {
                return user;
            }
        }

        public async Task<User> GetUserByName(string username)
        {
            var user = (await _persistenceService.GetUsers(u => u.UserName.Equals(username))).FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }
            else
            {
                return user;
            }
        }
        
        public async Task RegisterUser(User user)
        {
            var existingUser = (await _persistenceService.GetUsers(
                u => u.UserName.Equals(user.UserName) || u.Email.Equals(user.Email))).FirstOrDefault();

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException("The username is already taken");
            }
            else
            {
                user.PwHash = GetHash(user.PwHash);
                await _persistenceService.AddUser(user);
            }
        }

        public async Task<int> Login(User user)
        {
            var loggedUser = (await _persistenceService.GetUsers(u => u.Email.Equals(user.Email))).FirstOrDefault();
            if (loggedUser != null && CompareHash(user.PwHash, loggedUser.PwHash))
            {
                return loggedUser.UserId;
            }
            else
            {
                throw new UnauthorizedAccessException("Email or password does not match!");
            } 
            
        }

        public string GetHash(string password)
        {
            string fixedSalt = "7bV6DZ3t8Mwe?n?#";
            byte[] unhashedBytes = Encoding.Unicode.GetBytes(password + fixedSalt);
            SHA256 sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(unhashedBytes);
            return Convert.ToBase64String(hashedBytes);
        }

        public bool CompareHash(string attemptedPassword, string storedPwd)
        {
            string base64AttemptedHash = GetHash(attemptedPassword);
            return storedPwd.Equals(base64AttemptedHash);
        }
    }
}
