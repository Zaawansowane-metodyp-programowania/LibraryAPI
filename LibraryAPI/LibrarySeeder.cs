using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;




namespace LibraryAPI
{
    public class LibrarySeeder
    {
        private readonly LibraryDBContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LibrarySeeder(LibraryDBContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Books.Any())
                {
                    var books = GetBooks();
                    _dbContext.Books.AddRange(books);
                    _dbContext.SaveChanges();
                }

            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Employee"
                },
                new Role()
                {
                    Name = "Admin"
                },
            };

            return roles;
        }
        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>()
                {
                    new User()
                    {
                        Name ="admin",
                        Surname = "admin",
                        Email = "admin@example.com",
                        Password = _passwordHasher.HashPassword(null, "Admin123@"),
                        RoleId=3,
                    },

                    new User()
                    {
                        Name = "Kamil",
                        Surname = "Warda",
                        Email = "warda123@wp.pl",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=2,
                    }
                };
            return users;
        }

        private IEnumerable<Book> GetBooks()
        {
            var books = new List<Book>()
            {
                new Book()
                {
                    ISBN = "978-83-283-0234-1",
                    BookName= "Czysty Kod",
                    AuthorName = "Robert C.Martin",
                    PublisherName = "Helion",
                    PublishDate = 2015,
                    Category = "Programming",
                    Language = "Polish",
                    BookDescription = "Authorized translation from the English language edition, entitled:Clean Code",

                },
                new Book()
                {
                    ISBN = "978-83-283-6150-8",
                    BookName= "Python. Wprowadzenie. Wydanie V",
                    AuthorName = "Mark Lutz",
                    PublisherName = "Helion",
                    PublishDate = 2020,
                    Category = "Programming",
                    Language = "",
                    BookDescription = "",
                },
            };
            return books;
        }



    }

}

