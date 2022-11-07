using System.ComponentModel.DataAnnotations;

namespace MyFlix.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Rating { get; set; }
        public string? PosterUrl { get; set; }
        public string? LaunchDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<Season>? Seasons { get; set; }

        public List<Category>? Categories { get; set; }

    }
}
