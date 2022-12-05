namespace Api.MyFlix.Models.Object
{
    public class ParamEpisode
    {
        public int EpisodeNum { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> EpisodeVideo { get; set; }
        public string EpisodeImg { get; set; }
        public bool IsIframe { get; set; } = true;
    }
}
