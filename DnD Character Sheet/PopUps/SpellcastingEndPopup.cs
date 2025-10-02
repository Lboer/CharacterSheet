using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class SpellcastingEndPopup : Popup
{
    public CharacterSheet Character { get; private set; }
    private List<VerticalStackLayout> spellRows = new();
    private VerticalStackLayout spellEntryContainer;

    public SpellcastingEndPopup(CharacterSheet character)
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
            Text = "Spells and cantrips represent your character's magical abilities. Add or edit them below.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        layout.Children.Add(new Label
        {
            Text = "Character Spells",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            Margin = new Thickness(0, 10, 0, 0)
        });

        spellEntryContainer = new VerticalStackLayout { Spacing = 10 };
        layout.Children.Add(spellEntryContainer);

        foreach (var spell in Character.SpellCasting.Cantrips)
        {
            AddSpellRow(spell.Name, spell.Description, spell.Damage, spell.DamageType, spell.Level, spell.CastingTime, spell.Range, spell.Components, spell.Duration, spell.Area);
        }

        foreach (var spell in Character.SpellCasting.Spells)
        {
            AddSpellRow(spell.Name, spell.Description, spell.Damage, spell.DamageType, spell.Level, spell.CastingTime, spell.Range, spell.Components, spell.Duration, spell.Area);
        }

        var addButton = new Button
        {
            Text = "Add Spell",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };

        addButton.Clicked += (s, e) => AddSpellRow("", "", "", "", 0, "", "", "", "", "");
        layout.Children.Add(addButton);

        var saveButton = new Button
        {
            Text = "Save",
            BackgroundColor = Colors.Purple,
            TextColor = Colors.White,
            Margin = new Thickness(0, 10, 0, 0)
        };

        saveButton.Clicked += (s, e) =>
        {
            var allSpells = spellRows.Select(row =>
            {
                var detailsLayout = row.Children.OfType<VerticalStackLayout>().FirstOrDefault();
                var entries = detailsLayout?.Children.OfType<Entry>().ToList();
                var editors = detailsLayout?.Children.OfType<Editor>().ToList();

                return new Spell
                {
                    Name = entries?[0]?.Text?.Trim() ?? "",
                    Description = editors?[0]?.Text?.Trim() ?? "",
                    Damage = entries?[1]?.Text?.Trim(),
                    DamageType = entries?[2]?.Text?.Trim(),
                    Level = int.TryParse(entries?[3]?.Text, out var lvl) ? lvl : 0,
                    CastingTime = entries?[4]?.Text?.Trim() ?? "",
                    Range = entries?[5]?.Text?.Trim() ?? "",
                    Components = entries?[6]?.Text?.Trim() ?? "",
                    Duration = entries?[7]?.Text?.Trim() ?? "",
                    Area = entries?[8]?.Text?.Trim()
                };
            })
            .Where(s => !string.IsNullOrWhiteSpace(s.Name))
            .ToList();

            Character.SpellCasting.Cantrips = allSpells.Where(s => s.Level == 0).ToList();
            Character.SpellCasting.Spells = allSpells.Where(s => s.Level > 0).ToList();

            Close();
        };

        layout.Children.Add(saveButton);

        Content = new ScrollView
        {
            Content = new VerticalStackLayout { Children = { layout } }
        };
    }

    private void AddSpellRow(string name, string description, string damage, string damageType, int level, string castingTime, string range, string components, string duration, string area)
    {
        var nameEntry = new Entry { Text = name, Placeholder = "Spell Name" };
        var descEditor = new Editor
        {
            Text = description,
            Placeholder = "Description",
            AutoSize = EditorAutoSizeOption.TextChanges,
            HeightRequest = 60
        };
        var damageEntry = new Entry { Text = damage, Placeholder = "Damage" };
        var typeEntry = new Entry { Text = damageType, Placeholder = "Damage Type" };
        var levelEntry = new Entry { Text = level.ToString(), Placeholder = "Level", Keyboard = Keyboard.Numeric };
        var timeEntry = new Entry { Text = castingTime, Placeholder = "Casting Time" };
        var rangeEntry = new Entry { Text = range, Placeholder = "Range" };
        var compEntry = new Entry { Text = components, Placeholder = "Components" };
        var durationEntry = new Entry { Text = duration, Placeholder = "Duration" };
        var areaEntry = new Entry { Text = area, Placeholder = "Area of Effect" };

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
                descEditor,
                new Label { Text = "Damage", FontAttributes = FontAttributes.Bold },
                damageEntry,
                new Label { Text = "Damage Type", FontAttributes = FontAttributes.Bold },
                typeEntry,
                new Label { Text = "Level", FontAttributes = FontAttributes.Bold },
                levelEntry,
                new Label { Text = "Casting Time", FontAttributes = FontAttributes.Bold },
                timeEntry,
                new Label { Text = "Range", FontAttributes = FontAttributes.Bold },
                rangeEntry,
                new Label { Text = "Components", FontAttributes = FontAttributes.Bold },
                compEntry,
                new Label { Text = "Duration", FontAttributes = FontAttributes.Bold },
                durationEntry,
                new Label { Text = "Area of Effect", FontAttributes = FontAttributes.Bold },
                areaEntry
            }
        };

        var spellBlock = new VerticalStackLayout
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
            spellEntryContainer.Children.Remove(spellBlock);
            spellRows.Remove(spellBlock);
        };

        spellRows.Add(spellBlock);
        spellEntryContainer.Children.Add(spellBlock);
    }
}
