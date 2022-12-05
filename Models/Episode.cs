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
            EpisodeVideo = string.Join(";", returnEpisode.EpisodeVideo);
            IsIframe = returnEpisode.IsIframe;
        }
        public Episode(ParamEpisode paramEpisode, int seasonNum ,string SerieKey)
        {
            EpisodeKey = $"{SerieKey}-temporada-{seasonNum}-episodio-{paramEpisode.EpisodeNum}";
            EpisodeNum = paramEpisode.EpisodeNum;
            Title = paramEpisode.Title;
            Description = paramEpisode.Description;
            EpisodeVideo = string.Join(";", paramEpisode.EpisodeVideo);
            EpisodeImg = paramEpisode.EpisodeImg;
            IsIframe= paramEpisode.IsIframe;
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
        public int Views { get; set; }
        public bool IsIframe { get; set; } = true;

        [ForeignKey("Season")]
        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}
