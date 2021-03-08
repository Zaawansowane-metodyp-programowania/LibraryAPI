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
        private readonly IMapper _mapper;

        public UsersController(LibraryDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var users = _dbContext
                .Users
                .ToList();

            var usersDtos = _mapper.Map<List<UserDto>>(users);

            return Ok(usersDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetById([FromRoute] int id) 
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if(user is null) 
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
    }
}
