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

            var getUsersTask = await _persistenceService.GetUsers(
                user => user.UserName.Equals(user.UserName) || user.Email.Equals(user.Email));

            if (getUsersTask.SingleOrDefault() != null)
            {
                throw new UserAlreadyExistsException("User with that nickname or email already exists!");
            }
            else
            {
                await _persistenceService.AddUser(user);
            }
        }

        public async Task Login(User input)
        {
            var getUsersTask = await _persistenceService.GetUsers(user => user.Email.Equals(input.Email) && user.PwHash.Equals(input.PwHash));

            if (getUsersTask.FirstOrDefault() == null)
            {
                throw new UnauthorizedAccessException("Email or password does not match!");
            }
        }
    }
}
