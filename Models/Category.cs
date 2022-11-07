using System.ComponentModel.DataAnnotations;

namespace MyFlix.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Movie>? Movies { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
