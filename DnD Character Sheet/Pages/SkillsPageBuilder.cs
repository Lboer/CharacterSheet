using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class SkillsPageBuilder
{
    private static Grid _skillsGrid;

    public static View Build(CharacterSheet character)
    {
        _skillsGrid = CreateGridLayout();
        PopulateGrid(character);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _skillsGrid
        };
    }

    public static void Refresh(CharacterSheet updatedCharacter)
    {
        if (_skillsGrid == null)
            return;

        _skillsGrid.Children.Clear();
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
                new RowDefinition { Height = GridLength.Auto }, // Proficiency Bonus
                new RowDefinition { Height = GridLength.Auto }, // Skills Grid
                new RowDefinition { Height = GridLength.Auto }  // Edit Button
            }
        };
    }

    private static void PopulateGrid(CharacterSheet character)
    {
        var bonusRow = BuildProficiencyBonusRow(character.ProficiencyBonus);
        var skillsGrid = BuildSkillsGrid(character);
        var editButton = BuildEditButton(character);

        _skillsGrid.Add(bonusRow, 0, 0);
        _skillsGrid.Add(skillsGrid, 0, 1);
        _skillsGrid.Add(editButton, 0, 2);
    }

    private static Grid BuildProficiencyBonusRow(int bonus)
    {
        var grid = new Grid
        {
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

        grid.Add(new Label
        {
            Text = $"Proficiency Bonus: +{bonus}",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 0);

        return grid;
    }

    private static Grid BuildSkillsGrid(CharacterSheet character)
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
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            }
        };

        grid.Add(BuildAbilityBox("Strength", character.AbilityScores.Strength, new Dictionary<string, int>
        {
            { "Athletics", character.Skills.Athletics }
        }), 0, 0);

        grid.Add(BuildAbilityBox("Dexterity", character.AbilityScores.Dexterity, new Dictionary<string, int>
        {
            { "Acrobatics", character.Skills.Acrobatics },
            { "Sleight of Hand", character.Skills.SleightOfHand },
            { "Stealth", character.Skills.Stealth }
        }), 1, 0);

        grid.Add(BuildAbilityBox("Constitution", character.AbilityScores.Constitution, new Dictionary<string, int>()), 0, 1);

        grid.Add(BuildAbilityBox("Intelligence", character.AbilityScores.Intelligence, new Dictionary<string, int>
        {
            { "Arcana", character.Skills.Arcana },
            { "History", character.Skills.History },
            { "Investigation", character.Skills.Investigation },
            { "Nature", character.Skills.Nature },
            { "Religion", character.Skills.Religion }
        }), 1, 1);

        grid.Add(BuildAbilityBox("Wisdom", character.AbilityScores.Wisdom, new Dictionary<string, int>
        {
            { "Animal Handling", character.Skills.AnimalHandling },
            { "Insight", character.Skills.Insight },
            { "Medicine", character.Skills.Medicine },
            { "Perception", character.Skills.Perception },
            { "Survival", character.Skills.Survival }
        }), 0, 2);

        grid.Add(BuildAbilityBox("Charisma", character.AbilityScores.Charisma, new Dictionary<string, int>
        {
            { "Deception", character.Skills.Deception },
            { "Intimidation", character.Skills.Intimidation },
            { "Performance", character.Skills.Performance },
            { "Persuasion", character.Skills.Persuasion }
        }), 1, 2);

        return grid;
    }

    private static View BuildAbilityBox(string abilityName, int score, Dictionary<string, int> skills)
    {
        int modifier = (int)Math.Floor((score - 10) / 2.0);

        var modifierLabel = new Label
        {
            Text = $"{(modifier >= 0 ? "+" : "")}{modifier}",
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            TextColor = modifier < 0 ? Colors.Red : Colors.Green,
            HorizontalTextAlignment = TextAlignment.End
        };

        var header = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            Padding = new Thickness(0, 5)
        };

        header.Add(new Label
        {
            Text = abilityName,
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center
        }, 0, 0);

        header.Add(modifierLabel, 1, 0);

        var skillList = new VerticalStackLayout
        {
            Spacing = 4,
            Padding = new Thickness(10, 0)
        };

        foreach (var skill in skills)
        {
            var valueLabel = new Label
            {
                Text = $"{(skill.Value >= 0 ? "+" : "")}{skill.Value}",
                FontSize = 14,
                HorizontalOptions = LayoutOptions.End,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.Center
            };

            var row = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            row.Add(new Label
            {
                Text = skill.Key,
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            }, 0, 0);

            row.Add(valueLabel, 1, 0);
            skillList.Children.Add(row);
        }

        return new VerticalStackLayout
        {
            Padding = new Thickness(5),
            Spacing = 6,
            Children = { header, skillList }
        };
    }

    private static Button BuildEditButton(CharacterSheet character)
    {
        var button = new Button
        {
            Text = "Edit Skills"
        };

        button.Clicked += (s, e) =>
        {
            var startPopup = new SkillsStartPopup(character);
            startPopup.Closed += (_, __) =>
            {
                var endPopup = new SkillsEndPopup(character);
                endPopup.Closed += (_, __) => Refresh(character);
                Application.Current.Windows[0].Page.ShowPopup(endPopup);
            };

            Application.Current.Windows[0].Page.ShowPopup(startPopup);
        };

        return button;
    }
}
