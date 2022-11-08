using Api.MyFlix.Models.Object;
using Api.MyFlix.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Api.MyFlix.Models
{
    [Index(nameof(MovieKey))]
    public class Movie
    {
        public Movie() { }
        public Movie(string title, string description, string posterImg, string releasedDate, List<ParamSeason> seasons, List<Category> categories)
        {
            MovieKey = Utils.ReplaceSpecialChar(title.ToLower()).Replace(' ', '-');
            Title = title;
            Description = description;
            PosterImg = posterImg;
            ReleasedDate = releasedDate;
            Seasons = seasons.Select(s => new Season(s, MovieKey)).ToList();
            Categories = categories;
        }
        [Key]
        public int MovieId { get; set; }
        public string MovieKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImg { get; set; }
        public string ReleasedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<Season> Seasons { get; set; }

        public List<Category> Categories { get; set; }

    }
}
