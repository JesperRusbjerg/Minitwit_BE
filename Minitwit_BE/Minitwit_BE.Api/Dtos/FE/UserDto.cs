namespace Minitwit_BE.Api.Dtos
{
    public class UserDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PwHash { get; set; }
    }
}
