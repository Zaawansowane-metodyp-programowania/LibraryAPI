using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class UserBookReservation
    {
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
        public int BookId { get; set; }
        public DateTime ReservationTime { get; set; }
    }
}
