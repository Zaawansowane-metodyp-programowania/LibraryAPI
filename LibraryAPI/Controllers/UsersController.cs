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
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly LibraryDBContext _dbContext;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
           var isDeleted = _userService.Delete(id);

            if(isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateUser ([FromBody]CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var id =  _userService.Create(dto);

            return Created($"/api/users/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var usersDtos = _userService.GetAll();

            return Ok(usersDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> Get([FromRoute] int id) 
        {
            var user = _userService.GetById(id);
           
            if(user is null) 
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
