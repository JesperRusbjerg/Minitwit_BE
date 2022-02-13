using Minitwit_BE.Domain;

namespace Minitwit_BE.DomainService.Interfaces
{
    public interface IUserDomainService
    {
        Task RegisterUser(User user);
        Task Login(User input);
    }
}
