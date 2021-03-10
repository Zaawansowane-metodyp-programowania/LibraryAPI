using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace LibraryAPI.Services
{
    public interface IBookService
    {
        int Create(CreateBookDto dto);
        IEnumerable<BookDto> GetAll();
        BookDto GetById(int id);
        bool Delete(int id);
        bool Update(int id, UpdateBookDto dto);
    }

    public class BookService : IBookService
    {
        private readonly LibraryDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(LibraryDBContext dbContext, IMapper mapper, ILogger<BookService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        
        public bool Update(int id, UpdateBookDto dto) 
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

            if (book is null)
                return false;

            book.ISBN = dto.ISBN;
            book.BookName = dto.BookName;
            book.AuthorName = dto.AuthorName;
            book.PublisherName = dto.PublisherName;
            book.PublishDate = dto.PublishDate;
            book.Category = dto.Category;
            book.Reservation = dto.Reservation;
            book.Language = dto.Language;
            book.BookDescription = dto.BookDescription;

            _dbContext.SaveChanges();

            return true;


        }

        public bool Delete(int id) 
        {
            _logger.LogError($"Book with id: {id} DELETE action invoked");

            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

             if (book is null) return false;

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();

            return true;
        }

        public BookDto GetById(int id)
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

            if (book is null) return null;

            var result = _mapper.Map<BookDto>(book);
            return result;
        }
        public IEnumerable<BookDto> GetAll()
        {
            var books = _dbContext
                .Books
                .ToList();

            var booksDtos = _mapper.Map<List<BookDto>>(books);

            return booksDtos;
        }
        public int Create(CreateBookDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            var userId = dto.UserId == 0 ? null : dto.UserId;
            book.UserId = userId;
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

            return book.Id;
        }

    }
}
