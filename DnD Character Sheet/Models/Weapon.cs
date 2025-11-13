using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class Weapon
{
    [Required] 
    public required string Name { get; set; }
    [Required] 
    public required string AttackBonus { get; set; }
    [Required] 
    public required string Damage { get; set; }
    [Required] 
    public required string DamageType { get; set; }
    public string? Range { get; set; }
    [Required] 
    public List<string> Properties { get; set; }
}
