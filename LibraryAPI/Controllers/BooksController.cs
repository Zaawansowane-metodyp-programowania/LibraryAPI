using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Dtos;
using AutoMapper;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public ActionResult Update([FromBody] UpdateBookDto dto, [FromRoute] int id)
        {

            _bookService.Update(id, dto);

            return Ok();
        }

        [HttpPatch("reservation/{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public ActionResult UpdateReservation([FromBody] UpdateBookReservationDto dto, [FromRoute] int id)
        {

            _bookService.UpdateReservationById(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public ActionResult Delete([FromRoute] int id)
        {
            _bookService.Delete(id);

            return NoContent();


        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
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
        public ActionResult<IEnumerable<BookDto>> GetAll([FromQuery] BookQuery query)
        {
            var booksDtos = _bookService.GetAll(query);


            return Ok(booksDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> Get([FromRoute] int id)
        {
            var book = _bookService.GetById(id);

            return Ok(book);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,Employee")]
        public ActionResult<List<BookDto>> GetBooks([FromRoute] int userId)
        {
            var result = _bookService.GetAllbyUser(userId);
            return Ok(result);
        }
    }
}
