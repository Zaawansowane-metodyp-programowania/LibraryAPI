using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class Books
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string book_name { get; set; }
        public string author_name { get; set; }
        public string publisher_name { get; set; }
        public string language { get; set; }
        public string book_description { get; set; }
        public int publish_date { get; set; }
        public int actual_stock { get; set; }
        public int current_stock { get; set; }
       

        public virtual ICollection<BooksUsers> Users { get; set; }



    }
}
