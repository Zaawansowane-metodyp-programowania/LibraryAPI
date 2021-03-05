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
    public class LibraryController : ControllerBase
    {
        private readonly LibraryDBContext _dbContext;
        private readonly IMapper _mapper;

        //Dostęp do kontekstu baz danych
        public LibraryController(LibraryDBContext dBContext, IMapper mapper)
        {
            _dbContext = dBContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Metoda zwracająca wszystkie książki z bazy danych 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<BooksDto>> GetAll()
        {
            var book = _dbContext
                .Books
                .ToList();

            var bookDtos = _mapper.Map<List<BooksDto>>(book);
           

            return Ok(bookDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BooksDto> GetById([FromRoute]int id)
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

                if (book is null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BooksDto>(book);
            return Ok(bookDto);

        }
    }
}
