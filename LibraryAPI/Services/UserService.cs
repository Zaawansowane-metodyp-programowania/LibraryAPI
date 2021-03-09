using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using AutoMapper;


namespace LibraryAPI.Services
{
    public interface IUserService
    {
        int Create(CreateUserDto dto);
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        bool Delete(int id);
        bool Update(int id, UpdateUserDto dto);
    }

    public class UserService : IUserService
    {
        private readonly LibraryDBContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(LibraryDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool Update(int id, UpdateUserDto dto)
        {
            var user = _dbContext
               .Users
               .FirstOrDefault(u => u.Id == id);

            if (user is null) 
                return false;

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;

            _dbContext.SaveChanges();

            return true;
        }

        public bool Delete (int id) 
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if (user is null) return false;

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return true;

        }

        public UserDto GetById(int id)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if (user is null) return null;

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
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;

        }
    }
}

