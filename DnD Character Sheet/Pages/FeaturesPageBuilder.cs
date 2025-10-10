using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class FeaturesPageBuilder
{
    private static Grid _featuresGrid;

    public static View Build(CharacterSheet character)
    {
        _featuresGrid = CreateGridLayout();
        PopulateGrid(character);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _featuresGrid
        };
    }

    public static void Refresh(CharacterSheet updatedCharacter)
    {
        if (_featuresGrid == null)
            return;

        _featuresGrid.Children.Clear();
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
                new RowDefinition { Height = GridLength.Auto }, // Features
                new RowDefinition { Height = GridLength.Auto }, // Languages
                new RowDefinition { Height = GridLength.Auto }, // Proficiencies
                new RowDefinition { Height = GridLength.Auto }  // Edit Button
            }
        };
    }

    private static void PopulateGrid(CharacterSheet character)
    {
        var featuresSection = BuildFeatureSection(character.Features);
        var languagesSection = BuildLanguageSection(character.Languages);
        var proficienciesSection = BuildProficiencySection(character.Proficiencies);
        var editButton = BuildEditButton(character);

        _featuresGrid.Add(featuresSection, 0, 0);
        _featuresGrid.Add(languagesSection, 0, 1);
        _featuresGrid.Add(proficienciesSection, 0, 2);
        _featuresGrid.Add(editButton, 0, 3);
    }

    private static Grid BuildFeatureSection(List<Feature> features)
    {
        var grid = CreateSingleColumnGrid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        grid.Add(new Label
        {
            Text = "Features",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        }, 0, 0);

        for (int i = 0; i < features.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var feature = features[i]; // Capture the current feature

            var label = new Label
            {
                Text = feature.Name,
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await Application.Current.MainPage.DisplayAlert(feature.Name, feature.Description, "OK");
            };
            label.GestureRecognizers.Add(tapGesture);

            // first element is Features header
            grid.Add(label, 0, i + 1);
        }

        return grid;
    }

    private static Grid BuildLanguageSection(List<string> languages)
    {
        var grid = CreateSingleColumnGrid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        grid.Add(new Label
        {
            Text = "Languages",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        }, 0, 0);

        for (int i = 0; i < languages.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Add(new Label
            {
                Text = languages[i],
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center
            }, 0, i + 1);
        }

        return grid;
    }

    private static Grid BuildProficiencySection(List<string> proficiencies)
    {
        var grid = CreateSingleColumnGrid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        grid.Add(new Label
        {
            Text = "Proficiencies",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        }, 0, 0);

        for (int i = 0; i < proficiencies.Count; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Add(new Label
            {
                Text = proficiencies[i],
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center
            }, 0, i + 1);
        }

        return grid;
    }

    private static Grid CreateSingleColumnGrid()
    {
        return new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };
    }

    private static Button BuildEditButton(CharacterSheet character)
    {
        var button = new Button
        {
            Text = "Edit Features"
        };

        button.Clicked += (s, e) =>
        {
            var featuresPopup = new FeaturesPopup(character);
            featuresPopup.Closed += (_, __) =>
            {
                var languagesPopup = new LanguagesPopup(character);
                languagesPopup.Closed += (_, __) => Refresh(character);
                Application.Current?.MainPage?.ShowPopup(languagesPopup);
            };

            Application.Current?.MainPage?.ShowPopup(featuresPopup);
        };

        return button;
    }
}

