using Api.MyFlix.Models.Object;
using Api.MyFlix.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.MyFlix.Models
{
    [Index(nameof(SerieKey))]
    public class Serie
    {
        public Serie() { }
        public Serie(string title, string description, string posterImg, string releasedDate, List<ParamSeason> seasons, List<Category> categories)
        {
            SerieKey = Utils.ReplaceSpecialChar(title.ToLower()).Replace(' ', '-');
            Title = title;
            Description = description;
            PosterImg = posterImg;
            ReleasedDate = releasedDate;
            Seasons = seasons.Select(s => new Season(s, SerieKey)).ToList();
            Categories = categories;
        }
        [Key]
        public int SerieId { get; set; }
        public string SerieKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImg { get; set; }
        public string ReleasedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<Season> Seasons { get; set; }

        public List<Category> Categories { get; set; }

    }
}
