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
    [ApiController]
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

            _bookService.Update(id, dto);
            
            return Ok();
        }

        [HttpPut("reservation/{id}")]
        public ActionResult UpdateReservation([FromBody] UpdateBookDto dto, [FromRoute] int id)
        {

            _bookService.UpdateReservationById(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _bookService.Delete(id);

            return NotFound();


        }

        [HttpPost]
        public ActionResult CreateBook([FromBody] CreateBookDto dto)
        {
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

            return Ok(book);
        }

        [HttpGet("user/{userId}")]
        public ActionResult<List<BookDto>> GetBooks([FromRoute]int userId)
        {
            var result = _bookService.GetAllbyUser(userId);
            return Ok(result);
        }
    }
}
