using Minitwit_BE.Domain;
using Minitwit_BE.Domain.Exceptions;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.DomainService
{
    public class UserDomainService : IUserDomainService
    {
        private readonly IPersistenceService _persistenceService;

        public UserDomainService(IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
        }
        
        public async Task RegisterUser(User user)
        {

            var existingUser = (await _persistenceService.GetUsers(e => e.UserName.Equals(user.UserName))).FirstOrDefault();

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException("The username is already taken");
            }
            else
            {
                await _persistenceService.AddUser(user);
            }
        }

        public async Task Login(User input)
        {
            var loggedUser = (await _persistenceService.GetUsers(
                user => user.Email.Equals(input.Email) && user.PwHash.Equals(input.PwHash))).FirstOrDefault();

            if (loggedUser == null)
            {
                throw new UnauthorizedAccessException("Email or password does not match!");
            }
        }
    }
}
