using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class AbilityScoresPopup : Popup
{
    public CharacterSheet Character { get; private set; }

    // Entry fields for saving values
    private Entry strengthEntry, dexterityEntry, constitutionEntry, intelligenceEntry, wisdomEntry, charismaEntry;

    public AbilityScoresPopup(CharacterSheet character)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15,
            WidthRequest = 320
        };

        // Description
        layout.Children.Add(new Label
        {
            Text = "Edit your character's core stats. These affect combat, skill checks, and spellcasting.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        // Ability Scores Section
        layout.Children.Add(new Label
        {
            Text = "Ability Scores",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            Margin = new Thickness(0, 10, 0, 0)
        });

        strengthEntry = AddLabeledEntry(layout, "Strength", character.AbilityScores.Strength);
        dexterityEntry = AddLabeledEntry(layout, "Dexterity", character.AbilityScores.Dexterity);
        constitutionEntry = AddLabeledEntry(layout, "Constitution", character.AbilityScores.Constitution);
        intelligenceEntry = AddLabeledEntry(layout, "Intelligence", character.AbilityScores.Intelligence);
        wisdomEntry = AddLabeledEntry(layout, "Wisdom", character.AbilityScores.Wisdom);
        charismaEntry = AddLabeledEntry(layout, "Charisma", character.AbilityScores.Charisma);

        // Save Button
        var saveButton = new Button
        {
            Text = "Save",
            BackgroundColor = Colors.Purple,
            TextColor = Colors.White,
            Margin = new Thickness(0, 20, 0, 0)
        };

        saveButton.Clicked += (s, e) =>
        {
            Character.AbilityScores.Strength = ParseOrDefault(strengthEntry.Text);
            Character.AbilityScores.Dexterity = ParseOrDefault(dexterityEntry.Text);
            Character.AbilityScores.Constitution = ParseOrDefault(constitutionEntry.Text);
            Character.AbilityScores.Intelligence = ParseOrDefault(intelligenceEntry.Text);
            Character.AbilityScores.Wisdom = ParseOrDefault(wisdomEntry.Text);
            Character.AbilityScores.Charisma = ParseOrDefault(charismaEntry.Text);

            Close();
        };

        layout.Children.Add(saveButton);
        Content = layout;
    }

    private static Entry AddLabeledEntry(Layout layout, string label, int value)
    {
        layout.Children.Add(new Label { Text = label, FontAttributes = FontAttributes.Bold });
        var entry = new Entry { Text = value.ToString(),  };
        layout.Children.Add(entry);
        return entry;
    }

    private static int ParseOrDefault(string text)
    {
        return int.TryParse(text, out var result) ? result : 0;
    }
}