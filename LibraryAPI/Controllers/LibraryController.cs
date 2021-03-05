using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers
{
    [Route("api/books")]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryDBContext _dbContext;
        //Dostęp do kontekstu baz danych
        public LibraryController(LibraryDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        /// <summary>
        /// Metoda zwracająca wszystkie książki z bazy danych 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Books>> GetAll()
        {
            var books = _dbContext
                .Books
                .ToList();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public ActionResult<Books> GetById([FromRoute]int id)
        {
            var book = _dbContext
                .Books
                .FirstOrDefault(r => r.Id == id);

                if(book is null)
            {
                return NotFound();
            }

            return Ok(book);

        }
    }
}
