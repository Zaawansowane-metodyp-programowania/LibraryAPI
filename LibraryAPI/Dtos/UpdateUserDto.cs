using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Dtos
{
    public class UpdateUserDto
    {
        
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
