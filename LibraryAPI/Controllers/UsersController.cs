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
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly LibraryDBContext _dbContext;

        public UsersController(LibraryDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var users = _dbContext
                .Users
                .ToList();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public ActionResult<User> GetById([FromRoute] int id) 
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if(user is null) 
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
