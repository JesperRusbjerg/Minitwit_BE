﻿using Minitwit_BE.Domain;
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
            var existingUser = (await _persistenceService.GetUsers(
                u => u.UserName.Equals(user.UserName) || u.Email.Equals(user.Email))).FirstOrDefault();

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException("User with that nickname or email already exists!");
            }
            else
            {
                await _persistenceService.AddUser(user);
            }
        }

        public async Task<int> Login(User input)
        {
            var loggedUser = (await _persistenceService.GetUsers(
                user => user.Email.Equals(input.Email) && user.PwHash.Equals(input.PwHash))).FirstOrDefault();

            if (loggedUser == null)
            {
                throw new UnauthorizedAccessException("Email or password does not match!");
            } 
            
            return getUsersTask.FirstOrDefault().UserId;
        }
    }
}
