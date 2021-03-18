using LibraryAPI.Dtos;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly LibraryDBContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(LibraryDBContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = dto.RoleId
            };

           var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.Password = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}
