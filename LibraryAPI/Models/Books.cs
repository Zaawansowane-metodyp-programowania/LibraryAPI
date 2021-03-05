using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Books
    {        
        [Key]
        public int Id { get; set; }
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
        [Required]
        public string Category { get; set; }
        public bool Reservation { get; set; }
        public string Language { get; set; }
        public string BookDescription { get; set; }
       
        public int UsersId { get; set; }

        public virtual Users Users { get; set; }





    }
}
