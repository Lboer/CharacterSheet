using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models
{
    public class SpellLevel
    {
        [Required]
        [Range (0,9)]
        public int Level { get; set; }
        [Required] 
        public int Max {  get; set; }
        [Required] 
        public int Current { get; set; }
    }
}
