using System.ComponentModel.DataAnnotations;

namespace Api.MyFlix.Models
{
    public class Category
    {
        public Category () { }

        public Category(string name)
        {
            Name = name;
        }

        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
