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
            EpisodeVideo = episode.EpisodeVideo.Split(';').ToList();
            EpisodeImg = episode.EpisodeImg;
            CreatedDate = episode.CreatedDate;
            Views = episode.Views;   
            IsIframe = episode.IsIframe;
        }

        public string EpisodeKey { get; set; }
        public int EpisodeNum { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> EpisodeVideo { get; set; }
        public string EpisodeImg { get; set; }
        public string SerieKey { get; set; }
        public string SeasonKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Views { get; set; }
        public string PreviousEpisodeKey { get; set; } = null;
        public string NextEpisodeKey { get; set; } = null;
        public bool IsIframe { get; set; } = true;
    }
}
