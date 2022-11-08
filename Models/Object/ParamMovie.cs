namespace Api.MyFlix.Models.Object
{
    public class ParamMovie
    {
        public string MovieKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImg { get; set; }
        public string ReleasedDate { get; set; }
        public List<ReturnSeason> Seasons { get; set; }
        public List<string> Categories { get; set; }
    }
}
