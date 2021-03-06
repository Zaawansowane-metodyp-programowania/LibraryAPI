﻿using LibraryAPI.Dtos;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        public LoginVm GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly LibraryDBContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(LibraryDBContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
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

            int[] validRoleId = { 1, 2, 3 };
            if (validRoleId.Contains(dto.RoleId))
            {
                var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

                newUser.Password = hashedPassword;
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
            else
            {
                throw new BadRequestException("Invalid RoleId");
            }

            
        }
        public LoginVm GenerateJwt(LoginDto dto)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var tokenJwt = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenJwt);

            var loginVm = new LoginVm
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Token = token
            };

            return loginVm;
        }
    }
}
