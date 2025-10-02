using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class BackstoryPopup : Popup
{
    public CharacterSheet Character { get; private set; }

    private List<Entry> traitEntries = new();
    private List<Entry> idealEntries = new();
    private List<Entry> bondEntries = new();
    private List<Entry> flawEntries = new();

    public BackstoryPopup(CharacterSheet character, bool reloadPage = false)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(20),
            Spacing = 10,
            WidthRequest = 320
        };

        layout.Children.Add(new Label
        {
            Text = "Backstory & Personality",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center
        });

        // Traits
        layout.Children.Add(new Label { Text = "Personality Traits", FontAttributes = FontAttributes.Bold });

        var traitsContainer = new VerticalStackLayout { Spacing = 5 };
        layout.Children.Add(traitsContainer);

        foreach (var trait in character.Personality.Traits)
        {
            AddTraitRow(trait);
        }

        var addTraitButton = new Button
        {
            Text = "Add Trait",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };
        addTraitButton.Clicked += (s, e) => AddTraitRow("");
        layout.Children.Add(addTraitButton);

        void AddTraitRow(string text)
        {
            var entry = new Entry { Text = text, Placeholder = "Trait" };
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
                traitsContainer.Children.Remove(row);
                traitEntries.Remove(entry);
            };
            traitEntries.Add(entry);
            traitsContainer.Children.Add(row);
        }

        // Ideals
        layout.Children.Add(new Label { Text = "Ideals", FontAttributes = FontAttributes.Bold });

        var idealsContainer = new VerticalStackLayout { Spacing = 5 };
        layout.Children.Add(idealsContainer);

        foreach (var ideal in character.Personality.Ideals)
        {
            AddIdealRow(ideal);
        }

        var addIdealButton = new Button
        {
            Text = "Add Ideal",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };
        addIdealButton.Clicked += (s, e) => AddIdealRow("");
        layout.Children.Add(addIdealButton);

        void AddIdealRow(string text)
        {
            var entry = new Entry { Text = text, Placeholder = "Ideal" };
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
                idealsContainer.Children.Remove(row);
                idealEntries.Remove(entry);
            }; 
            idealEntries.Add(entry);
            idealsContainer.Children.Add(row);
        }

        // Bonds
        layout.Children.Add(new Label { Text = "Bonds", FontAttributes = FontAttributes.Bold });

        var bondsContainer = new VerticalStackLayout { Spacing = 5 };
        layout.Children.Add(bondsContainer);

        foreach (var bond in character.Personality.Bonds)
        {
            AddBondRow(bond);
        }

        var addBondButton = new Button
        {
            Text = "Add Bond",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };
        addBondButton.Clicked += (s, e) => AddBondRow("");
        layout.Children.Add(addBondButton);

        void AddBondRow(string text)
        {
            var entry = new Entry { Text = text, Placeholder = "Bond" };
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
                bondsContainer.Children.Remove(row);
                bondEntries.Remove(entry);
            };
            bondEntries.Add(entry);
            bondsContainer.Children.Add(row);
        }

        // Flaws
        layout.Children.Add(new Label { Text = "Flaws", FontAttributes = FontAttributes.Bold });

        var flawsContainer = new VerticalStackLayout { Spacing = 5 };
        layout.Children.Add(flawsContainer);

        foreach (var flaw in character.Personality.Flaws)
        {
            AddFlawRow(flaw);
        }

        var addFlawButton = new Button
        {
            Text = "Add Flaw",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };
        addFlawButton.Clicked += (s, e) => AddFlawRow("");
        layout.Children.Add(addFlawButton);

        void AddFlawRow(string text)
        {
            var entry = new Entry { Text = text, Placeholder = "Flaw" };
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
                flawsContainer.Children.Remove(row);
                flawEntries.Remove(entry);
            };
            flawEntries.Add(entry);
            flawsContainer.Children.Add(row);
        }


        layout.Children.Add(new Label { Text = "Backstory", FontAttributes = FontAttributes.Bold });
        var backstoryEditor = new Editor
        {
            Text = character.Backstory,
            Placeholder = "Write your character's backstory...",
            AutoSize = EditorAutoSizeOption.TextChanges,
            HeightRequest = 100
        };
        layout.Children.Add(backstoryEditor);

        layout.Children.Add(new Label { Text = "Notes", FontAttributes = FontAttributes.Bold });
        var notesEditor = new Editor
        {
            Text = character.Notes,
            Placeholder = "Additional notes...",
            AutoSize = EditorAutoSizeOption.TextChanges,
            HeightRequest = 80
        };
        layout.Children.Add(notesEditor);

        var saveButton = new Button
        {
            Text = "Save",
            BackgroundColor = Colors.Purple,
            TextColor = Colors.White
        };

        saveButton.Clicked += (s, e) =>
        {
            character.Notes = notesEditor.Text;
            character.Backstory = backstoryEditor.Text;
            character.Personality.Flaws = flawEntries
                .Select(entry => entry.Text.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .ToList();
            character.Personality.Ideals = idealEntries
                .Select(entry => entry.Text.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .ToList();
            character.Personality.Bonds = bondEntries
                .Select(entry => entry.Text.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .ToList();
            character.Personality.Traits = traitEntries
                .Select(entry => entry.Text.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
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
}

