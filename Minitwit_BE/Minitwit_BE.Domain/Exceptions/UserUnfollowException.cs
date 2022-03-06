using System;

namespace Minitwit_BE.Domain.Exceptions
{
    public class UserUnfollowException : Exception
    {
        public UserUnfollowException()
        {
        }

        public UserUnfollowException(string message)
            : base(message)
        {
        }

        public UserUnfollowException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
