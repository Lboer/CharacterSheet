using DnD_Character_Sheet.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class Character
{
    [Required] 
    public required string Name { get; set; }
    [Required] 
    public Classes Class { get; set; }
    [Required]
    public int Level { get; set; }
    [Required] 
    public required string Race { get; set; }
    [Required] 
    public required string Background { get; set; }
    [Required] 
    public required string Alignment { get; set; }
    [Required] 
    public int? ExperiencePoints { get; set; }
}
