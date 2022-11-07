namespace Api.MyFlix.Models.Object
{

    public class ReturnMovie
    {
        public ReturnMovie() { }
        public  ReturnMovie(Movie movie)
        {
            MovieKey = movie.MovieKey;
            Title = movie.Title;    
            Description = movie.Description;
            PosterImg = movie.PosterImg;    
            ReleasedDate = movie.ReleasedDate;
            Seasons = movie.Seasons.Select(x => new ReturnSeason(x)).ToList();
            Categories = movie.Categories.Select(x => x.Name).ToList();
        }

        public string MovieKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImg { get; set; }
        public string ReleasedDate { get; set; }
        public List<ReturnSeason> Seasons { get; set; }
        public List<string> Categories { get; set; }
    }
    public class ReturnMovies
    {
        public ReturnMovies() { }
        public ReturnMovies(Movie movie)
        {
            MovieKey = movie.MovieKey;
            Title = movie.Title;
            Description = movie.Description;
            PosterImg = movie.PosterImg;
            ReleasedDate = movie.ReleasedDate;
            Categories = movie.Categories.Select(x => x.Name).ToList();
        }

        public string MovieKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImg { get; set; }
        public string ReleasedDate { get; set; }
        public List<string> Categories { get; set; }
    }

}
