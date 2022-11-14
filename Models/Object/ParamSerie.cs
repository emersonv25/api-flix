namespace Api.MyFlix.Models.Object
{
    public class ParamSerie
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterImg { get; set; }
        public string ReleasedDate { get; set; }
        public List<ParamSeason> Seasons { get; set; }
        public List<string> Categories { get; set; }
    }
}
