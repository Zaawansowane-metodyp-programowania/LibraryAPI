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
                        Name ="Dariusz",
                        Surname = "Kowalski",
                        Email = "dariusz@o2.pl",
                        Password = _passwordHasher.HashPassword(null, "User123@"),
                        RoleId=1,
                    },

                    new User()
                    {   Id = 2,
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
                    Reservation = true,
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
                    Reservation = false,
                    Language = "",
                    BookDescription = "",
                    UserId =1,
                },
            };
            return books;
        }



    }

}

