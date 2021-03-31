using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models;




namespace LibraryAPI
{
    public class LibrarySeeder
    {
        private readonly LibraryDBContext _dbContext;
        public LibrarySeeder(LibraryDBContext dbContext)
        {
            _dbContext = dbContext;
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
                        Name ="Karol",
                        Surname = "Kowalski",
                        Email = "karolek123@o2.pl",
                        Password = "t32423",
                        RoleId=1,
                    },

                    new User()
                    {
                        Name = "Marcin",
                        Surname = "Zawada",
                        Email = "zawadzior125@wp.pl",
                        Password = "43423",
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

