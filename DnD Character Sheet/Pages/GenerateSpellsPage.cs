using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class GenerateSpellsPage
{
    private static Grid _parentGrid;

    public static View GenerateSpellsGrid(CharacterSheet character)
    {
        _parentGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto } // Edit Button
            },
            Padding = new Thickness(10),
            RowSpacing = 15
        };

        var titleGrid = GenerateTitleGrid(character);
        var spellsGrid = GenerateSpellsSection(character);
        var editButton = GenerateEditButton(character);

        _parentGrid.Add(titleGrid, 0, 0);
        _parentGrid.Add(spellsGrid, 0, 1);
        _parentGrid.Add(editButton, 0, 2);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _parentGrid
        };
    }

    public static void RefreshSpellsGrid(CharacterSheet updatedCharacter)
    {
        if (_parentGrid == null)
            return;

        _parentGrid.Children.Clear();

        var titleGrid = GenerateTitleGrid(updatedCharacter);
        var spellsGrid = GenerateSpellsSection(updatedCharacter);
        var editButton = GenerateEditButton(updatedCharacter);

        _parentGrid.Add(titleGrid, 0, 0);
        _parentGrid.Add(spellsGrid, 0, 1);
        _parentGrid.Add(editButton, 0, 2);
    }

    private static Grid GenerateTitleGrid(CharacterSheet character)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
            },
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } }
        };

        grid.Add(new Label
        {
            Text = "Spells",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 10, 0, 10)
        }, 0, 0);

        grid.Add(new Label
        {
            Text = $"Spell attack: +{character.SpellCasting.SpellAttackBonus}",
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        }, 1, 0);

        grid.Add(new Label
        {
            Text = $"Spell save DC: {character.SpellCasting.SpellSaveDC}",
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.End,
            Margin = new Thickness(0, 10, 0, 10)
        }, 2, 0);

        return grid;
    }

    private static Grid GenerateSpellsSection(CharacterSheet character)
    {
        int currentRow = 0;
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

        if (character.SpellCasting.Cantrips?.Count > 0)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Add(new Label
            {
                Text = "Cantrips",
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalOptions = LayoutOptions.Center
            }, 0, currentRow++);

            foreach (var cantrip in character.SpellCasting.Cantrips)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Add(BuildSpellRow(cantrip), 0, currentRow++);
            }
        }

        if (character.SpellCasting.Spells?.Count > 0)
        {
            var groupedSpells = character.SpellCasting.Spells
                .GroupBy(s => s.Level)
                .OrderBy(g => g.Key);

            foreach (var group in groupedSpells)
            {
                var spellLevel = character.SpellCasting.Levels[group.Key - 1];

                var slotLabel = new Label
                {
                    Text = $"Level {group.Key} Spells ({spellLevel.Current}/{spellLevel.Max})",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    Margin = new Thickness(0, 5, 0, 5),
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.Orange,
                    TextDecorations = TextDecorations.Underline
                };

                var slotTap = new TapGestureRecognizer();
                slotTap.Tapped += (s, e) =>
                {
                    var popup = new GenericValueEditPopup($"Level {group.Key} Spell Slots", spellLevel.Current, newValue =>
                    {
                        spellLevel.Current = newValue;
                        GenerateSpellsPage.RefreshSpellsGrid(character);
                    });

                    Application.Current.MainPage.ShowPopup(popup);
                };

                slotLabel.GestureRecognizers.Add(slotTap);

                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Add(slotLabel, 0, currentRow++);

                foreach (var spell in group)
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.Add(BuildSpellRow(spell), 0, currentRow++);
                }
            }

        }

        return grid;
    }

    private static Button GenerateEditButton(CharacterSheet character)
    {
        var editButton = new Button
        {
            Text = "Edit Spells"
        };

        editButton.Clicked += (s, e) =>
        {
            var startPopup = new SpellcastingStartPopup(character);

            startPopup.Closed += (sender1, args1) =>
            {
                var endPopup = new SpellcastingEndPopup(character);

                endPopup.Closed += (sender2, args2) =>
                {
                    RefreshSpellsGrid(character);
                };

                Application.Current.MainPage.ShowPopup(endPopup);
            };

            Application.Current.MainPage.ShowPopup(startPopup);
        };

        return editButton;
    }

    private static Grid BuildSpellRow(Spell spell)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            Margin = new Thickness(5, 2)
        };

        var nameLabel = new Label
        {
            Text = spell.Name,
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Center
        };

        var rangeLabel = new Label
        {
            Text = $"Range: {spell.Range}",
            FontSize = 16,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        var castingLabel = new Label
        {
            Text = $"Cast: {spell.CastingTime}",
            FontSize = 14,
            HorizontalTextAlignment = TextAlignment.End,
            VerticalTextAlignment = TextAlignment.Center
        };

        grid.Add(nameLabel, 0, 0);
        grid.Add(rangeLabel, 1, 0);
        grid.Add(castingLabel, 2, 0);

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            var details = $"{spell.Description}\n\n" +
                          $"Components: {spell.Components}\n" +
                          $"Duration: {spell.Duration}";

            if (!string.IsNullOrEmpty(spell.Damage))
                details += $"\nDamage: {spell.Damage}";

            if (!string.IsNullOrEmpty(spell.DamageType))
                details += $"\nDamage Type: {spell.DamageType}";

            if (!string.IsNullOrEmpty(spell.Area))
                details += $"\nArea: {spell.Area}";

            await Application.Current.MainPage.DisplayAlert(spell.Name, details, "OK");
        };

        grid.GestureRecognizers.Add(tapGesture);

        return grid;
    }
}
