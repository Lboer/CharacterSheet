using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;
using Microsoft.Maui.Controls.Shapes;

namespace DnD_Character_Sheet.Pages;

public class HomePageBuilder
{
    private static Grid _homeGrid;

    public static View Build(CharacterSheet character)
    {
        _homeGrid = CreateHomeGrid();
        PopulateHomeGrid(character);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _homeGrid
        };
    }

    public static void Refresh(CharacterSheet updatedCharacter)
    {
        if (_homeGrid == null)
            return;

        _homeGrid.Children.Clear();
        PopulateHomeGrid(updatedCharacter);
    }

    private static Grid CreateHomeGrid()
    {
        return new Grid
        {
            Padding = new Thickness(10),
            RowSpacing = 15,
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto } // Edit Button
            },
        };
    }

    private static void PopulateHomeGrid(CharacterSheet character)
    {

        var identityGrid = BuildIdentitySection(character);
        var statsGrid = BuildStatsSection(character);
        var abilitiesGrid = BuildAbilitySection(character);
        var editButton = CreateEditCharacterButton(character);

        _homeGrid.Add(identityGrid, 0, 0);
        _homeGrid.Add(statsGrid, 0, 1);
        _homeGrid.Add(abilitiesGrid, 0, 2);
        _homeGrid.Add(editButton, 0, 3);
    }

    private static Grid BuildIdentitySection(CharacterSheet character)
    {
        var identityGrid = new Grid
        {
            RowDefinitions =
        {
            new RowDefinition { Height = GridLength.Auto },
            new RowDefinition { Height = GridLength.Auto }
        },
            ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star }
        }
        };

        // Top row: Name, Race, Class + Level
        identityGrid.Add(CreateBoldLabel(character.Character.Name), 0, 0);
        identityGrid.Add(CreateBoldLabel(character.Character.Race), 1, 0);
        identityGrid.Add(CreateBoldLabel($"{character.Character.Class} {character.Character.Level}"), 2, 0);

        // Bottom row: Background, Alignment, XP (interactive)
        identityGrid.Add(CreateCenteredLabel(character.Character.Background), 0, 1);
        identityGrid.Add(CreateCenteredLabel(character.Character.Alignment), 1, 1);

        var xpText = character.Character.ExperiencePoints?.ToString() ?? "0";
        var xpLabel = CreateInteractiveLabel("Experience Points", xpText, character.Character.ExperiencePoints ?? 0, newValue =>
        {
            character.Character.ExperiencePoints = newValue;
            Refresh(character);
        });
        identityGrid.Add(xpLabel, 2, 1);

        return identityGrid;
    }

    private static Grid BuildStatsSection(CharacterSheet character)
    {
        var statsGrid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star }
        },
            RowSpacing = 2,
            ColumnSpacing = 2
        };

        // AC
        statsGrid.Add(CreateStatFrame("AC", character.Combat.ArmorClass.ToString()), 0, 0);

        // HP (interactive)
        var hpText = $"{character.Combat.HitPoints.Current}/{character.Combat.HitPoints.Maximum}";
        var hpBorder = CreateInteractiveStatFrame("HP", hpText, "Current HP", character.Combat.HitPoints.Current.ToString(), newValue =>
        {
            character.Combat.HitPoints.Current = int.Parse(newValue);
            Refresh(character);
        });
        statsGrid.Add(hpBorder, 1, 0);

        // Temp HP (interactive)
        var tempHpBorder = CreateInteractiveStatFrame("Temp HP", character.Combat.HitPoints.Temporary.ToString(), "Temporary HP", character.Combat.HitPoints.Temporary.ToString(), newValue =>
        {
            character.Combat.HitPoints.Temporary = int.Parse(newValue);
            Refresh(character);
        });
        statsGrid.Add(tempHpBorder, 2, 0);

        // Initiative
        statsGrid.Add(CreateStatFrame("Initiative", character.Combat.Initiative.ToString()), 0, 1);

        // Speed
        statsGrid.Add(CreateStatFrame("Speed", character.Combat.Speed.ToString()), 1, 1);

        // Hit Dice (interactive)
        var hitDiceBorder = CreateInteractiveStatFrame("Hit Dice", character.Combat.HitPoints.HitDice, "Hit Dice", character.Combat.HitPoints.HitDice, newValue =>
        {
            character.Combat.HitPoints.HitDice = newValue;
            Refresh(character);
        });
        statsGrid.Add(hitDiceBorder, 2, 1);

        return statsGrid;
    }

    private static Grid BuildAbilitySection(CharacterSheet character)
    {
        var abilityGrid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star }
        },
            RowDefinitions =
        {
            new RowDefinition { Height = GridLength.Auto },
            new RowDefinition { Height = GridLength.Auto },
            new RowDefinition { Height = GridLength.Auto }
        },
            ColumnSpacing = 4,
            RowSpacing = 4
        };

        var abilityData = new (string Label, int Score, int SaveBonus, int Column, int Row)[]
        {
            ("Strength", character.AbilityScores.Strength, character.SavingThrows.Strength, 0, 0),
            ("Dexterity", character.AbilityScores.Dexterity, character.SavingThrows.Dexterity, 1, 0),
            ("Constitution", character.AbilityScores.Constitution, character.SavingThrows.Constitution, 0, 1),
            ("Intelligence", character.AbilityScores.Intelligence, character.SavingThrows.Intelligence, 1, 1),
            ("Wisdom", character.AbilityScores.Wisdom, character.SavingThrows.Wisdom, 0, 2),
            ("Charisma", character.AbilityScores.Charisma, character.SavingThrows.Charisma, 1, 2)
        };

        foreach (var (label, score, saveBonus, column, row) in abilityData)
        {
            abilityGrid.Add(BuildAbilityFrame(label, score, saveBonus), column, row);
        }

        return abilityGrid;
    }

    private static Button CreateEditCharacterButton(CharacterSheet characterSheet)
    {
        var editButton = new Button
        {
            Text = "Edit Character Info"
        };

        editButton.Clicked += (sender, args) =>
        {
            var characterInfoPopup = new CharacterInfoPopup(characterSheet);

            characterInfoPopup.Closed += (_, __) =>
            {
                var characterStatsPopup = new CharacterStatsPopup(characterSheet);

                characterStatsPopup.Closed += (_, __) =>
                {
                    var abilityScoresPopup = new AbilityScoresPopup(characterSheet);

                    abilityScoresPopup.Closed += (_, __) =>
                    {
                        var savingThrowsPopup = new SavingThrowsPopup(characterSheet);

                        savingThrowsPopup.Closed += (_, __) =>
                        {
                            Refresh(characterSheet);
                        };

                        Application.Current.Windows[0].Page.ShowPopup(savingThrowsPopup);
                    };

                    Application.Current.Windows[0].Page.ShowPopup(abilityScoresPopup);
                };

                Application.Current.Windows[0].Page.ShowPopup(characterStatsPopup);
            };

            Application.Current.Windows[0].Page.ShowPopup(characterInfoPopup);
        };

        return editButton;
    }

    private static Label CreateBoldLabel(string text)
    {
        return new Label
        {
            Text = text,
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        };
    }

    private static Label CreateCenteredLabel(string text)
    {
        return new Label
        {
            Text = text,
            HorizontalOptions = LayoutOptions.Center
        };
    }

    private static Label CreateInteractiveLabel(string title, string valueText, int currentValue, Action<int> onValueChanged)
    {
        var label = new Label
        {
            Text = valueText,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Orange
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (_, __) =>
        {
            var popup = new GenericValueEditPopup($"{title}: ", currentValue, onValueChanged);
            Application.Current.Windows[0].Page.ShowPopup(popup);
        };

        label.GestureRecognizers.Add(tapGesture);
        return label;
    }

    private static Border BuildAbilityFrame(string abilityLabel, int abilityScore, int savingThrowBonus)
    {
        int abilityModifier = (int)Math.Floor((abilityScore - 10) / 2.0);

        var abilityGrid = new Grid
        {
            RowDefinitions =
        {
            new RowDefinition { Height = GridLength.Auto }, // Label row
            new RowDefinition { Height = GridLength.Auto }, // Header row
            new RowDefinition { Height = GridLength.Auto }  // Value row
        },
            ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star }
        }
        };

        // Ability label (spans all columns)
        var label = new Label
        {
            Text = abilityLabel,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };
        abilityGrid.Add(label, 0, 0);
        Grid.SetColumnSpan(label, 3);

        // Header row
        abilityGrid.Add(new Label
        {
            Text = "Score",
            FontSize = 10,
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 1);

        abilityGrid.Add(new Label
        {
            Text = "Mod",
            FontSize = 10,
            HorizontalTextAlignment = TextAlignment.Center
        }, 1, 1);

        abilityGrid.Add(new Label
        {
            Text = "Save",
            FontSize = 10,
            HorizontalTextAlignment = TextAlignment.Center
        }, 2, 1);

        // Value row
        abilityGrid.Add(new Label
        {
            Text = abilityScore.ToString(),
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 2);

        abilityGrid.Add(new Label
        {
            Text = $"{abilityModifier:+#;-#;0}",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 1, 2);

        abilityGrid.Add(new Label
        {
            Text = $"{savingThrowBonus:+#;-#;0}",
            HorizontalTextAlignment = TextAlignment.Center
        }, 2, 2);

        return new Border
        {
            Content = abilityGrid,
            Stroke = Colors.Black,
            StrokeThickness = 2,
            Padding = new Thickness(5),
            StrokeShape = new RoundRectangle { CornerRadius = 8 }
        };
    }

    private static Border CreateStatFrame(string label, string value)
    {
        return new Border
        {
            Content = new Label
            {
                Text = $"{label}\n{value}",
                HorizontalTextAlignment = TextAlignment.Center
            },
            Stroke = Colors.Black,
            StrokeThickness = 2,
            Padding = new Thickness(5),
            StrokeShape = new RoundRectangle { CornerRadius = 8 }
        };
    }

    private static Border CreateInteractiveStatFrame(string label, string value, string popupTitle, string currentValue, Action<string> onValueChanged)
    {
        var statLabel = new Label
        {
            Text = $"{label}\n{value}",
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Colors.Orange
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (_, __) =>
        {
            var popup = new GenericValueEditPopup(popupTitle, currentValue, onValueChanged);
            Application.Current.Windows[0].Page.ShowPopup(popup);
        };

        statLabel.GestureRecognizers.Add(tapGesture);

        return new Border
        {
            Content = statLabel,
            Stroke = Colors.Black,
            StrokeThickness = 2,
            Padding = new Thickness(5),
            StrokeShape = new RoundRectangle { CornerRadius = 8 }
        };
    }
}
