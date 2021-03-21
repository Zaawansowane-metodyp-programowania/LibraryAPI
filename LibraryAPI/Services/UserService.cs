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

namespace LibraryAPI.Services
{
    public interface IUserService
    {
        int Create(CreateUserDto dto);
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        void Delete(int id);
        void Update(int id, UpdateUserDto dto);
    }

    public class UserService : IUserService
    {
        private readonly LibraryDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(LibraryDBContext dbContext, IMapper mapper,ILogger<UserService> logger, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public void Update(int id, UpdateUserDto dto)
        {
            var user = _dbContext
               .Users
               .FirstOrDefault(u => u.Id == id);

            if (user is null)
                throw new NotFoundException("User not found");

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;

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
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;

        }
    }
}

