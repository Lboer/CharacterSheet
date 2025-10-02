namespace DnD_Character_Sheet.Factories;

public class ViewFactoryWrapper
{
    public required Func<View> CreateView { get; set; }
}
