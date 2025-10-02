using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class SkillsStartPopup : Popup
{
    public CharacterSheet Character { get; private set; }

    public SkillsStartPopup(CharacterSheet character)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            WidthRequest = 340
        };

        layout.Children.Add(new Label
        {
            Text = "Skills define your character's proficiency in various areas. Edit their modifiers below.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        layout.Children.Add(CreateSkillRow("Athletics", () => Character.Skills.Athletics, val => Character.Skills.Athletics = val));
        layout.Children.Add(CreateSkillRow("Acrobatics", () => Character.Skills.Acrobatics, val => Character.Skills.Acrobatics = val));
        layout.Children.Add(CreateSkillRow("Sleight Of Hand", () => Character.Skills.SleightOfHand, val => Character.Skills.SleightOfHand = val));
        layout.Children.Add(CreateSkillRow("Stealth", () => Character.Skills.Stealth, val => Character.Skills.Stealth = val));
        layout.Children.Add(CreateSkillRow("Arcana", () => Character.Skills.Arcana, val => Character.Skills.Arcana = val));
        layout.Children.Add(CreateSkillRow("History", () => Character.Skills.History, val => Character.Skills.History = val));
        layout.Children.Add(CreateSkillRow("Investigation", () => Character.Skills.Investigation, val => Character.Skills.Investigation = val));
        layout.Children.Add(CreateSkillRow("Nature", () => Character.Skills.Nature, val => Character.Skills.Nature = val));
        layout.Children.Add(CreateSkillRow("Religion", () => Character.Skills.Religion, val => Character.Skills.Religion = val));

        layout.Children.Add(new Button
        {
            Text = "Save",
            BackgroundColor = Colors.Purple,
            TextColor = Colors.White,
            Margin = new Thickness(0, 10, 0, 0),
            Command = new Command(() => Close())
        });

        Content = new ScrollView { Content = layout };
    }

    private HorizontalStackLayout CreateSkillRow(string skillName, Func<int> getter, Action<int> setter)
    {
        var label = new Label
        {
            Text = skillName,
            VerticalOptions = LayoutOptions.Center,
            FontSize = 14,
            WidthRequest = 160
        };

        var entry = new Entry
        {
            Text = getter().ToString(),
            WidthRequest = 60,
            HorizontalOptions = LayoutOptions.End
        };

        entry.TextChanged += (s, e) =>
        {
            if (int.TryParse(e.NewTextValue, out int result))
                setter(result);
        };

        return new HorizontalStackLayout
        {
            Spacing = 10,
            Children = { label, entry }
        };
    }
}