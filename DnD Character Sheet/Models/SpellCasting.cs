namespace DnD_Character_Sheet.Models
{
    public class SpellCasting
    {
        public required string SpellcastingAbility {  get; set; }
        public int SpellSaveDC { get; set; }
        public int SpellAttackBonus { get; set;}
        public List<SpellLevel>? Levels { get; set; } 
        public List<Spell>? Spells { get; set; }
        public required List<Spell> Cantrips { get; set; }
    }
}