﻿namespace Minitwit_BE.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PwHash { get; set; }
    }
}