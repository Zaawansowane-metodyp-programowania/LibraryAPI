using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class Book
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
        public string Language { get; set; }
        public string BookDescription { get; set; }

        public int? UserId { get; set; }

        public virtual User Users { get; set; }
        public DateTime? BorrowedAt { get; set; }

        public DateTime? ReturningTime { get; set; }

        public virtual List<UserBookReservation> Reservations { get; set; }





    }
}
