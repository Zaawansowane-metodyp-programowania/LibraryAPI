using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Services
{
    public interface IBookService
    {
        int Create(CreateBookDto dto);
        IEnumerable<BookDto> GetAll();
        BookDto GetById(int id);
        void Delete(int id);
        void Update(int id, UpdateBookDto dto);
        List<BookDto> GetAllbyUser(int UserId);
        void UpdateReservationById(int id, UpdateBookDto dto);
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
        
        public void Update(int id, UpdateBookDto dto) 
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

            if (book is null)
                throw new NotFoundException("Book not found");

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

        }


        public void UpdateReservationById(int id, UpdateBookDto dto)
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

            if (book is null)
                throw new NotFoundException("Book not found");

           
            book.Reservation = dto.Reservation;
            
            _dbContext.SaveChanges();

        }


        public void Delete(int id) 
        {
            _logger.LogError($"Book with id: {id} DELETE action invoked");

            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

             if (book is null)
                throw new NotFoundException("Book not found");

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();

        }

        public BookDto GetById(int id)
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

            if (book is null)
                throw new NotFoundException("Book not found");

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

        public List<BookDto> GetAllbyUser (int UserId)
        {
            var user = _dbContext
                .Users
                .Include(r => r.Books)
                .FirstOrDefault(r => r.Id == UserId);
            if (user is null)
                throw new NotFoundException("User not found");

            var bookDtos = _mapper.Map<List<BookDto>>(user.Books);

            return bookDtos;
        }

    }
}
