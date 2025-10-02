using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class InventoryPopup : Popup
{
    public CharacterSheet Character { get; private set; }
    private List<HorizontalStackLayout> inventoryRows = new();
    private VerticalStackLayout inventoryEntryContainer;

    public InventoryPopup(CharacterSheet character)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            WidthRequest = 320
        };

        // Description
        layout.Children.Add(new Label
        {
            Text = "Your inventory contains all the gear, tools, and miscellaneous items your character carries. Add or remove items below.",
            FontSize = 14,
            TextColor = Colors.Gray
        });

        // Section header
        layout.Children.Add(new Label
        {
            Text = "Inventory",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            Margin = new Thickness(0, 10, 0, 0)
        });

        // Container for entries
        inventoryEntryContainer = new VerticalStackLayout { Spacing = 5 };
        layout.Children.Add(inventoryEntryContainer);

        // Add existing inventory items
        foreach (var item in character.Equipment)
        {
            AddInventoryRow(item);
        }

        // Add button
        var addButton = new Button
        {
            Text = "Add Item",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black
        };

        addButton.Clicked += (s, e) => AddInventoryRow("");

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
            Character.Equipment = inventoryRows
                .Select(row => row.Children.OfType<Entry>().FirstOrDefault()?.Text?.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .Distinct()
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

    private void AddInventoryRow(string initialText)
    {
        var entry = new Entry { Text = initialText, Placeholder = "Item name" };
        var removeButton = new Button
        {
            Text = "❌",
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Red,
            WidthRequest = 40
        };

        var row = new HorizontalStackLayout
        {
            Spacing = 5,
            Children = { entry, removeButton }
        };

        removeButton.Clicked += (s, e) =>
        {
            inventoryEntryContainer.Children.Remove(row);
            inventoryRows.Remove(row);
        };

        inventoryRows.Add(row);
        inventoryEntryContainer.Children.Add(row);
    }
}

