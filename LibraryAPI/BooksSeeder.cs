using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using LibraryAPI.Models;

namespace LibraryAPI
{
    public class BooksSeeder
    {
        private readonly Faker _faker;
        private readonly LibraryDBContext _context;
        private readonly Random _random;

        public BooksSeeder(LibraryDBContext context)
        {
            _faker = new Faker();
            _random = new Random();
            _context = context;
        }

        public void SeedBooks()
        {
            if (_context.Books.Count() < 100)
            {
                var bookGenerator = new Faker<Book>()
                    .RuleFor(x => x.BookName,
                        f => f.Lorem.Sentence())
                    .RuleFor(x => x.AuthorName,
                        f => f.Person.FullName)
                    .RuleFor(x => x.PublishDate,
                        f => GeneratePublicDate())
                    .RuleFor(x => x.ISBN,
                        f => GenerateISBN())
                    .RuleFor(x => x.Category,
                        f => f.PickRandom<Categories>().ToString())
                    .RuleFor(x => x.PublisherName,
                        f => f.PickRandom<PublisherNames>().ToString())
                    .RuleFor(x => x.Language,
                        f => f.PickRandom<Languages>().ToString())
                    .RuleFor(x => x.BookDescription,
                        f => f.Lorem.Paragraph(1));


                var books = bookGenerator.Generate(1);

                _context.AddRange(books);
                _context.SaveChanges();
            }
        }

        public string GenerateISBN()
        {
            var numbers =
                Enumerable.Range(0, 9)
                    .Select(x => x.ToString())
                    .ToList();

            var query = numbers
                .OrderBy(x => _random.Next()).AsQueryable();

            var strings = new List<string>
            {
                string.Join("", query.Take(3).ToList()),
                query.FirstOrDefault(),
                string.Join("", query.Take(2).ToList()),
                string.Join("", query.Take(6).ToList()),
                query.FirstOrDefault(),
            };

            var isbn = new StringBuilder()
                .Append(string.Join("-", strings));

            return isbn.ToString();
        }

        public int GeneratePublicDate() =>
             _random.Next(1850, 2021);
    }

    public enum PublisherNames
    {
        Helion,
        Znak,
        PWN,
        Forum,
        Publicat
    }

    public enum Categories
    {
        Horror,
        Comedy,
        Drama,
        Adventure,
        Documentary
    }

    public enum Languages
    {
        PL,
        EN,
        DE,
        ES,
    }
}
