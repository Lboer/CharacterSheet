using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class InventoryPageBuilder
{
    private static Grid _inventoryGrid;

    public static View Build(CharacterSheet character)
    {
        _inventoryGrid = CreateInventoryGrid();
        PopulateInventoryGrid(character);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _inventoryGrid
        };
    }

    public static void Refresh(CharacterSheet updatedCharacter)
    {
        if (_inventoryGrid == null)
            return;

        _inventoryGrid.Children.Clear();
        PopulateInventoryGrid(updatedCharacter);
    }

    private static Grid CreateInventoryGrid()
    {
        return new Grid
        {
            Padding = new Thickness(10),
            RowSpacing = 15,
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Header
                new RowDefinition { Height = GridLength.Auto }, // Items
                new RowDefinition { Height = GridLength.Auto }  // Edit Button
            }
        };
    }

    private static void PopulateInventoryGrid(CharacterSheet character)
    {
        var headerLabel = new Label
        {
            Text = "Inventory",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        };

        var itemsGrid = BuildItemList(character.Equipment);
        var editButton = BuildEditButton(character);

        _inventoryGrid.Add(headerLabel, 0, 0);
        _inventoryGrid.Add(itemsGrid, 0, 1);
        _inventoryGrid.Add(editButton, 0, 2);
    }

    private static Grid BuildItemList(List<string> items)
    {
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

        for (int row = 0; row < items.Count; row++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Add(new Label
            {
                Text = items[row],
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center
            }, 0, row);
        }

        return grid;
    }

    private static Button BuildEditButton(CharacterSheet character)
    {
        var button = new Button
        {
            Text = "Edit Inventory"
        };

        button.Clicked += (s, e) =>
        {
            var popup = new InventoryPopup(character);
            popup.Closed += (_, __) => Refresh(character);
            Application.Current.MainPage?.ShowPopup(popup);
        };

        return button;
    }
}