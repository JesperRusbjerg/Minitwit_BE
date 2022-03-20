using Minitwit_BE.Domain;

namespace Minitwit_BE.Api.Dtos
{
    public class UserDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PwHash { get; set; }

        public static explicit operator UserDto(Task<User> v)
        {
            throw new NotImplementedException();
        }
    }
}
