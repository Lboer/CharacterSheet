using System.ComponentModel.DataAnnotations;

namespace DnD_Character_Sheet.Models;

public class Skills
{
    [Required] 
    public int Acrobatics { get; set; }
    [Required] 
    public int AnimalHandling { get; set; }
    [Required] 
    public int Arcana { get; set; }
    [Required] 
    public int Athletics { get; set; }
    [Required] 
    public int Deception { get; set; }
    [Required] 
    public int History { get; set; }
    [Required] 
    public int Insight { get; set; }
    [Required] 
    public int Intimidation { get; set; }
    [Required] 
    public int Investigation { get; set; }
    [Required] 
    public int Medicine { get; set; }
    [Required] 
    public int Nature { get; set; }
    [Required] 
    public int Perception { get; set; }
    [Required] 
    public int Performance { get; set; }
    [Required] 
    public int Persuasion { get; set; }
    [Required] 
    public int Religion { get; set; }
    [Required] 
    public int SleightOfHand { get; set; }
    [Required] 
    public int Stealth { get; set; }
    [Required] 
    public int Survival { get; set; }
}
