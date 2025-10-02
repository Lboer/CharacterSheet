using DnD_Character_Sheet.Models.Enums;

namespace DnD_Character_Sheet.Models;

public class Character
{
    public required string Name { get; set; }
    public Classes Class { get; set; }
    public int Level { get; set; }
    public required string Race { get; set; }
    public required string Background { get; set; }
    public required string Alignment { get; set; }
    public int? ExperiencePoints { get; set; }
}
