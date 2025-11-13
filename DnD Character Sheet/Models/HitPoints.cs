using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class HitPoints
{
    [Required]
    public int Maximum { get; set; }
    [Required]
    public int Current { get; set; }
    [Required]
    public int Temporary { get; set; }
    [Required]
    public required string HitDice { get; set; }
}