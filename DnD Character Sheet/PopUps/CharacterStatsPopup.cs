using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;

namespace DnD_Character_Sheet.PopUps;

public class CharacterStatsPopup : Popup
{
    public CharacterSheet Character { get; private set; }

    public CharacterStatsPopup(CharacterSheet character)
    {
        Character = character;

        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            WidthRequest = 300
        };

        layout.Children.Add(new Label { Text = "Armor Class", FontAttributes = FontAttributes.Bold });
        var acEntry = new Entry { Text = character.Combat.ArmorClass.ToString(), Keyboard = Keyboard.Numeric };
        layout.Children.Add(acEntry);

        layout.Children.Add(new Label { Text = "Initiative", FontAttributes = FontAttributes.Bold });
        var initiativeEntry = new Entry { Text = character.Combat.Initiative.ToString(),  };
        layout.Children.Add(initiativeEntry);

        layout.Children.Add(new Label { Text = "Speed", FontAttributes = FontAttributes.Bold });
        var speedEntry = new Entry { Text = character.Combat.Speed.ToString(), Keyboard = Keyboard.Numeric };
        layout.Children.Add(speedEntry);

        layout.Children.Add(new Label { Text = "Max HP", FontAttributes = FontAttributes.Bold });
        var maxHpEntry = new Entry { Text = character.Combat.HitPoints.Maximum.ToString(), Keyboard = Keyboard.Numeric };
        layout.Children.Add(maxHpEntry);

        layout.Children.Add(new Label { Text = "Current HP", FontAttributes = FontAttributes.Bold });
        var currentHpEntry = new Entry { Text = character.Combat.HitPoints.Current.ToString(), Keyboard = Keyboard.Numeric };
        layout.Children.Add(currentHpEntry);

        layout.Children.Add(new Label { Text = "Hit Dice", FontAttributes = FontAttributes.Bold });
        var hitDiceEntry = new Entry { Text = character.Combat.HitPoints.HitDice };
        layout.Children.Add(hitDiceEntry);

        var saveButton = new Button { Text = "Save", BackgroundColor = Colors.Purple, TextColor = Colors.White };
        saveButton.Clicked += (s, e) =>
        {
            character.Combat.ArmorClass = int.TryParse(acEntry.Text, out var ac) ? ac : 10;
            character.Combat.Initiative = int.TryParse(initiativeEntry.Text, out var init) ? init : 0;
            character.Combat.Speed = int.TryParse(speedEntry.Text, out var speed) ? speed : 30;
            character.Combat.HitPoints.Maximum = int.TryParse(maxHpEntry.Text, out var maxHp) ? maxHp : 1;
            character.Combat.HitPoints.Current = int.TryParse(currentHpEntry.Text, out var currHp) ? currHp : 1;
            character.Combat.HitPoints.HitDice = hitDiceEntry.Text;

            Close();
        };

        layout.Children.Add(saveButton);
        Content = layout;
    }
}
