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
        [Required]
        public string Category { get; set; }
        public string Language { get; set; }
        public string BookDescription { get; set; }

    }
}
