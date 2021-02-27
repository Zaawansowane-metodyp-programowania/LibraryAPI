using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class BooksUsers
    {
        public int BooksId { get; set; }
        public Books Books { get; set; }
        public int UsersId { get; set; }
        public Users Users { get; set; }
        public DateTime issue_date { get; set; }
        public DateTime due_date { get; set; }

    }
}
