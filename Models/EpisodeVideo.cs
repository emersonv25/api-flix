using Api.MyFlix.Models.Object;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.MyFlix.Models
{
    public class EpisodeVideo
    {

        public EpisodeVideo(){}
        public EpisodeVideo(string optionName, string videoUrl, bool isIframe = true) 
        {
            OptionName = optionName;
            VideoUrl = videoUrl;
            IsIframe = isIframe;
        }
        [Key]
        public int VideoId { get; set; }
        public string OptionName { get; set; }
        public string VideoUrl { get; set; }
        public bool IsIframe { get; set; } = true;

        [ForeignKey("Episode")]
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
    }
}
