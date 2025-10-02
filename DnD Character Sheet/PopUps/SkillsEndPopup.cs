using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class SkillsEndPopup : Popup
{
    public CharacterSheet Character { get; private set; }

    public SkillsEndPopup(CharacterSheet character)
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
            Text = "Continue editing your character's skill modifiers.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        layout.Children.Add(CreateSkillRow("AnimalHandling", () => Character.Skills.AnimalHandling, val => Character.Skills.AnimalHandling = val));
        layout.Children.Add(CreateSkillRow("Insight", () => Character.Skills.Insight, val => Character.Skills.Insight = val));
        layout.Children.Add(CreateSkillRow("Medicine", () => Character.Skills.Medicine, val => Character.Skills.Medicine = val));
        layout.Children.Add(CreateSkillRow("Perception", () => Character.Skills.Perception, val => Character.Skills.Perception = val));
        layout.Children.Add(CreateSkillRow("Survival", () => Character.Skills.Survival, val => Character.Skills.Survival = val));
        layout.Children.Add(CreateSkillRow("Deception", () => Character.Skills.Deception, val => Character.Skills.Deception = val));
        layout.Children.Add(CreateSkillRow("Intimidation", () => Character.Skills.Intimidation, val => Character.Skills.Intimidation = val));
        layout.Children.Add(CreateSkillRow("Performance", () => Character.Skills.Performance, val => Character.Skills.Performance = val));
        layout.Children.Add(CreateSkillRow("Persuasion", () => Character.Skills.Persuasion, val => Character.Skills.Persuasion = val));

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

