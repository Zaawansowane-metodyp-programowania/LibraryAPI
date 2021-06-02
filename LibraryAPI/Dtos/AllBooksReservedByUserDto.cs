using System;

namespace LibraryAPI.Dtos
{
    public class AllBooksReservedByUserDto
    {
        public int UserId { get; set; }
        public DateTime ReservationTime { get; set; }
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public int PublishDate { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string BookDescription { get; set; }
    }
}
