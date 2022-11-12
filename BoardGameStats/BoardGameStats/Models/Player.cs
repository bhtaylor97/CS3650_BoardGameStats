using System.ComponentModel.DataAnnotations;

namespace BoardGameStats.Models
{
    public class Player
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int NumPlays { get; set; }

    }
}
