using System.ComponentModel.DataAnnotations;

namespace MyFlix.Models
{
    public class Episode
    {
        [Key]
        public int EpisodeId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string?  Description { get; set; }
        public string VideoUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
