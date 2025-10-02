namespace DnD_Character_Sheet.Models;

public class Spell
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Damage { get; set; }
    public string? DamageType { get; set; }
    public int Level { get; set; }
    public required string CastingTime { get; set; }
    public required string Range { get; set; }
    public required string Components { get; set; }
    public required string Duration { get; set; }
    public string? Area { get; set; }
}