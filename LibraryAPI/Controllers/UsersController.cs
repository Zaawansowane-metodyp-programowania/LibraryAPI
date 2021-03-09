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
       

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateUserDto dto, [FromRoute]int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdate = _userService.Update(id, dto);
            if(!isUpdate)
            {
                return NotFound();
            }

            return Ok();
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
