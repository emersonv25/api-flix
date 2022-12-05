using System.Security.Cryptography.X509Certificates;

namespace Api.MyFlix.Models.Object
{

    public class ReturnSerie
    {
        public ReturnSerie() { }
        public  ReturnSerie(Serie Serie)
        {
            SerieKey = Serie.SerieKey;
            Title = Serie.Title;    
            Description = Serie.Description;
            PosterImg = Serie.PosterImg;    
            ReleasedDate = Serie.ReleasedDate;
            Seasons = Serie.Seasons.Select(x => new ReturnSeason(x)).ToList();
            Categories = Serie.Categories.Select(x => x.Name).ToList();
        }

        public string SerieKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImg { get; set; }
        public string ReleasedDate { get; set; }
        public List<ReturnSeason> Seasons { get; set; }
        public List<string> Categories { get; set; }
    }
    public class ReturnSeries
    {
        public ReturnSeries() { }
        public ReturnSeries(Serie Serie)
        {
            SerieKey = Serie.SerieKey;
            Title = Serie.Title;
            Description = Serie.Description;
            PosterImg = Serie.PosterImg;
            ReleasedDate = Serie.ReleasedDate;
            Categories = Serie.Categories.Select(x => x.Name).ToList();
            CreatedDate = Serie.CreatedDate;
            Views = Serie.Views;
            LatestRelease = Serie.LatestRelease;
        }

        public string SerieKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImg { get; set; }
        public string ReleasedDate { get; set; }
        public DateTime LatestRelease { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Views { get; set; }
        public List<string> Categories { get; set; }
    }

}
