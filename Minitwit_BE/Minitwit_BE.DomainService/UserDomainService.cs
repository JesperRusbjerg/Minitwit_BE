using Minitwit_BE.Domain;
using Minitwit_BE.Domain.Exceptions;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;
using System.Security.Cryptography;
using System.Text;
using Prometheus;

namespace Minitwit_BE.DomainService
{
    public class UserDomainService : IUserDomainService
    {
        // Prometheus 
        private readonly Counter _totalUsersCounter;
        private readonly Gauge _newUserInProgress;


        private readonly IPersistenceService _persistenceService;

        public UserDomainService(IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
            _totalUsersCounter = Metrics.CreateCounter("TotalUsers", "The amount of total users");
            _newUserInProgress = Metrics.CreateGauge("NewUserInProgress", "Show the hour if which new user register");
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
            using (_newUserInProgress.TrackInProgress())
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
                    _totalUsersCounter.Inc();
                }

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
