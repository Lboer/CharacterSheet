using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class GenerateInventoryPage
{
    private static VerticalStackLayout _inventoryRoot;
    public static View GenerateInventoryLayout(CharacterSheet character)
    {
        _inventoryRoot = new VerticalStackLayout
        {
            Padding = 10,
            Spacing = 10
        };

        _inventoryRoot.Children.Add(new Label
        {
            Text = "Inventory",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        });

        foreach (var item in character.Equipment)
        {
            _inventoryRoot.Children.Add(new Label
            {
                Text = item,
                FontSize = 16
            });
        }

        var editButton = new Button
        {
            Text = "Edit Inventory"
        };

        editButton.Clicked += (s, e) =>
        {
            var popup = new InventoryPopup(character);
            popup.Closed += (sender, args) =>
            {
                RefreshInventoryLayout(character);
            };

            Application.Current.MainPage.ShowPopup(popup);
        };

        _inventoryRoot.Children.Add(editButton);

        return _inventoryRoot;
    }

    public static void RefreshInventoryLayout(CharacterSheet updatedCharacter)
    {
        if (_inventoryRoot == null)
            return;

        // Remove all except the edit button
        var editButton = _inventoryRoot.Children.OfType<Button>().FirstOrDefault();
        _inventoryRoot.Children.Clear();

        foreach (var item in updatedCharacter.Equipment)
        {
            _inventoryRoot.Children.Add(new Label
            {
                Text = item,
                FontSize = 16
            });
        }

        if (editButton != null)
            _inventoryRoot.Children.Add(editButton);
    }
}
