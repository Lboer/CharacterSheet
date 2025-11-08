using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class WeaponsPageBuilder
{
    private static Grid _weaponsGrid;

    public static View Build(CharacterSheet character)
    {
        _weaponsGrid = CreateWeaponsGrid();
        PopulateWeaponsGrid(character);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _weaponsGrid
        };
    }

    public static void Refresh(CharacterSheet updatedCharacter)
    {
        if (_weaponsGrid == null)
            return;

        _weaponsGrid.Children.Clear();
        PopulateWeaponsGrid(updatedCharacter);
    }

    private static Grid CreateWeaponsGrid()
    {
        return new Grid
        {
            Padding = new Thickness(10),
            RowSpacing = 15,
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }, // Title
                new RowDefinition { Height = GridLength.Auto }  // Header
                // Weapon rows and edit button will be added dynamically
            }
        };
    }

    private static void PopulateWeaponsGrid(CharacterSheet character)
    {
        var titleLabel = new Label
        {
            Text = "Weapons",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        };

        var headerGrid = BuildHeaderRow();
        var editButton = BuildEditButton(character);

        _weaponsGrid.Add(titleLabel, 0, 0);
        _weaponsGrid.Add(headerGrid, 0, 1);

        AddWeaponRows(character.Weapons);

        _weaponsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        _weaponsGrid.Add(editButton, 0, _weaponsGrid.RowDefinitions.Count - 1);
    }

    private static Grid BuildHeaderRow()
    {
        var grid = new Grid
        {
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        grid.Add(new Label
        {
            Text = "Weapon",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 0);

        grid.Add(new Label
        {
            Text = "Attack bonus",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 1, 0);

        grid.Add(new Label
        {
            Text = "Damage",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 2, 0);

        return grid;
    }

    private static void AddWeaponRows(List<Weapon> weapons)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            _weaponsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var weapon = weapons[i];
            var rowGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            rowGrid.Add(new Label
            {
                Text = weapon.Name,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center
            }, 0, 0);

            rowGrid.Add(new Label
            {
                Text = weapon.AttackBonus,
                HorizontalTextAlignment = TextAlignment.Center
            }, 1, 0);

            rowGrid.Add(new Label
            {
                Text = weapon.Damage,
                HorizontalTextAlignment = TextAlignment.Center
            }, 2, 0);

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                if (Application.Current?.MainPage is not null)
                {
                    var details = $"{weapon.DamageType} weapon.\nRange: {weapon.Range}\nProperties: {string.Join(", ", weapon.Properties)}";
                    await Application.Current.MainPage.DisplayAlert(weapon.Name, details, "OK");
                }
            };
            rowGrid.GestureRecognizers.Add(tapGesture);

            _weaponsGrid.Add(rowGrid, 0, i + 2);
        }
    }

    private static Button BuildEditButton(CharacterSheet character)
    {
        var button = new Button
        {
            Text = "Edit Weapons"
        };

        button.Clicked += (s, e) =>
        {
            var popup = new WeaponsPopup(character);
            popup.Closed += (_, __) => Refresh(character);
            Application.Current.MainPage?.ShowPopup(popup);
        };

        return button;
    }
}


