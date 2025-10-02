using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class GenerateBackstoryPage
{
    private static Grid _parentGrid;

    public static View GenerateBackstoryGrid(CharacterSheet character)
    {
        _parentGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            Padding = new Thickness(10),
            RowSpacing = 15
        };

        var characterGrid = GenerateCharacterGrid(character);
        var backstoryGrid = GenerateBackgroundGrid(character);
        var notesGrid = GenerateNotesGrid(character);
        var editButton = GenerateEditButton(character);

        _parentGrid.Add(characterGrid, 0, 0);
        _parentGrid.Add(backstoryGrid, 0, 1);
        _parentGrid.Add(notesGrid, 0, 2);
        _parentGrid.Add(editButton, 0, 3);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _parentGrid
        };
    }

    public static void RefreshBackstoryLayout(CharacterSheet updatedCharacter)
    {
        if (_parentGrid == null)
            return;

        _parentGrid.Children.Clear();

        var characterGrid = GenerateCharacterGrid(updatedCharacter);
        var backstoryGrid = GenerateBackgroundGrid(updatedCharacter);
        var notesGrid = GenerateNotesGrid(updatedCharacter);
        var editButton = GenerateEditButton(updatedCharacter);

        _parentGrid.Add(characterGrid, 0, 0);
        _parentGrid.Add(backstoryGrid, 0, 1);
        _parentGrid.Add(notesGrid, 0, 2);
        _parentGrid.Add(editButton, 0, 3);
    }

    private static Grid GenerateCharacterGrid(CharacterSheet character)
    {
        var characterGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        characterGrid.Add(CreateBackstoryFrame("Personality Traits", string.Join(" ", character.Personality.Traits)), 0, 0);
        characterGrid.Add(CreateBackstoryFrame("Ideals", string.Join(" ", character.Personality.Ideals)), 1, 0);
        characterGrid.Add(CreateBackstoryFrame("Bonds", string.Join(" ", character.Personality.Bonds)), 0, 1);
        characterGrid.Add(CreateBackstoryFrame("Flaws", string.Join(" ", character.Personality.Flaws)), 1, 1);

        return characterGrid;
    }

    private static Grid GenerateBackgroundGrid(CharacterSheet character)
    {
        var backstoryGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            },
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

        backstoryGrid.Add(new Label
        {
            Text = "Backstory",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 0);

        backstoryGrid.Add(new Label
        {
            Text = character.Backstory,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 1);

        return backstoryGrid;
    }

    private static Grid GenerateNotesGrid(CharacterSheet character)
    {
        var notesGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            },
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

        notesGrid.Add(new Label
        {
            Text = "Notes",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 0);

        notesGrid.Add(new Label
        {
            Text = character.Notes,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 1);

        return notesGrid;
    }

    private static Button GenerateEditButton(CharacterSheet character)
    {
        var editButton = new Button
        {
            Text = "Edit Backstory/Personality"
        };

        editButton.Clicked += (s, e) =>
        {
            var popup = new BackstoryPopup(character, true);

            popup.Closed += (sender, args) =>
            {
                RefreshBackstoryLayout(character);
            };

            Application.Current.MainPage.ShowPopup(popup);
        };

        return editButton;
    }

    private static Frame CreateBackstoryFrame(string name, string description)
    {
        var innerGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        var nameLabel = new Label
        {
            Text = name,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };
        innerGrid.Add(nameLabel, 0, 0);
        Grid.SetColumnSpan(nameLabel, 3);

        innerGrid.Add(new Label
        {
            Text = description,
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 1);

        return new Frame
        {
            Content = innerGrid,
            BorderColor = Colors.Black,
            CornerRadius = 8,
            Padding = new Thickness(5)
        };
    }
}
