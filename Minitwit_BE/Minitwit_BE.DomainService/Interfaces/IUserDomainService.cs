using Minitwit_BE.Domain;

namespace Minitwit_BE.DomainService.Interfaces
{
    public interface IUserDomainService
    {
        Task<User> GetUserById(int id);
        Task<User> GetUserByName(string username);
        Task RegisterUser(User user);
        Task<int> Login(User input);
    }
}
