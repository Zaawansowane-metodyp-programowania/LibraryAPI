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
        private IEnumerable<Users> GetUsers()
        {
            var users = new List<Users>()
                {
                    new Users()
                    {
                        Name ="Karol",
                        Surname = "Kowalski",
                        Email = "karolek123@o2.pl",
                        Password = "t32423",
                        Authorization=1,
                    },

                    new Users()
                    {
                        Name = "Marcin",
                        Surname = "Zawada",
                        Email = "zawadzior125@wp.pl",
                        Password = "43423",
                        Authorization=2,
                    }
                };
            return users;
        }

        private IEnumerable<Books> GetBooks()
        {
            var books = new List<Books>()
            {
                new Books()
                {
                    ISBN = "978-83-283-0234-1",
                    BookName= "Czysty Kod",
                    AuthorName = "Robert C.Martin",
                    PublisherName = "Helion",
                    PublishDate = 2015,
                    Language = "Polish",
                    BookDescription = "Authorized translation from the English language edition, entitled:Clean Code",
                    UsersId =2,

                },
                new Books()
                {
                    ISBN = "978-83-283-6150-8",
                    BookName= "Python. Wprowadzenie. Wydanie V",
                    AuthorName = "Mark Lutz",
                    PublisherName = "Helion",
                    PublishDate = 2020,
                    Language = "",
                    BookDescription = "",
                    UsersId =1,
                },
            };
            return books;
        }

        

    }

}

