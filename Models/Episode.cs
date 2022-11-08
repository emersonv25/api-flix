using Api.MyFlix.Models.Object;
using Api.MyFlix.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.MyFlix.Models
{
    [Index(nameof(EpisodeKey))]
    public class Episode
    {
        public Episode () { }
        public Episode(ReturnEpisode returnEpisode)
        {
            EpisodeKey = returnEpisode.EpisodeKey;
            EpisodeNum = returnEpisode.EpisodeNum;  
            Title = returnEpisode.Title;    
            Description = returnEpisode.Description;    
            EpisodeUrl = returnEpisode.EpisodeUrl;
        }
        public Episode(ParamEpisode paramEpisode, string movieKey)
        {
            EpisodeKey = $"{movieKey}-episodio-{paramEpisode.EpisodeNum}";
            EpisodeNum = paramEpisode.EpisodeNum;
            Title = paramEpisode.Title;
            Description = paramEpisode.Description;
            EpisodeUrl = paramEpisode.EpisodeUrl;
        }

        [Key]
        public int EpisodeId { get; set; }
        public string EpisodeKey { get; set; }
        public int EpisodeNum { get; set; }
        public string Title { get; set; }
        public string  Description { get; set; }
        public string EpisodeUrl { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
