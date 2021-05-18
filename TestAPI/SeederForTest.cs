using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;




namespace TestAPI
{
    public class SeederForTest
    {
        private readonly LibraryDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;

        public SeederForTest(LibraryDBContext dbContext, PasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public void Seed()
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

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {   Id=1,
                    Name = "User"
                },
                new Role()
                {   Id=2,
                    Name = "Employee"
                },
                new Role()
                {   Id=3,
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
                    {   Id = 1,
                        Name ="admin",
                        Surname = "test",
                        Email = "adminTest@example.com",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=3,
                    },

                    new User()
                    {   Id = 2,
                        Name = "employee",
                        Surname = "test",
                        Email = "employeeTest@example.com",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=2,
                    },

                     new User()
                    {   Id = 3,
                        Name = "user",
                        Surname = "test",
                        Email = "userTest@example.com",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=1,
                    },
                     new User()
                    {   Id = 4,
                        Name = "user",
                        Surname = "test",
                        Email = "userqTest@example.com",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=1,
                    },

                     new User()
                    {   Id = 5,
                        Name = "user",
                        Surname = "test",
                        Email = "userqqTest@example.com",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=1,
                    },

                      new User()
                    {   Id = 6,
                        Name = "user",
                        Surname = "test",
                        Email = "user2Test@example.com",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=1,
                    },

                        new User()
                    {   Id = 7,
                        Name = "user",
                        Surname = "test",
                        Email = "user3Test@example.com",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=1,
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
                    UserId =2,

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
                    UserId =1,
                },
            };
            return books;
        }



    }

}

