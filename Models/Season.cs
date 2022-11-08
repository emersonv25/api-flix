using Api.MyFlix.Models.Object;
using Api.MyFlix.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.MyFlix.Models
{
    [Index(nameof(SeasonKey))]
    public class Season
    {
        public Season () { }
        public Season (ReturnSeason returnSeason)
        {
            SeasonKey = returnSeason.SeasonKey;
            SeasonNum = returnSeason.SeasonNum;
            Episodes = returnSeason.Episodes.Select(e => new Episode(e)).ToList();
        }
        public Season(ParamSeason paramSeason, string movieKey)
        {
            SeasonKey = $"{movieKey}-{paramSeason.SeasonNum}-temporada-online";
            SeasonNum = paramSeason.SeasonNum;
            Episodes = paramSeason.Episodes.Select(e => new Episode(e, movieKey)).ToList();
        }

        [Key]
        public int SeasonId { get; set; }
        public string SeasonKey { get; set; }
        public int SeasonNum { get; set; }
        public List<Episode> Episodes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

    }
}
