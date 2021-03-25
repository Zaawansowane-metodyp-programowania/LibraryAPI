using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using AutoMapper;
using LibraryAPI.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Authorization;

namespace LibraryAPI.Services
{
    public interface IUserService
    {
        int Create(CreateUserDto dto);
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        void Delete(int id);
        void Update(int id, UpdateUserDto dto);
        void ChangePassword(int id, ChangePasswordDto dto);
    }

    public class UserService : IUserService
    {
        private readonly LibraryDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;

        public UserService(LibraryDBContext dbContext, IMapper mapper,ILogger<UserService> logger, IPasswordHasher<User> passwordHasher, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public void Update(int id, UpdateUserDto dto)
        {
            var user = _dbContext
               .Users
               .FirstOrDefault(u => u.Id == id);

            if (user is null)
                throw new NotFoundException("User not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user,
               new UserOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidException();

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;

            _dbContext.SaveChanges();
 
        }
        public void ChangePassword(int id, ChangePasswordDto dto)
        {
            var userToModify = _dbContext.Users.Include(x => x.Role).FirstOrDefault(x => x.Id == id);

            if (userToModify is null)
                throw new NotFoundException("User not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, userToModify,
                new UserOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidException();

            var userRole = _userContextService.GetUserRole;

            if (userRole == "Admin")
            {
                var hashedPassword = _passwordHasher.HashPassword(userToModify, dto.NewPassword);
                userToModify.Password = hashedPassword;
            }
            else
            {
                var result = _passwordHasher.VerifyHashedPassword(userToModify, userToModify.Password, dto.OldPassword);

                if (result == PasswordVerificationResult.Failed)
                    throw new BadRequestException("Invalid password");

                var hashedPassword = _passwordHasher.HashPassword(userToModify, dto.NewPassword);
                userToModify.Password = hashedPassword;
            }

            _dbContext.Users.Update(userToModify);
            _dbContext.SaveChanges();
        }

        public void Delete (int id) 
        {
            _logger.LogInformation($"User with id {id} DELETE action invoked");
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
                throw new NotFoundException("User not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user,
                new UserOperationRequirement(ResourceOperation.Delete)).Result;

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

        }

        public UserDto GetById(int id)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
                throw new NotFoundException("User not found");

            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = _dbContext
                .Users
                .ToList();

            var usersDtos = _mapper.Map<List<UserDto>>(users);

            return usersDtos;
        }
        public int Create(CreateUserDto dto)
        {

            var user = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.Password = hashedPassword;


            int[] validRoleId = { 1, 2, 3 };
            if (validRoleId.Contains(user.RoleId))            
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return user.Id;
            }
            else
            {
                throw new BadRequestException("Invalid RoleId");
            }

        }
    }
}

