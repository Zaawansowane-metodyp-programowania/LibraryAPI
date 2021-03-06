using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace LibraryAPI.Dtos
{
    public class CreateBookDto
    {
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string BookName { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public string PublisherName { get; set; }
        [Required]
        public int PublishDate { get; set; }
        public string Category { get; set; }
        public bool Reservation { get; set; }
        public string Language { get; set; }
        public string BookDescription { get; set; }      
        public int? UserId { get; set; }
       

    }
}
