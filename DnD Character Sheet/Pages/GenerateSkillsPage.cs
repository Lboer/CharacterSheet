using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class GenerateSkillsPage
{
    private static Grid _parentGrid;

    public static View GenerateSkillsGrid(CharacterSheet character)
    {
        _parentGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Proficiency Bonus
                new RowDefinition { Height = GridLength.Auto }, // Skills
                new RowDefinition { Height = GridLength.Auto }  // Edit Button
            },
            Padding = new Thickness(10),
            RowSpacing = 15
        };

        var bonusGrid = GenerateProficiencyBonusGrid(character.ProficiencyBonus);
        var skillsGrid = GenerateCharacterSkillsGrid(character);
        var editButton = GenerateEditButton(character);

        _parentGrid.Add(bonusGrid, 0, 0);
        _parentGrid.Add(skillsGrid, 0, 1);
        _parentGrid.Add(editButton, 0, 2);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _parentGrid
        };
    }

    public static void RefreshSkillsGrid(CharacterSheet updatedCharacter)
    {
        if (_parentGrid == null)
            return;

        _parentGrid.Children.Clear();

        var bonusGrid = GenerateProficiencyBonusGrid(updatedCharacter.ProficiencyBonus);
        var skillsGrid = GenerateCharacterSkillsGrid(updatedCharacter);
        var editButton = GenerateEditButton(updatedCharacter);

        _parentGrid.Add(bonusGrid, 0, 0);
        _parentGrid.Add(skillsGrid, 0, 1);
        _parentGrid.Add(editButton, 0, 2);
    }

    private static Button GenerateEditButton(CharacterSheet character)
    {
        var editButton = new Button
        {
            Text = "Edit Skills"
        };

        editButton.Clicked += (s, e) =>
        {
            var startPopup = new SkillsStartPopup(character);

            startPopup.Closed += (sender1, args1) =>
            {
                var endPopup = new SkillsEndPopup(character);

                endPopup.Closed += (sender2, args2) =>
                {
                    RefreshSkillsGrid(character);
                };

                Application.Current.MainPage.ShowPopup(endPopup);
            };

            Application.Current.MainPage.ShowPopup(startPopup);
        };

        return editButton;
    }

    private static Grid GenerateProficiencyBonusGrid(int proficiencyBonus)
    {
        var grid = new Grid
        {
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

        grid.Add(new Label
        {
            Text = $"Proficiency Bonus: +{proficiencyBonus}",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center
        }, 0, 0);

        return grid;
    }

    private static Grid GenerateCharacterSkillsGrid(CharacterSheet character)
    {
        var strengthBox = BuildAbilityBox("Strength", (character.AbilityScores.Strength - 10) / 2, new Dictionary<string, int>
        {
            { "Athletics", character.Skills.Athletics }
        });

        var dexterityBox = BuildAbilityBox("Dexterity", (character.AbilityScores.Dexterity - 10) / 2, new Dictionary<string, int>
        {
            { "Acrobatics", character.Skills.Acrobatics },
            { "Sleight of Hand", character.Skills.SleightOfHand },
            { "Stealth", character.Skills.Stealth }
        });

        var constitutionBox = BuildAbilityBox("Constitution", (character.AbilityScores.Constitution - 10) / 2, new Dictionary<string, int>());

        var intelligenceBox = BuildAbilityBox("Intelligence", (character.AbilityScores.Intelligence - 10) / 2, new Dictionary<string, int>
        {
            { "Arcana", character.Skills.Arcana },
            { "History", character.Skills.History },
            { "Investigation", character.Skills.Investigation },
            { "Nature", character.Skills.Nature },
            { "Religion", character.Skills.Religion }
        });

        var wisdomBox = BuildAbilityBox("Wisdom", (character.AbilityScores.Wisdom - 10) / 2, new Dictionary<string, int>
        {
            { "Animal Handling", character.Skills.AnimalHandling },
            { "Insight", character.Skills.Insight },
            { "Medicine", character.Skills.Medicine },
            { "Perception", character.Skills.Perception },
            { "Survival", character.Skills.Survival }
        });

        var charismaBox = BuildAbilityBox("Charisma", (character.AbilityScores.Charisma - 10) / 2, new Dictionary<string, int>
        {
            { "Deception", character.Skills.Deception },
            { "Intimidation", character.Skills.Intimidation },
            { "Performance", character.Skills.Performance },
            { "Persuasion", character.Skills.Persuasion }
        });

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

        grid.Add(strengthBox, 0, 0);
        grid.Add(dexterityBox, 1, 0);
        grid.Add(constitutionBox, 0, 1);
        grid.Add(intelligenceBox, 1, 1);
        grid.Add(wisdomBox, 0, 2);
        grid.Add(charismaBox, 1, 2);

        return grid;
    }

    public static View BuildAbilityBox(string abilityName, int modifier, Dictionary<string, int> skills)
    {
        var modifierLabel = new Label
        {
            Text = $"{(modifier >= 0 ? "+" : "")}{modifier}",
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            TextColor = modifier < 0 ? Colors.Red : Colors.Green,
            HorizontalOptions = LayoutOptions.EndAndExpand,
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


        // Skills list
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
        },
                Children =
        {
            new Label
            {
                Text = skill.Key,
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            },
            valueLabel
        }
            };

            Grid.SetColumn(valueLabel, 1);

            skillList.Children.Add(row);
        }


        // Combine header and skills
        return new VerticalStackLayout
        {
            Padding = new Thickness(5),
            Spacing = 6,
            Children = { header, skillList }
        };
    }
}