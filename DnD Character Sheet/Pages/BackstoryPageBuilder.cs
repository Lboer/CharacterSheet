using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;
using Microsoft.Maui.Controls.Shapes;

namespace DnD_Character_Sheet.Pages;

public static class BackstoryPageBuilder
{
    private static Grid _backstoryGrid;

    public static View Build(CharacterSheet character)
    {
        _backstoryGrid = CreateGridLayout();
        PopulateGrid(character);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _backstoryGrid
        };
    }

    public static void Refresh(CharacterSheet updatedCharacter)
    {
        if (_backstoryGrid == null)
            return;

        _backstoryGrid.Children.Clear();
        PopulateGrid(updatedCharacter);
    }

    private static Grid CreateGridLayout()
    {
        return new Grid
        {
            Padding = new Thickness(10),
            RowSpacing = 15,
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Personality
                new RowDefinition { Height = GridLength.Auto }, // Backstory
                new RowDefinition { Height = GridLength.Auto }, // Notes
                new RowDefinition { Height = GridLength.Auto }  // Edit Button
            }
        };
    }

    private static void PopulateGrid(CharacterSheet character)
    {
        var personalitySection = BuildPersonalitySection(character);
        var backstorySection = BuildTextSection("Backstory", character.Backstory);
        var notesSection = BuildTextSection("Notes", character.Notes);
        var editButton = BuildEditButton(character);

        _backstoryGrid.Add(personalitySection, 0, 0);
        _backstoryGrid.Add(backstorySection, 0, 1);
        _backstoryGrid.Add(notesSection, 0, 2);
        _backstoryGrid.Add(editButton, 0, 3);
    }

    private static Grid BuildPersonalitySection(CharacterSheet character)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            }
        };

        grid.Add(BuildFramedText("Personality Traits", character.Personality.Traits), 0, 0);
        grid.Add(BuildFramedText("Ideals", character.Personality.Ideals), 1, 0);
        grid.Add(BuildFramedText("Bonds", character.Personality.Bonds), 0, 1);
        grid.Add(BuildFramedText("Flaws", character.Personality.Flaws), 1, 1);

        return grid;
    }

    private static Grid BuildTextSection(string title, string content)
    {
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            }
        };

        grid.Add(new Label
        {
            Text = title,
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 0);

        grid.Add(new Label
        {
            Text = content,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 1);

        return grid;
    }

    private static Border BuildFramedText(string title, List<string> items)
    {
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            }
        };

        grid.Add(new Label
        {
            Text = title,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 0);

        grid.Add(new Label
        {
            Text = string.Join(" ", items),
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 1);

        return new Border
        {
            Content = grid,
            Stroke = Colors.Black,
            StrokeThickness = 2,
            Padding = new Thickness(5),
            StrokeShape = new RoundRectangle { CornerRadius = 8 }
		};
    }

    private static Button BuildEditButton(CharacterSheet character)
    {
        var button = new Button
        {
            Text = "Edit Backstory/Personality"
        };

        button.Clicked += (s, e) =>
        {
            var popup = new BackstoryPopup(character);
            popup.Closed += (_, __) => Refresh(character);
            Application.Current.Windows[0].Page.ShowPopup(popup);
        };

        return button;
    }
}

