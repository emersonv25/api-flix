namespace Api.MyFlix.Models.Object
{
    public class ReturnEpisode
    {
        public ReturnEpisode() { }
        public ReturnEpisode(Episode episode)
        {
            EpisodeKey = episode.EpisodeKey;
            EpisodeNum = episode.EpisodeNum;
            Title = episode.Title;
            Description = episode.Description;
            EpisodeUrl = episode.EpisodeUrl;
        }

        public string EpisodeKey { get; set; }
        public int EpisodeNum { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EpisodeUrl { get; set; }
    }
}
