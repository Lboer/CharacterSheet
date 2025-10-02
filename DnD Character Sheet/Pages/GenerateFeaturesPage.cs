using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class GenerateFeaturesPage
{
    private static Grid _parentGrid;

    public static View GenerateFeaturesGrid(CharacterSheet character)
    {
        _parentGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Features
                new RowDefinition { Height = GridLength.Auto }, // Languages
                new RowDefinition { Height = GridLength.Auto }, // Proficiencies
                new RowDefinition { Height = GridLength.Auto }  // Edit Button
            },
            Padding = new Thickness(10),
            RowSpacing = 15
        };

        var featuresGrid = GenerateFeaturesSection(character.Features);
        var languageGrid = GenerateLanguageSection(character.Languages);
        var proficiencyGrid = GenerateProficiencySection(character.Proficiencies);
        var editButton = GenerateEditButton(character);

        _parentGrid.Add(featuresGrid, 0, 0);
        _parentGrid.Add(languageGrid, 0, 1);
        _parentGrid.Add(proficiencyGrid, 0, 2);
        _parentGrid.Add(editButton, 0, 3);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _parentGrid
        };
    }

    public static void RefreshFeaturesGrid(CharacterSheet updatedCharacter)
    {
        if (_parentGrid == null)
            return;

        _parentGrid.Children.Clear();

        var featuresGrid = GenerateFeaturesSection(updatedCharacter.Features);
        var languageGrid = GenerateLanguageSection(updatedCharacter.Languages);
        var proficiencyGrid = GenerateProficiencySection(updatedCharacter.Proficiencies);
        var editButton = GenerateEditButton(updatedCharacter);

        _parentGrid.Add(featuresGrid, 0, 0);
        _parentGrid.Add(languageGrid, 0, 1);
        _parentGrid.Add(proficiencyGrid, 0, 2);
        _parentGrid.Add(editButton, 0, 3);
    }

    private static Grid GenerateFeaturesSection(List<Feature> features)
    {
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

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

            var feature = features[i];
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

            grid.Add(label, 0, i + 1);
        }

        return grid;
    }

    private static Grid GenerateLanguageSection(List<string> languages)
    {
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

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

    private static Grid GenerateProficiencySection(List<string> proficiencies)
    {
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

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

    private static Button GenerateEditButton(CharacterSheet character)
    {
        var editButton = new Button
        {
            Text = "Edit Features"
        };

        editButton.Clicked += (s, e) =>
        {
            var featuresPopup = new FeaturesPopup(character);

            featuresPopup.Closed += (sender1, args1) =>
            {
                var languagesPopup = new LanguagesPopup(character);

                languagesPopup.Closed += (sender2, args2) =>
                {
                    RefreshFeaturesGrid(character);
                };

                Application.Current.MainPage.ShowPopup(languagesPopup);
            };

            Application.Current.MainPage.ShowPopup(featuresPopup);
        };

        return editButton;
    }
}
