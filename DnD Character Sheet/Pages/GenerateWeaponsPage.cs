using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Pages;

public static class GenerateWeaponsPage
{
    private static Grid _weaponsGrid;

    public static View GenerateWeaponsGrid(CharacterSheet character)
    {
        _weaponsGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star } }
        };

        // Title
        _weaponsGrid.Add(new Label
        {
            Text = "Weapons",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        }, 0, 0);

        // Header row
        var descriptionGrid = new Grid
        {
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        descriptionGrid.Add(new Label
        {
            Text = "Weapon",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 0);

        descriptionGrid.Add(new Label
        {
            Text = "Attack bonus",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 1, 0);

        descriptionGrid.Add(new Label
        {
            Text = "Damage",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 2, 0);

        _weaponsGrid.Add(descriptionGrid, 0, 1);

        AddWeaponRows(character.Weapons);

        // Edit button
        var editButton = new Button
        {
            Text = "Edit Weapons"
        };

        editButton.Clicked += (s, e) =>
        {
            var popup = new WeaponsPopup(character);
            popup.Closed += (sender, args) =>
            {
                RefreshWeaponsGrid(character);
            };

            Application.Current.MainPage.ShowPopup(popup);
        };

        _weaponsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        _weaponsGrid.Add(editButton, 0, _weaponsGrid.RowDefinitions.Count - 1);

        return new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = _weaponsGrid
        };
    }

    public static void RefreshWeaponsGrid(CharacterSheet updatedCharacter)
    {
        if (_weaponsGrid == null)
            return;

        // Remove all rows except title and header
        _weaponsGrid.Children.Clear();
        _weaponsGrid.RowDefinitions.Clear();

        _weaponsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        _weaponsGrid.Add(new Label
        {
            Text = "Weapons",
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10, 0, 10)
        }, 0, 0);

        _weaponsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var descriptionGrid = new Grid
        {
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        descriptionGrid.Add(new Label
        {
            Text = "Weapon",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 0);

        descriptionGrid.Add(new Label
        {
            Text = "Attack bonus",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 1, 0);

        descriptionGrid.Add(new Label
        {
            Text = "Damage",
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }, 2, 0);

        _weaponsGrid.Add(descriptionGrid, 0, 1);

        AddWeaponRows(updatedCharacter.Weapons);

        var editButton = new Button
        {
            Text = "Edit Weapons",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            Margin = new Thickness(0, 10, 0, 10)
        };

        editButton.Clicked += (s, e) =>
        {
            var popup = new WeaponsPopup(updatedCharacter);
            popup.Closed += (sender, args) =>
            {
                RefreshWeaponsGrid(updatedCharacter);
            };

            Application.Current.MainPage.ShowPopup(popup);
        };

        _weaponsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        _weaponsGrid.Add(editButton, 0, _weaponsGrid.RowDefinitions.Count - 1);
    }

    private static void AddWeaponRows(List<Weapon> weapons)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            _weaponsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var weapon = weapons[i];

            var innerGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            innerGrid.Add(new Label
            {
                Text = weapon.Name,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center
            }, 0, 0);

            innerGrid.Add(new Label
            {
                Text = weapon.AttackBonus,
                HorizontalTextAlignment = TextAlignment.Center
            }, 1, 0);

            innerGrid.Add(new Label
            {
                Text = weapon.Damage,
                HorizontalTextAlignment = TextAlignment.Center
            }, 2, 0);

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await Application.Current.MainPage.DisplayAlert(weapon.Name,
                    $"{weapon.DamageType} weapon.\nRange: {weapon.Range}\nProperties: {string.Join(", ", weapon.Properties)}", "OK");
            };
            innerGrid.GestureRecognizers.Add(tapGesture);

            _weaponsGrid.Add(innerGrid, 0, i + 2);
        }
    }
}

