namespace DnD_Character_Sheet.Models;

public class HitPoints
{
    public int Maximum { get; set; }
    public int Current { get; set; }
    public int Temporary { get; set; }
    public required string HitDice { get; set; }
}