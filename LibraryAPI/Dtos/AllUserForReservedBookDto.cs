using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Dtos
{
    public class AllUserForReservedBookDto
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public DateTime ReservationTime { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
