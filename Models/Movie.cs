using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.MyFlix.Models
{
    [Index(nameof(MovieKey))]
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string? MovieKey { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PosterImg { get; set; }
        public string? ReleasedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<Season>? Seasons { get; set; }

        public List<Category>? Categories { get; set; }

    }
}
