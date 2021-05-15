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
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

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
        [Description("Update user by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public ActionResult Update([FromBody] UpdateUserDto dto, [FromRoute] int id)
        {
            _userService.Update(id, dto);

            return Ok();
        }

        [HttpPatch("role/{id}")]
        [Authorize(Roles = "Admin")]
        [Description("Update user Role by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult UpdateRole([FromBody] UpdateUserRoleDto dto, [FromRoute] int id)
        {
            _userService.UpdateUserRole(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Description("Delete user by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public ActionResult Delete([FromRoute] int id)
        {
            _userService.Delete(id);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Description("Create new user")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {

            var id = _userService.Create(dto);

            return Created($"/api/users/{id}", null);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        [Description("Get all users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var usersDtos = _userService.GetAll();

            return Ok(usersDtos);
        }

        [HttpGet("{id}")]
        [Description("Get user by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public ActionResult<UserDto> Get([FromRoute] int id)
        {
            var user = _userService.GetById(id);

            return Ok(user);
        }
        [HttpPatch("changePassword/{id}")]
        [Description("Change password for user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult ChangePassword([FromBody] ChangePasswordDto dto, [FromRoute] int id)
        {
            _userService.ChangePassword(id, dto);

            return Ok();
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("book/reservation/{bookId}")]
        [Description("Get all users for reserved book by BookId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<AllUserForReservedBookDto>> GetUsersForReservedBook([FromRoute] int bookId)
        {
            var result = _userService.GetAllUserForReservedBookByBookId(bookId);
            return Ok(result);
        }
    }
}
