namespace DnD_Character_Sheet.Models;

public class Combat
{
    public int ArmorClass { get; set; }
    public int Initiative { get; set; }
    public int Speed { get; set; }
    public required HitPoints HitPoints { get; set; }
}
