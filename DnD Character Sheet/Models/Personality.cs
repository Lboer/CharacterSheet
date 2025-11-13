using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class Personality
{
    [Required]
    public List<string> Traits { get; set; }
    [Required] 
    public List<string> Ideals { get; set; }
    [Required] 
    public List<string> Bonds { get; set; }
    [Required] 
    public List<string> Flaws { get; set; }
}
