namespace DnD_Character_Sheet.Models;

public class CharacterSheet
{
    public required Character Character { get; set; }
    public required AbilityScores AbilityScores { get; set; }
    public int ProficiencyBonus { get; set; }
    public required SavingThrows SavingThrows { get; set; }
    public required Skills Skills { get; set; }
    public required Combat Combat { get; set; }
    public List<Weapon> Weapons { get; set; }
    public List<string> Equipment { get; set; }
    public required List<Feature> Features { get; set; }
    public SpellCasting? SpellCasting { get; set; }
    public Personality Personality { get; set; }
    public string Backstory { get; set; }
    public string Notes { get; set; }
    public List<string> Languages { get; set; }
    public List<string> Proficiencies { get; set; }
}
