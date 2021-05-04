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
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using LibraryAPI.Authorization;

namespace LibraryAPI.Services
{
    public interface IBookService
    {
        int Create(CreateBookDto dto);
        PagedResult<BookDto> GetAll(BookQuery query);
        BookDto GetById(int id);
        void Delete(int id);
        void Update(int id, UpdateBookDto dto);
        List<AllBooksUserDto> GetAllbyUser(int UserId);
        void AddReservationById(int id);
        void DeleteReservationById(int id);
        void BorrowBookById(int id, BorrowBookDto dto);
        void ReturnBookById(int id);
    }

    public class BookService : IBookService
    {
        private readonly LibraryDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;

        public BookService(LibraryDBContext dbContext, IMapper mapper, ILogger<BookService> logger,IUserContextService userContextService,IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
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
            book.Language = dto.Language;
            book.BookDescription = dto.BookDescription;

            _dbContext.SaveChanges();

        }


        public void AddReservationById(int id)
        {
            var book = _dbContext
                .Books
                .Include(x => x.Reservations)
                .FirstOrDefault(r => r.Id == id);

            if (book is null)
                throw new NotFoundException("Book not found");

            var userId = _userContextService.GetUserId;
            if (userId is null)
            {
                throw new ForbidException();
            }

            var reservationFromBase = book.Reservations
                .FirstOrDefault(x =>
                    x.UserId == userId && x.BookId == id);

            if (reservationFromBase != null)
            {
                throw new BadRequestException("User already has a reservation for this book");
            }

            var reservation = new UserBookReservation
            {
                UserId = (int)userId,
                BookId = id,
                ReservationTime = DateTime.UtcNow
            };

            _dbContext.UserBookReservations.Add(reservation);
            _dbContext.SaveChanges();
        }

        public void DeleteReservationById(int id)
        {
            var book = _dbContext
                .Books
                .Include(x => x.Reservations)
                .FirstOrDefault(r => r.Id == id);

            if (book is null)
                throw new NotFoundException("Book not found");

            var userId = _userContextService.GetUserId;
            if (userId is null)
            {
                throw new ForbidException();
            }

            var reservation =
                book.Reservations
                    .FirstOrDefault(x =>
                        x.UserId == userId && x.BookId == id);

            if (reservation == null)
            {
                throw new NotFoundException("Reservation not found");
            }

            _dbContext.UserBookReservations.Remove(reservation);
            _dbContext.SaveChanges();

        }

        public void BorrowBookById(int id, BorrowBookDto dto)
        {
            var book = _dbContext
                .Books
                .Include(x => x.Reservations)
                .FirstOrDefault(r => r.Id == id);

            if (book is null)
                throw new NotFoundException("Book not found");

            if (book.UserId != null)
                throw new BadRequestException("The book is currently on loan");

            var userId = dto.UserId;

            if (userId is null)
                throw new ForbidException();

            var user = _dbContext.Users.Include(x => x.Books)
                .FirstOrDefault(x => x.Id == userId);

            if (user is null)
                throw new NotFoundException("User not found");

            if (user.Books.Count >= 5)
                throw new BadRequestException("User can borrow only 5 books");

            var reservation = book
                .Reservations
                .OrderBy(x => x.ReservationTime)
                .FirstOrDefault();

            if (reservation != null && reservation.UserId != userId)
            {
                throw new BadRequestException("Another user has first reservation");
            }

            if (reservation != null)
            {
                _dbContext.UserBookReservations.Remove(reservation);
            }

            book.UserId = userId;
            book.BorrowedAt = DateTime.UtcNow;
            book.ReturningTime = DateTime.UtcNow.AddDays(14);

            _dbContext.Books.Update(book);
            _dbContext.SaveChanges();

        }


        public void ReturnBookById(int id)
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

            if (book is null)
                throw new NotFoundException("Book not found");

            book.UserId = null;
            book.BorrowedAt = null;
            book.ReturningTime = null;

            _dbContext.Books.Update(book);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"Book with id: {id} DELETE action invoked");

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
        public PagedResult<BookDto> GetAll(BookQuery query)
        {
            var baseQuery = _dbContext
                .Books
                .Where(r => query.SearchPhrase == null || (r.BookName.ToLower().Contains(query.SearchPhrase.ToLower())
                            || r.PublisherName.ToLower().Contains(query.SearchPhrase.ToLower())
                            || r.AuthorName.ToLower().Contains(query.SearchPhrase.ToLower())
                            || r.BookDescription.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Book, object>>>
                {
                    { nameof(Book.BookName), r => r.BookName },
                    { nameof(Book.BookDescription), r => r.BookDescription },
                    { nameof(Book.Category), r => r.Category},
                    { nameof(Book.PublisherName), r => r.PublisherName},
                    { nameof(Book.PublishDate), r => r.PublishDate},

                };
                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                     ? baseQuery.OrderBy(selectedColumn)
                     : baseQuery.OrderByDescending(selectedColumn);
            }

            var books = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();
            var totalItemsCount = baseQuery.Count();

            var booksDtos = _mapper.Map<List<BookDto>>(books);

            var result = new PagedResult<BookDto>(booksDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }
        public int Create(CreateBookDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            //var userId = dto.UserId == 0 ? null : dto.UserId;
            //book.UserId = userId;
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

            return book.Id;
        }

        public List<AllBooksUserDto> GetAllbyUser(int UserId)
        {
            var user = _dbContext
                .Users
                .Include(r => r.Books)
                .FirstOrDefault(r => r.Id == UserId);
            if (user is null)
                throw new NotFoundException("User not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user,
               new UserOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidException();

            var bookDtos = _mapper.Map<List<AllBooksUserDto>>(user.Books);

            return bookDtos;
        }

    }
}
