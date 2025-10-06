using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class GenerateHomePage
{
    private static Grid _parentGrid;

    public static View GenerateHomeGrid(CharacterSheet character)
    {
        _parentGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto } // Edit Button
            },
            Padding = new Thickness(10),
            RowSpacing = 15
        };

        var identityGrid = GenerateIdentityGrid(character);
        var statsGrid = GenerateStatsGrid(character);
        var abilitiesGrid = GenerateAbilityGrid(character);
        var editButton = GenerateEditButton(character);

        _parentGrid.Add(identityGrid, 0, 0);
        _parentGrid.Add(statsGrid, 0, 1);
        _parentGrid.Add(abilitiesGrid, 0, 2);
        _parentGrid.Add(editButton, 0, 3);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _parentGrid
        };
    }

    public static void RefreshHomeGrid(CharacterSheet updatedCharacter)
    {
        if (_parentGrid == null)
            return;

        _parentGrid.Children.Clear();

        var identityGrid = GenerateIdentityGrid(updatedCharacter);
        var statsGrid = GenerateStatsGrid(updatedCharacter);
        var abilitiesGrid = GenerateAbilityGrid(updatedCharacter);
        var editButton = GenerateEditButton(updatedCharacter);

        _parentGrid.Add(identityGrid, 0, 0);
        _parentGrid.Add(statsGrid, 0, 1);
        _parentGrid.Add(abilitiesGrid, 0, 2);
        _parentGrid.Add(editButton, 0, 3);
    }

    private static Button GenerateEditButton(CharacterSheet character)
    {
        var editButton = new Button
        {
            Text = "Edit Character Info"
        };

        editButton.Clicked += (s, e) =>
        {
            var infoPopup = new CharacterInfoPopup(character);

            infoPopup.Closed += (sender1, args1) =>
            {
                var statsPopup = new CharacterStatsPopup(character);

                statsPopup.Closed += (sender2, args2) =>
                {
                    var abilityPopup = new AbilityScoresPopup(character);

                    abilityPopup.Closed += (sender3, args3) =>
                    {
                        var savingPopup = new SavingThrowsPopup(character);

                        savingPopup.Closed += (sender4, args4) =>
                        {
                            RefreshHomeGrid(character);
                        };

                        Application.Current.MainPage.ShowPopup(savingPopup);
                    };

                    Application.Current.MainPage.ShowPopup(abilityPopup);
                };

                Application.Current.MainPage.ShowPopup(statsPopup);
            };

            Application.Current.MainPage.ShowPopup(infoPopup);
        };

        return editButton;
    }

    private static Grid GenerateIdentityGrid(CharacterSheet character)
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

        identityGrid.Add(new Label
        {
            Text = character.Character.Name,
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 0);

        identityGrid.Add(new Label
        {
            Text = character.Character.Race,
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        }, 1, 0);

        identityGrid.Add(new Label
        {
            Text = $"{character.Character.Class} {character.Character.Level}",
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center
        }, 2, 0);

        identityGrid.Add(new Label { Text = character.Character.Background, HorizontalOptions = LayoutOptions.Center }, 0, 1);
        identityGrid.Add(new Label { Text = character.Character.Alignment, HorizontalOptions = LayoutOptions.Center }, 1, 1);
        
        var xpLabel = new Label {
            Text = character.Character.ExperiencePoints.ToString(),
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Orange
        };

        var xpTap = new TapGestureRecognizer();
        xpTap.Tapped += (s, e) =>
        {
            var popup = new GenericValueEditPopup("Experience Points: ", character.Character.ExperiencePoints ?? 0, newValue =>
            {
                character.Character.ExperiencePoints = newValue;
                GenerateHomePage.RefreshHomeGrid(character);
            });

            Application.Current.MainPage.ShowPopup(popup);
        };
        xpLabel.GestureRecognizers.Add(xpTap);

        identityGrid.Add(xpLabel, 2, 1);

        return identityGrid;
    }

    private static Grid GenerateStatsGrid(CharacterSheet character)
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
        statsGrid.Add(new Frame
        {
            Content = new Label { Text = $"AC\n{character.Combat.ArmorClass}", HorizontalTextAlignment = TextAlignment.Center },
            BorderColor = Colors.Black,
            CornerRadius = 8,
            Padding = new Thickness(5)
        }, 0, 0);

        // HP (interactive)
        var hpLabel = new Label
        {
            Text = $"HP\n{character.Combat.HitPoints.Current}/{character.Combat.HitPoints.Maximum}",
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Colors.Orange
        };

        var hpTap = new TapGestureRecognizer();
        hpTap.Tapped += (s, e) =>
        {
            var popup = new GenericValueEditPopup("Current HP", character.Combat.HitPoints.Current, newValue =>
            {
                character.Combat.HitPoints.Current = newValue;
                GenerateHomePage.RefreshHomeGrid(character); // Replace with your actual character reference
            });

            Application.Current.MainPage.ShowPopup(popup);
        };

        hpLabel.GestureRecognizers.Add(hpTap);

        statsGrid.Add(new Frame
        {
            Content = hpLabel,
            BorderColor = Colors.Black,
            CornerRadius = 8,
            Padding = new Thickness(5)
        }, 1, 0);

        // Temp HP (interactive)
        var tempHpLabel = new Label
        {
            Text = $"Temp HP\n{character.Combat.HitPoints.Temporary}",
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Colors.Orange
        };

        var tempHpTap = new TapGestureRecognizer();
        tempHpTap.Tapped += (s, e) =>
        {
            var popup = new GenericValueEditPopup("Temporary HP", character.Combat.HitPoints.Temporary, newValue =>
            {
                character.Combat.HitPoints.Temporary = newValue;
                GenerateHomePage.RefreshHomeGrid(character);
            });

            Application.Current.MainPage.ShowPopup(popup);
        };

        tempHpLabel.GestureRecognizers.Add(tempHpTap);

        statsGrid.Add(new Frame
        {
            Content = tempHpLabel,
            BorderColor = Colors.Black,
            CornerRadius = 8,
            Padding = new Thickness(5)
        }, 2, 0);

        // Initiative
        statsGrid.Add(new Frame
        {
            Content = new Label { Text = $"Initiative\n{character.Combat.Initiative}", HorizontalTextAlignment = TextAlignment.Center },
            BorderColor = Colors.Black,
            CornerRadius = 8,
            Padding = new Thickness(5)
        }, 0, 1);

        // Speed
        statsGrid.Add(new Frame
        {
            Content = new Label { Text = $"Speed\n{character.Combat.Speed}", HorizontalTextAlignment = TextAlignment.Center },
            BorderColor = Colors.Black,
            CornerRadius = 8,
            Padding = new Thickness(5)
        }, 1, 1);

        // Hit Dice (interactive)
        var hitDiceLabel = new Label
        {
            Text = $"Hit Dice\n{character.Combat.HitPoints.HitDice}",
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Colors.Orange
        };

        var hitDiceTap = new TapGestureRecognizer();
        hitDiceTap.Tapped += (s, e) =>
        {
            var popup = new GenericValueEditPopup("Hit Dice:", character.Combat.HitPoints.HitDice, newValue =>
            {
                character.Combat.HitPoints.HitDice = newValue;
                GenerateHomePage.RefreshHomeGrid(character); // Replace with your actual character reference
            });

            Application.Current.MainPage.ShowPopup(popup);
        };

        hitDiceLabel.GestureRecognizers.Add(hitDiceTap);

        statsGrid.Add(new Frame
        {
            Content = hitDiceLabel,
            BorderColor = Colors.Black,
            CornerRadius = 8,
            Padding = new Thickness(5)
        }, 2, 1);

        return statsGrid;
    }

    private static Grid GenerateAbilityGrid(CharacterSheet character)
    {
        var abilitiesGrid = new Grid
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

        // Fill the 2x3 grid
        abilitiesGrid.Add(CreateAbilityFrame("Strength",
            character.AbilityScores.Strength,
            character.SavingThrows.Strength), 0, 0);

        abilitiesGrid.Add(CreateAbilityFrame("Dexterity",
            character.AbilityScores.Dexterity,
            character.SavingThrows.Dexterity), 1, 0);

        abilitiesGrid.Add(CreateAbilityFrame("Constitution",
            character.AbilityScores.Constitution,
            character.SavingThrows.Constitution), 0, 1);

        abilitiesGrid.Add(CreateAbilityFrame("Intelligence",
            character.AbilityScores.Intelligence,
            character.SavingThrows.Intelligence), 1, 1);

        abilitiesGrid.Add(CreateAbilityFrame("Wisdom",
            character.AbilityScores.Wisdom,
            character.SavingThrows.Wisdom), 0, 2);

        abilitiesGrid.Add(CreateAbilityFrame("Charisma",
            character.AbilityScores.Charisma,
            character.SavingThrows.Charisma), 1, 2);

        return abilitiesGrid;
    }

    static Frame CreateAbilityFrame(string abilityName, int score, int save)
    {
        int modifier = (int)Math.Floor((score - 10) / 2.0);

        var innerGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Ability name
                new RowDefinition { Height = GridLength.Auto }, // Headers
                new RowDefinition { Height = GridLength.Auto }  // Values
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        // Ability name on top (spans all columns)
        var nameLabel = new Label
        {
            Text = abilityName,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        };
        innerGrid.Add(nameLabel, 0, 0);
        Grid.SetColumnSpan(nameLabel, 3);

        // Headers row
        innerGrid.Add(new Label
        {
            Text = "Score",
            FontSize = 10,
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 1);

        innerGrid.Add(new Label
        {
            Text = "Mod",
            FontSize = 10,
            HorizontalTextAlignment = TextAlignment.Center
        }, 1, 1);

        innerGrid.Add(new Label
        {
            Text = "Save",
            FontSize = 10,
            HorizontalTextAlignment = TextAlignment.Center
        }, 2, 1);

        // Values row
        innerGrid.Add(new Label
        {
            Text = score.ToString(),
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 2);

        innerGrid.Add(new Label
        {
            Text = $"{modifier:+#;-#;0}",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 1, 2);

        innerGrid.Add(new Label
        {
            Text = $"{save:+#;-#;0}",
            HorizontalTextAlignment = TextAlignment.Center
        }, 2, 2);

        return new Frame
        {
            Content = innerGrid,
            BorderColor = Colors.Black,
            CornerRadius = 8,
            Padding = new Thickness(5)
        };
    }
}
