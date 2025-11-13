using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class Spell
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
    public string? Damage { get; set; }
    public string? DamageType { get; set; }
    [Required]
    public int Level { get; set; }
    [Required]
    public required string CastingTime { get; set; }
    [Required]
    public required string Range { get; set; }
    [Required] 
    public required string Components { get; set; }
    [Required] 
    public required string Duration { get; set; }
    public string? Area { get; set; }
}