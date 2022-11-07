using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.MyFlix.Models
{
    [Index(nameof(EpisodeKey))]
    public class Episode
    {
        [Key]
        public int EpisodeId { get; set; }
        public string? EpisodeKey { get; set; }
        public int EpisodeNum { get; set; }
        public string? Title { get; set; }
        public string?  Description { get; set; }
        public string? EpisodeUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
