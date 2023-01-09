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
            ReleasedDate = episode.ReleasedDate;
            EpisodeVideos = episode.EpisodeVideos.Select(e => new ReturnEpisodeVideo(e.OptionName, e.VideoUrl, e.IsIframe)).ToList(); ;
            EpisodeImg = episode.EpisodeImg;
            CreatedDate = episode.CreatedDate;
            Views = episode.Views;   
        }

        public string EpisodeKey { get; set; }
        public int EpisodeNum { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReleasedDate { get; set; }
        public List<ReturnEpisodeVideo> EpisodeVideos { get; set; }
        public string EpisodeImg { get; set; }
        public string SerieKey { get; set; }
        public string SeasonKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Views { get; set; }
        public string PreviousEpisodeKey { get; set; } = null;
        public string NextEpisodeKey { get; set; } = null;
        public bool IsIframe { get; set; } = true;
    }
    public class ReturnEpisodeVideo
    {
        public ReturnEpisodeVideo(string optionName, string videoUrl, bool isIframe = true)
        {
            OptionName = optionName;
            VideoUrl = videoUrl;
            IsIframe = isIframe;
        }
        public string OptionName { get; set; }
        public string VideoUrl { get; set; }
        public bool IsIframe { get; set; }
    }
}
