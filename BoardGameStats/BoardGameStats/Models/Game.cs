using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameStats.Models
{
    public class Game
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey("BoardGameId")]
        public int BoardGameId { get; set; }

        [Required]
        [ForeignKey("PlayerId")]
        public int PlayerId { get; set; }

        [Required]
        public bool Won { get; set; }

        public decimal Score { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Date")]
        public DateTime DateOfPlay { get; set; }
    }
}
