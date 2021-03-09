using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Dtos;
using AutoMapper;
using LibraryAPI.Services;




namespace LibraryAPI.Controllers
{
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
       
        public BooksController(IBookService  bookService)
        {
            _bookService = bookService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateBookDto dto, [FromRoute]int id) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

           var isUpdated = _bookService.Update(id, dto);
            if(!isUpdated)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
           var isDeleted = _bookService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();


        }

        [HttpPost]
        public ActionResult CreateBook([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var id = _bookService.Create(dto);
           
           return Created($"api/books/ {id}", null);

        }
            
        /// <summary>
        /// Metoda zwracająca wszystkie książki z bazy danych 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetAll()
        {
            var booksDtos = _bookService.GetAll();
           

            return Ok(booksDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> Get([FromRoute]int id)
        {
            var book = _bookService.GetById(id);

            if (book is null) 
            {
                return NotFound();
            }

            return Ok(book);
        }
    }
}
