namespace Api.MyFlix.Models.Object
{
    public class ParamSeason
    {
        public int SeasonNum { get; set; }
        public List<ParamEpisode> Episodes { get; set; }
    }
}
