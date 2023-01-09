namespace Api.MyFlix.Models.Object
{
    public class ParamEpisode
    {
        public int EpisodeNum { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReleasedDate { get; set; }
        public List<ParamEpisodeVideo> EpisodeVideos { get; set; }
        public string EpisodeImg { get; set; }
        public bool IsIframe { get; set; } = true;
    }
    public class ParamEpisodeVideo
    {
        public string OptionName { get; set; }
        public string VideoUrl { get; set; }
        public bool IsIframe { get; set; } = true;
    }
}
