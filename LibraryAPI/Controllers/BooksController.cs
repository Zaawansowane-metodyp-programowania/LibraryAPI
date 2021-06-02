using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Dtos;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

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
        [Description("Update book by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin,Employee")]
        public ActionResult Update([FromBody] UpdateBookDto dto, [FromRoute] int id)
        {

            _bookService.Update(id, dto);

            return Ok();
        }

        [HttpPatch("reservation/{id}")]
        [Description("Reserve a book")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult AddReservation([FromRoute] int id)
        {

            _bookService.AddReservationById(id);

            return Ok();
        }

        [HttpDelete("reservation/{id}")]
        [Description("Delete a Reservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult DeleteReservation([FromRoute] int id)
        {

            _bookService.DeleteReservationById(id);

            return Ok();
        }


        [HttpPatch("borrow/{id}")]
        [Authorize(Roles = "Admin,Employee")]
        [Description("Borrow book for user by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult BorrowBook([FromBody] BorrowBookDto dto, [FromRoute] int id)
        {

            _bookService.BorrowBookById(id, dto);

            return Ok();
        }

        [HttpPatch("return/{id}")]
        [Authorize(Roles = "Admin,Employee")]
        [Description("Return book from user to library by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult ReturnBook([FromRoute] int id)
        {

            _bookService.ReturnBookById(id);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        [Description("Delete book by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Delete([FromRoute] int id)
        {
            _bookService.Delete(id);

            return NoContent();


        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [Description("Create new book")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult CreateBook([FromBody] CreateBookDto dto)
        {
            var id = _bookService.Create(dto);

            return Created($"api/books/ {id}", null);

        }



        [HttpGet]
        [Description("Get all books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<BookDto>> GetAll([FromQuery] BookQuery query)
        {
            var booksDtos = _bookService.GetAll(query);


            return Ok(booksDtos);
        }

        [HttpGet("{id}")]
        [Description("Get book by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<BookDto> Get([FromRoute] int id)
        {
            var book = _bookService.GetById(id);

            return Ok(book);
        }

        [HttpGet("user/{userId}")]
        [Description("Get all books for user by user id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<BookDto>> GetBooks([FromRoute] int userId)
        {
            var result = _bookService.GetAllByUser(userId);
            return Ok(result);
        }

        [HttpGet("user/reservation/{userId}")]
        [Description("Get All Books Reserved by user id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<AllBooksReservedByUserDto>> GetReservedBooks([FromRoute] int userId)
        {
            var result = _bookService.GetAllReservedBooksByUserId(userId);
            return Ok(result);
        }
    }
}
