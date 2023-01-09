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
            EpisodeVideos = returnEpisode.EpisodeVideos.Select(e => new EpisodeVideo(e.OptionName, e.VideoUrl, e.IsIframe)).ToList();
        }
        public Episode(ParamEpisode paramEpisode, int seasonNum ,string SerieKey)
        {
            //EpisodeKey = $"{SerieKey}-temporada-{seasonNum}-episodio-{paramEpisode.EpisodeNum}";
            EpisodeKey = $"{SerieKey}{(seasonNum == 1 ? "" : "-" + seasonNum)}-episodio-{paramEpisode.EpisodeNum}";
            EpisodeNum = paramEpisode.EpisodeNum;
            Title = paramEpisode.Title;
            Description = paramEpisode.Description;
            EpisodeVideos = paramEpisode.EpisodeVideos.Select(e => new EpisodeVideo(e.OptionName, e.VideoUrl, e.IsIframe)).ToList();
            EpisodeImg = paramEpisode.EpisodeImg;
            ReleasedDate = paramEpisode.ReleasedDate;
        }

        [Key]
        public int EpisodeId { get; set; }
        public string EpisodeKey { get; set; }
        public int EpisodeNum { get; set; }
        public string Title { get; set; }
        public string  Description { get; set; }
        public List<EpisodeVideo> EpisodeVideos { get; set; }
        public string EpisodeImg { get; set; }
        public string ReleasedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int Views { get; set; }

        [ForeignKey("Season")]
        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}
