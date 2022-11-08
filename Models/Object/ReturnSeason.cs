namespace Api.MyFlix.Models.Object
{
    public class ReturnSeason
    {
        public ReturnSeason() { }
        public ReturnSeason(Season season)
        {
            SeasonKey = season.SeasonKey;
            SeasonNum = season.SeasonNum;
            Episodes = season.Episodes.Select(x => new ReturnEpisode(x)).ToList();
        }

        public string SeasonKey { get; set; }
        public int SeasonNum { get; set; }
        public List<ReturnEpisode> Episodes { get; set; }
    }
}
