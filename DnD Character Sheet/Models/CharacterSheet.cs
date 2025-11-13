using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class CharacterSheet
{
    [Required]
    public required Character Character { get; set; }
    [Required]
    public required AbilityScores AbilityScores { get; set; }
    [Required]
    public int ProficiencyBonus { get; set; }
    [Required]
    public required SavingThrows SavingThrows { get; set; }
    [Required]
    public required Skills Skills { get; set; }
    [Required]
    public required Combat Combat { get; set; }
    public List<Weapon> Weapons { get; set; }
    public List<string> Equipment { get; set; }
    [Required]
    public required List<Feature> Features { get; set; }
    public SpellCasting? SpellCasting { get; set; }
    [Required]
    public Personality Personality { get; set; }
    [Required]
    public string Backstory { get; set; }
    public string? Notes { get; set; }
    [Required]
    public List<string> Languages { get; set; }
    [Required]
    public List<string> Proficiencies { get; set; }
}
