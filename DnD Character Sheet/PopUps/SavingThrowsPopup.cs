using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class SavingThrowsPopup : Popup
{
    public CharacterSheet Character { get; private set; }

    // Entry fields for saving values
    private Entry strengthSaveEntry, dexteritySaveEntry, constitutionSaveEntry, intelligenceSaveEntry, wisdomSaveEntry, charismaSaveEntry;

    public SavingThrowsPopup(CharacterSheet character)
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
            Text = "Edit your character's saving throws.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        // Saving Throws Section
        layout.Children.Add(new Label
        {
            Text = "Saving Throws",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            Margin = new Thickness(0, 20, 0, 0)
        });

        strengthSaveEntry = AddLabeledEntry(layout, "Strength Save", character.SavingThrows.Strength);
        dexteritySaveEntry = AddLabeledEntry(layout, "Dexterity Save", character.SavingThrows.Dexterity);
        constitutionSaveEntry = AddLabeledEntry(layout, "Constitution Save", character.SavingThrows.Constitution);
        intelligenceSaveEntry = AddLabeledEntry(layout, "Intelligence Save", character.SavingThrows.Intelligence);
        wisdomSaveEntry = AddLabeledEntry(layout, "Wisdom Save", character.SavingThrows.Wisdom);
        charismaSaveEntry = AddLabeledEntry(layout, "Charisma Save", character.SavingThrows.Charisma);

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
            Character.SavingThrows.Strength = ParseOrDefault(strengthSaveEntry.Text);
            Character.SavingThrows.Dexterity = ParseOrDefault(dexteritySaveEntry.Text);
            Character.SavingThrows.Constitution = ParseOrDefault(constitutionSaveEntry.Text);
            Character.SavingThrows.Intelligence = ParseOrDefault(intelligenceSaveEntry.Text);
            Character.SavingThrows.Wisdom = ParseOrDefault(wisdomSaveEntry.Text);
            Character.SavingThrows.Charisma = ParseOrDefault(charismaSaveEntry.Text);

            Close();
        };

        layout.Children.Add(saveButton);
        Content = layout;
    }

    private static Entry AddLabeledEntry(Layout layout, string label, int value)
    {
        layout.Children.Add(new Label { Text = label, FontAttributes = FontAttributes.Bold });
        var entry = new Entry { Text = value.ToString() };
        layout.Children.Add(entry);
        return entry;
    }

    private static int ParseOrDefault(string text)
    {
        return int.TryParse(text, out var result) ? result : 0;
    }
}