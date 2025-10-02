using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class SpellcastingStartPopup : Popup
{
    public CharacterSheet Character { get; private set; }
    private VerticalStackLayout levelContainer;
    private int unlockedLevels => Character.SpellCasting.Levels.Count;

    public SpellcastingStartPopup(CharacterSheet character)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            WidthRequest = 340
        };

        layout.Children.Add(new Label
        {
            Text = "Configure your character's spellcasting stats and spell slot levels.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        // Spellcasting Ability
        var abilityEntry = new Entry
        {
            Text = Character.SpellCasting.SpellcastingAbility,
            Placeholder = "Spellcasting Ability (e.g., Intelligence)"
        };
        layout.Children.Add(new Label { Text = "Spellcasting Ability" });
        layout.Children.Add(abilityEntry);

        // Spell Save DC
        var dcEntry = new Entry
        {
            Text = Character.SpellCasting.SpellSaveDC.ToString(),
            Keyboard = Keyboard.Numeric,
            Placeholder = "Spell Save DC"
        };
        layout.Children.Add(new Label { Text = "Spell Save DC" });
        layout.Children.Add(dcEntry);

        // Spell Attack Bonus
        var attackEntry = new Entry
        {
            Text = Character.SpellCasting.SpellAttackBonus.ToString(),
            Placeholder = "Spell Attack Bonus"
        };
        layout.Children.Add(new Label { Text = "Spell Attack Bonus" });
        layout.Children.Add(attackEntry);

        // Spell Slot Levels
        layout.Children.Add(new Label
        {
            Text = "Spell Slot Levels",
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 10, 0, 0)
        });

        levelContainer = new VerticalStackLayout { Spacing = 10 };

        // Header row for spell slot levels
        var headerRow = new HorizontalStackLayout
        {
            Spacing = 10,
            Children =
            {
                new Label
                {
                    Text = "Level",
                    ClassId = "Description",
                    FontAttributes = FontAttributes.Bold,
                    WidthRequest = 40
                },
                new Label
                {
                    Text = "Max Casts",
                    FontAttributes = FontAttributes.Bold,
                    WidthRequest = 80
                },
                new Label
                {
                    Text = "Currently Available",
                    FontAttributes = FontAttributes.Bold,
                    WidthRequest = 120
                }
            }
        };

        levelContainer.Children.Add(headerRow);
        layout.Children.Add(levelContainer);

        foreach (var level in Character.SpellCasting.Levels)
        {
            AddLevelRow(level.Level, level.Max, level.Current);
        }

        // Unlock next level button
        var unlockButton = new Button
        {
            Text = "Unlock Next Level",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };

        unlockButton.Clicked += (s, e) =>
        {
            if (unlockedLevels < 9)
            {
                var nextLevel = (unlockedLevels + 1);
                Character.SpellCasting.Levels.Add(new SpellLevel
                {
                    Level = nextLevel,
                    Max = 0,
                    Current = 0
                });
                AddLevelRow(nextLevel, 0, 0);
            }
        };

        layout.Children.Add(unlockButton);

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
            Character.SpellCasting.SpellcastingAbility = abilityEntry.Text?.Trim() ?? "N/A";
            Character.SpellCasting.SpellSaveDC = int.TryParse(dcEntry.Text, out var dc) ? dc : 0;
            Character.SpellCasting.SpellAttackBonus = int.TryParse(attackEntry.Text, out var atk) ? atk : 0;

            foreach (var row in levelContainer.Children.OfType<HorizontalStackLayout>())
            {
                var levelLabel = row.Children[0] as Label;
                if (levelLabel.ClassId != null && levelLabel.ClassId == "Description")
                    continue;
                var maxEntry = row.Children[1] as Entry;
                var currentEntry = row.Children[2] as Entry;

                var levelObj = Character.SpellCasting.Levels.FirstOrDefault(l => l.Level == int.Parse(levelLabel.Text));
                if (levelObj != null)
                {
                    levelObj.Max = int.TryParse(maxEntry.Text, out var max) ? max : 0;
                    levelObj.Current = int.TryParse(currentEntry.Text, out var cur) ? cur : 0;
                }
            }

            Close();
        };

        layout.Children.Add(saveButton);

        var removeButton = new Button
        {
            Text = "Remove Last Level",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };

        removeButton.Clicked += (s, e) => RemoveLevelRow();
        layout.Children.Add(removeButton);

        Content = new ScrollView
        {
            Content = new VerticalStackLayout { Children = { layout } }
        };
    }

    private void AddLevelRow(int level, int max, int current)
    {
        var levelLabel = new Label
        {
            Text = level.ToString(),
            WidthRequest = 40,
            VerticalOptions = LayoutOptions.Center
        };

        var maxEntry = new Entry
        {
            Text = max.ToString(),
            Keyboard = Keyboard.Numeric,
            Placeholder = "Max",
            WidthRequest = 60
        };

        var currentEntry = new Entry
        {
            Text = current.ToString(),
            Keyboard = Keyboard.Numeric,
            Placeholder = "Current",
            WidthRequest = 60
        };

        var row = new HorizontalStackLayout
        {
            Spacing = 10,
            Children = { levelLabel, maxEntry, currentEntry }
        };

        levelContainer.Children.Add(row);
    }

    private void RemoveLevelRow()
    {
        if (Character.SpellCasting.Levels.Count > 1)
        {
            var lastLevel = Character.SpellCasting.Levels.Last();
            Character.SpellCasting.Levels.Remove(lastLevel);

            // Find and remove the corresponding UI row
            var rowToRemove = levelContainer.Children
                .OfType<HorizontalStackLayout>()
                .FirstOrDefault(row =>
                {
                    var label = row.Children[0] as Label;
                    return int.Parse(label?.Text) == lastLevel.Level;
                });

            if (rowToRemove != null)
            {
                levelContainer.Children.Remove(rowToRemove);
            }
        }
    }

}