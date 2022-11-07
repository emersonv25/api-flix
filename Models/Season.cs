using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFlix.Models
{
    public class Season
    {
        [Key]
        public int SeasonId { get; set; } 
        public int Number { get; set; }
        public List<Episode> Episodes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("Movie")]
        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }

    }
}
