using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class FeaturesPopup : Popup
{
    public CharacterSheet Character { get; private set; }
    private List<VerticalStackLayout> featureRows = new();
    private VerticalStackLayout featureEntryContainer;

    public FeaturesPopup(CharacterSheet character)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            WidthRequest = 340
        };

        // Description
        layout.Children.Add(new Label
        {
            Text = "Features represent special abilities, racial traits, or class powers your character has. Add or edit them below.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        // Section header
        layout.Children.Add(new Label
        {
            Text = "Character Features",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            Margin = new Thickness(0, 10, 0, 0)
        });

        // Container for feature entries
        featureEntryContainer = new VerticalStackLayout { Spacing = 10 };
        layout.Children.Add(featureEntryContainer);

        // Add existing features
        foreach (var feature in character.Features)
        {
            AddFeatureRow(feature.Name, feature.Description);
        }

        // Add Feature button
        var addButton = new Button
        {
            Text = "Add Feature",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };

        addButton.Clicked += (s, e) => AddFeatureRow("", "");

        layout.Children.Add(addButton);

        // Save button
        var saveButton = new Button
        {
            Text = "Save",
            BackgroundColor = Colors.Purple,
            TextColor = Colors.White,
            Margin = new Thickness(0, 10, 0, 0)
        };

        saveButton.Clicked += (s, e) =>
        {
            Character.Features = featureRows
                .Select(row =>
                {
                    var detailsLayout = row.Children.OfType<VerticalStackLayout>().FirstOrDefault();
                    var nameEntry = detailsLayout?.Children.OfType<Entry>().FirstOrDefault();
                    var descEditor = detailsLayout?.Children.OfType<Editor>().FirstOrDefault();

                    return new Feature
                    {
                        Name = nameEntry?.Text?.Trim() ?? "",
                        Description = descEditor?.Text?.Trim() ?? ""
                    };
                })
                .Where(f => !string.IsNullOrWhiteSpace(f.Name))
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

    private void AddFeatureRow(string name, string description)
    {
        var nameEntry = new Entry { Text = name, Placeholder = "Feature Name" };
        var descEditor = new Editor
        {
            Text = description,
            Placeholder = "Feature Description",
            AutoSize = EditorAutoSizeOption.TextChanges,
            HeightRequest = 60
        };

        var removeButton = new Button
        {
            Text = "❌",
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Red,
            WidthRequest = 40
        };

        var toggleButton = new Button
        {
            Text = "🔽",
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Black,
            WidthRequest = 40
        };

        var headerRow = new HorizontalStackLayout
        {
            Spacing = 5,
            Children = { new Label { Text = name, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center }, toggleButton, removeButton }
        };

        var detailsLayout = new VerticalStackLayout
        {
            Spacing = 5,
            Children =
        {
            new Label { Text = "Name", FontAttributes = FontAttributes.Bold },
            nameEntry,
            new Label { Text = "Description", FontAttributes = FontAttributes.Bold },
            descEditor
        }
        };

        var featureBlock = new VerticalStackLayout
        {
            Spacing = 5,
            Children = { headerRow, detailsLayout }
        };

        toggleButton.Clicked += (s, e) =>
        {
            detailsLayout.IsVisible = !detailsLayout.IsVisible;
            toggleButton.Text = detailsLayout.IsVisible ? "🔽" : "▶️";
        };

        removeButton.Clicked += (s, e) =>
        {
            featureEntryContainer.Children.Remove(featureBlock);
            featureRows.Remove(featureBlock);
        };

        featureRows.Add(featureBlock);
        featureEntryContainer.Children.Add(featureBlock);
    }
}