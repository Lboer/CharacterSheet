namespace DnD_Character_Sheet.Models;

public class Weapon
{
    public required string Name { get; set; }
    public required string AttackBonus { get; set; }
    public required string Damage { get; set; }
    public required string DamageType { get; set; }
    public string? Range { get; set; }
    public List<string> Properties { get; set; }
}
