using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
