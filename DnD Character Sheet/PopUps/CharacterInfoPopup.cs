using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.Models.Enums;

namespace DnD_Character_Sheet.PopUps;

public class CharacterInfoPopup : Popup
{
    public CharacterSheet Character { get; private set; }

    public CharacterInfoPopup(CharacterSheet character)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(20),
            Spacing = 10,
            WidthRequest = 300 // Wider popup
        };

        layout.Children.Add(new Label { Text = "Name", FontAttributes = FontAttributes.Bold });
        var nameEntry = new Entry { Text = character.Character.Name };
        layout.Children.Add(nameEntry);

        layout.Children.Add(new Label { Text = "Class", FontAttributes = FontAttributes.Bold });
        var classPicker = new Picker();
        foreach (var value in Enum.GetValues(typeof(Classes)))
        {
            classPicker.Items.Add(value.ToString());
        }
        classPicker.SelectedIndex = (int)character.Character.Class;
        layout.Children.Add(classPicker);

        layout.Children.Add(new Label { Text = "Race", FontAttributes = FontAttributes.Bold });
        var raceEntry = new Entry { Text = character.Character.Race };
        layout.Children.Add(raceEntry);

        layout.Children.Add(new Label { Text = "Background", FontAttributes = FontAttributes.Bold });
        var backgroundEntry = new Entry { Text = character.Character.Background };
        layout.Children.Add(backgroundEntry);

        layout.Children.Add(new Label { Text = "Alignment", FontAttributes = FontAttributes.Bold });
        var alignmentEntry = new Entry { Text = character.Character.Alignment };
        layout.Children.Add(alignmentEntry);

        layout.Children.Add(new Label { Text = "Level", FontAttributes = FontAttributes.Bold });
        var levelEntry = new Entry { Text = character.Character.Level.ToString(), Keyboard = Keyboard.Numeric };
        layout.Children.Add(levelEntry);

        layout.Children.Add(new Label { Text = "Proficiency Bonus", FontAttributes = FontAttributes.Bold });
        var proficiencyEntry = new Entry { Text = character.ProficiencyBonus.ToString(), Keyboard = Keyboard.Numeric };
        layout.Children.Add(proficiencyEntry);

        layout.Children.Add(new Label { Text = "Experience Points", FontAttributes = FontAttributes.Bold });
        var xpEntry = new Entry { Text = character.Character.ExperiencePoints.ToString(), Keyboard = Keyboard.Numeric };
        layout.Children.Add(xpEntry);

        var saveButton = new Button { Text = "Save", BackgroundColor = Colors.Purple, TextColor = Colors.White };
        saveButton.Clicked += (s, e) =>
        {
            character.Character.Name = nameEntry.Text;
            character.Character.Race = raceEntry.Text;
            character.Character.Background = backgroundEntry.Text;
            character.Character.Alignment = alignmentEntry.Text;
            character.Character.Level = int.TryParse(levelEntry.Text, out var lvl) ? lvl : 1;
            character.ProficiencyBonus = int.TryParse(proficiencyEntry.Text, out var prof) ? prof : 2;
            character.Character.ExperiencePoints = int.TryParse(xpEntry.Text, out var xp) ? xp : 0;
            character.Character.Class = (Classes)classPicker.SelectedIndex;

            Close(); // Close the popup
        };

        layout.Children.Add(saveButton);
        Content = layout;
    }
}