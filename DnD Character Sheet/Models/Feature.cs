using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class Feature
{
    [Required]
    public required string Name { get; set; }
    [Required] 
    public required string Description { get; set; }
}
