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
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;


        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateUserDto dto, [FromRoute] int id)
        {

            _userService.Update(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _userService.Delete(id);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {

            var id = _userService.Create(dto);

            return Created($"/api/users/{id}", null);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var usersDtos = _userService.GetAll();

            return Ok(usersDtos);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id}")]
        public ActionResult<UserDto> Get([FromRoute] int id)
        {
            var user = _userService.GetById(id);

            return Ok(user);
        }
        [HttpPatch("changePassword/{id}")]
        public ActionResult ChangePassword([FromBody] ChangePasswordDto dto, [FromRoute] int id)
        {
            _userService.ChangePassword(id, dto);

            return Ok();
        }
    }
}
