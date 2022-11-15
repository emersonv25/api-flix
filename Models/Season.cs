using Api.MyFlix.Models.Object;
using Microsoft.EntityFrameworkCore;
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
        public Season(ParamSeason paramSeason, string SerieKey)
        {
            SeasonKey = $"{SerieKey}-temporada-{paramSeason.SeasonNum}";
            SeasonNum = paramSeason.SeasonNum == 0 ? 1 : paramSeason.SeasonNum;
            Episodes = paramSeason.Episodes.Select(e => new Episode(e, SeasonNum, SerieKey)).ToList();
        }

        [Key]
        public int SeasonId { get; set; }
        public string SeasonKey { get; set; }
        public int SeasonNum { get; set; } = 1;
        public List<Episode> Episodes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("Serie")]
        public int SerieId { get; set; }
        public Serie Serie { get; set; }

    }
}
