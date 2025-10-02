using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class WeaponsPopup : Popup
{
    public CharacterSheet Character { get; private set; }
    private List<VerticalStackLayout> weaponRows = new();
    private VerticalStackLayout weaponEntryContainer;

    public WeaponsPopup(CharacterSheet character)
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
            Text = "Weapons include your character's combat tools. Add or edit them below.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        layout.Children.Add(new Label
        {
            Text = "Character Weapons",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            Margin = new Thickness(0, 10, 0, 0)
        });

        weaponEntryContainer = new VerticalStackLayout { Spacing = 10 };
        layout.Children.Add(weaponEntryContainer);

        foreach (var weapon in character.Weapons)
        {
            AddWeaponRow(weapon.Name, weapon.AttackBonus, weapon.Damage, weapon.DamageType, weapon.Range, weapon.Properties);
        }

        var addButton = new Button
        {
            Text = "Add Weapon",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };

        addButton.Clicked += (s, e) => AddWeaponRow("", "", "", "", "", new List<string>());
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
            Character.Weapons = weaponRows.Select(row =>
            {
                var detailsLayout = row.Children.OfType<VerticalStackLayout>().FirstOrDefault();
                var entries = detailsLayout?.Children.OfType<Entry>().ToList();
                var propsEditor = detailsLayout?.Children.OfType<Editor>().FirstOrDefault();

                return new Weapon
                {
                    Name = entries?[0]?.Text?.Trim() ?? "",
                    AttackBonus = entries?[1]?.Text?.Trim() ?? "",
                    Damage = entries?[2]?.Text?.Trim() ?? "",
                    DamageType = entries?[3]?.Text?.Trim() ?? "",
                    Range = entries?[4]?.Text?.Trim() ?? "",
                    Properties = propsEditor?.Text?.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList() ?? new List<string>()
                };
            })
            .Where(w => !string.IsNullOrWhiteSpace(w.Name))
            .ToList();

            Close();
        };

        layout.Children.Add(saveButton);

        Content = new ScrollView
        {
            Content = new VerticalStackLayout { Children = { layout } }
        };
    }

    private void AddWeaponRow(string name, string attackBonus, string damage, string damageType, string range, List<string> properties)
    {
        var nameEntry = new Entry { Text = name, Placeholder = "Weapon Name" };
        var attackEntry = new Entry { Text = attackBonus, Placeholder = "Attack Bonus" };
        var damageEntry = new Entry { Text = damage, Placeholder = "Damage" };
        var typeEntry = new Entry { Text = damageType, Placeholder = "Damage Type" };
        var rangeEntry = new Entry { Text = range, Placeholder = "Range" };
        var propsEditor = new Editor
        {
            Text = string.Join(", ", properties),
            Placeholder = "Properties (comma-separated)",
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
                new Label { Text = "Attack Bonus", FontAttributes = FontAttributes.Bold },
                attackEntry,
                new Label { Text = "Damage", FontAttributes = FontAttributes.Bold },
                damageEntry,
                new Label { Text = "Damage Type", FontAttributes = FontAttributes.Bold },
                typeEntry,
                new Label { Text = "Range", FontAttributes = FontAttributes.Bold },
                rangeEntry,
                new Label { Text = "Properties", FontAttributes = FontAttributes.Bold },
                propsEditor
            }
        };

        var weaponBlock = new VerticalStackLayout
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
            weaponEntryContainer.Children.Remove(weaponBlock);
            weaponRows.Remove(weaponBlock);
        };

        weaponRows.Add(weaponBlock);
        weaponEntryContainer.Children.Add(weaponBlock);
    }
}