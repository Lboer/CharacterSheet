using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class AbilityScores
{
    [Required]
    public int Strength { get; set; }
    [Required] 
    public int Dexterity { get; set; }
    [Required] 
    public int Constitution { get; set; }
    [Required] 
    public int Intelligence { get; set; }
    [Required] 
    public int Wisdom { get; set; }
    [Required] 
    public int Charisma { get; set; }
}