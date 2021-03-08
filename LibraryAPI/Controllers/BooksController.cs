using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Dtos;
using AutoMapper;




namespace LibraryAPI.Controllers
{
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDBContext _dbContext;
        private readonly IMapper _mapper;

        //Dostęp do kontekstu baz danych
        public BooksController(LibraryDBContext dBContext, IMapper mapper)
        {
            _dbContext = dBContext;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult CreateBook([FromBody] CreateBookDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            var userId = dto.UserId == 0 ? null : dto.UserId;
            book.UserId = userId;
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

            return Created($"api/books/ {book.Id}", null);

        }
            


        /// <summary>
        /// Metoda zwracająca wszystkie książki z bazy danych 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetAll()
        {
            var book = _dbContext
                .Books
                .ToList();

            var bookDtos = _mapper.Map<List<BookDto>>(book);
           

            return Ok(bookDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> GetById([FromRoute]int id)
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

                if (book is null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDto>(book);
            return Ok(bookDto);

        }
    }
}
