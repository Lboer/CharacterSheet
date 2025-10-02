using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class LanguagesPopup : Popup
{
    public CharacterSheet Character { get; private set; }
    private List<HorizontalStackLayout> languageRows = new();
    private List<HorizontalStackLayout> proficiencyRows = new();
    private VerticalStackLayout languageEntryContainer;
    private VerticalStackLayout proficiencyEntryContainer;

    public LanguagesPopup(CharacterSheet character)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            WidthRequest = 320
        };

        // Description
        layout.Children.Add(new Label
        {
            Text = "Languages determine what your character can understand and speak. Proficiencies reflect specialized skills or training.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        // Languages Section
        layout.Children.Add(new Label
        {
            Text = "Known Languages",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            Margin = new Thickness(0, 10, 0, 0)
        });

        languageEntryContainer = new VerticalStackLayout { Spacing = 5 };
        layout.Children.Add(languageEntryContainer);

        foreach (var lang in character.Languages)
        {
            AddLanguageRow(lang);
        }

        var addLanguageButton = new Button
        {
            Text = "Add Language",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };
        addLanguageButton.Clicked += (s, e) => AddLanguageRow("");
        layout.Children.Add(addLanguageButton);

        // Proficiencies Section
        layout.Children.Add(new Label
        {
            Text = "Proficiencies",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            Margin = new Thickness(0, 20, 0, 0)
        });

        proficiencyEntryContainer = new VerticalStackLayout { Spacing = 5 };
        layout.Children.Add(proficiencyEntryContainer);

        foreach (var prof in character.Proficiencies)
        {
            AddProficiencyRow(prof);
        }

        var addProficiencyButton = new Button
        {
            Text = "Add Proficiency",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };
        addProficiencyButton.Clicked += (s, e) => AddProficiencyRow("");
        layout.Children.Add(addProficiencyButton);

        // Save Button
        var saveButton = new Button
        {
            Text = "Save",
            BackgroundColor = Colors.Purple,
            TextColor = Colors.White,
            Margin = new Thickness(0, 10, 0, 0)
        };

        saveButton.Clicked += (s, e) =>
        {
            Character.Languages = languageRows
                .Select(row => row.Children.OfType<Entry>().FirstOrDefault()?.Text?.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .Distinct()
                .ToList();

            Character.Proficiencies = proficiencyRows
                .Select(row => row.Children.OfType<Entry>().FirstOrDefault()?.Text?.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .Distinct()
                .ToList();

            Close();
        };

        layout.Children.Add(saveButton);

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Children = { layout }
            }
        };
    }

    private void AddLanguageRow(string initialText)
    {
        var entry = new Entry { Text = initialText, Placeholder = "Language" };
        var removeButton = new Button
        {
            Text = "❌",
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Red,
            WidthRequest = 40
        };

        var row = new HorizontalStackLayout
        {
            Spacing = 5,
            Children = { entry, removeButton }
        };

        removeButton.Clicked += (s, e) =>
        {
            languageEntryContainer.Children.Remove(row);
            languageRows.Remove(row);
        };

        languageRows.Add(row);
        languageEntryContainer.Children.Add(row);
    }

    private void AddProficiencyRow(string initialText)
    {
        var entry = new Entry { Text = initialText, Placeholder = "Proficiency" };
        var removeButton = new Button
        {
            Text = "❌",
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Red,
            WidthRequest = 40
        };

        var row = new HorizontalStackLayout
        {
            Spacing = 5,
            Children = { entry, removeButton }
        };

        removeButton.Clicked += (s, e) =>
        {
            proficiencyEntryContainer.Children.Remove(row);
            proficiencyRows.Remove(row);
        };

        proficiencyRows.Add(row);
        proficiencyEntryContainer.Children.Add(row);
    }
}
