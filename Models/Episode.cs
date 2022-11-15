using Api.MyFlix.Models.Object;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
            EpisodeVideo = returnEpisode.EpisodeVideo;
        }
        public Episode(ParamEpisode paramEpisode, int seasonNum ,string SerieKey)
        {
            EpisodeKey = $"{SerieKey}-temporada-{seasonNum}-episodio-{paramEpisode.EpisodeNum}";
            EpisodeNum = paramEpisode.EpisodeNum;
            Title = paramEpisode.Title;
            Description = paramEpisode.Description;
            EpisodeVideo = paramEpisode.EpisodeVideo;
            EpisodeImg = paramEpisode.EpisodeImg;
        }

        [Key]
        public int EpisodeId { get; set; }
        public string EpisodeKey { get; set; }
        public int EpisodeNum { get; set; }
        public string Title { get; set; }
        public string  Description { get; set; }
        public string EpisodeVideo { get; set; }
        public string EpisodeImg { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Season")]
        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}
