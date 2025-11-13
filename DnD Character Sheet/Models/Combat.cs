using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class Combat
{
    [Required] 
    public int ArmorClass { get; set; }
    [Required] 
    public int Initiative { get; set; }
    [Required] 
    public int Speed { get; set; }
    [Required] 
    public required HitPoints HitPoints { get; set; }
}
