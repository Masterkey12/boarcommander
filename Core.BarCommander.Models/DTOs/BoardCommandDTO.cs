using System.ComponentModel.DataAnnotations;

namespace BarCommander.Core.Models.DTOs
{
    public class BoardCommandDTO
    {
        [Required]
        public string Color { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public int Position { get; set; }
    }
}