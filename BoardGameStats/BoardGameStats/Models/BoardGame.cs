using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameStats.Models
{
    public class BoardGame
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Can't play a game a negative number of times.")]
        [Display(Name = "Number of plays:")]
        public int NumPlays { get; set; }
    }
}
