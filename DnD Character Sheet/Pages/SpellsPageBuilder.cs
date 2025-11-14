using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class SpellsPageBuilder
{
    private static Grid _spellsGrid;

    public static View Build(CharacterSheet character)
    {
        _spellsGrid = CreateGridLayout();
        PopulateGrid(character);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _spellsGrid
        };
    }

    public static void Refresh(CharacterSheet updatedCharacter)
    {
        if (_spellsGrid == null)
            return;

        _spellsGrid.Children.Clear();
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
                new RowDefinition { Height = GridLength.Auto }, // Title
                new RowDefinition { Height = GridLength.Auto }, // Spells
                new RowDefinition { Height = GridLength.Auto }  // Edit Button
            }
        };
    }

    private static void PopulateGrid(CharacterSheet character)
    {
        var titleRow = BuildTitleRow(character);
        var spellsSection = BuildSpellsSection(character);
        var editButton = BuildEditButton(character);

        _spellsGrid.Add(titleRow, 0, 0);
        _spellsGrid.Add(spellsSection, 0, 1);
        _spellsGrid.Add(editButton, 0, 2);
    }

    private static Grid BuildTitleRow(CharacterSheet character)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
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
            Text = $"Spell attack: +{character.SpellCasting?.SpellAttackBonus ?? 0}",
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        }, 1, 0);

        grid.Add(new Label
        {
            Text = $"Spell save DC: {character.SpellCasting?.SpellSaveDC ?? 0}",
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.End,
            Margin = new Thickness(0, 10, 0, 10)
        }, 2, 0);

        return grid;
    }

    private static Grid BuildSpellsSection(CharacterSheet character)
    {
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

        int row = 0;

        if (character.SpellCasting?.Cantrips?.Count > 0)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Add(new Label
            {
                Text = "Cantrips",
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalOptions = LayoutOptions.Center
            }, 0, row++);

            foreach (var cantrip in character.SpellCasting.Cantrips)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Add(BuildSpellRow(cantrip), 0, row++);
            }
        }

        if (character.SpellCasting?.Spells?.Count > 0)
        {
            var groupedSpells = character.SpellCasting.Spells
                .GroupBy(s => s.Level)
                .OrderBy(g => g.Key);

            foreach (var group in groupedSpells)
            {
                var levelData = character.SpellCasting!.Levels[group.Key - 1];

                var slotLabel = new Label
                {
                    Text = $"Level {group.Key} Spells ({levelData!.Current}/{levelData!.Max})",
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
                    var popup = new GenericValueEditPopup($"Level {group.Key} Spell Slots", levelData.Current, newValue =>
                    {
                        levelData.Current = newValue;
                        Refresh(character);
                    });

                    Application.Current.Windows[0].Page.ShowPopup(popup);
                };

                slotLabel.GestureRecognizers.Add(slotTap);

                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Add(slotLabel, 0, row++);

                foreach (var spell in group)
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.Add(BuildSpellRow(spell), 0, row++);
                }
            }
        }

        return grid;
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

            await Application.Current.Windows[0].Page.DisplayAlertAsync(spell.Name, details, "OK");
        };

        grid.GestureRecognizers.Add(tapGesture);

        return grid;
    }

    private static Button BuildEditButton(CharacterSheet character)
    {
        var button = new Button
        {
            Text = "Edit Spells"
        };

        button.Clicked += (s, e) =>
        {
            var startPopup = new SpellcastingStartPopup(character);
            startPopup.Closed += (_, __) =>
            {
                var endPopup = new SpellcastingEndPopup(character);
                endPopup.Closed += (_, __) => Refresh(character);
                Application.Current.Windows[0].Page.ShowPopup(endPopup);
            };

            Application.Current.Windows[0].Page.ShowPopup(startPopup);
        };

        return button;
    }
}