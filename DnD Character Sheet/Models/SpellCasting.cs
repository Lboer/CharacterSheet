using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models
{
    public class SpellCasting
    {
        [Required] 
        public required string SpellcastingAbility {  get; set; }
        [Required] 
        public int SpellSaveDC { get; set; }
        [Required] 
        public int SpellAttackBonus { get; set;}
        public List<SpellLevel>? Levels { get; set; } 
        public List<Spell>? Spells { get; set; }
        public required List<Spell>? Cantrips { get; set; }
    }
}