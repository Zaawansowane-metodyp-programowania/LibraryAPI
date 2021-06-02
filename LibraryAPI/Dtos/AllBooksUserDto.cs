using System;

namespace LibraryAPI.Dtos
{
    public class AllBooksUserDto
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public int PublishDate { get; set; }
        public string Category { get; set; }
        public bool Reservation { get; set; }
        public string Language { get; set; }
        public string BookDescription { get; set; }
        public int? UserId { get; set; }
        public DateTime? BorrowedAt { get; set; }
        public DateTime? ReturningTime { get; set; }
    }
}
