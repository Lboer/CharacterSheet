using CommunityToolkit.Maui.Views;

namespace DnD_Character_Sheet.PopUps;

public class GenericValueEditPopup : Popup
{
    public GenericValueEditPopup(string label, int currentValue, Action<int> onSave)
    {
        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            WidthRequest = 300
        };

        layout.Children.Add(new Label
        {
            Text = $"Edit {label}",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            HorizontalOptions = LayoutOptions.Center
        });

        var entry = new Entry
        {
            Keyboard = Keyboard.Numeric,
            Text = currentValue.ToString(),
            Placeholder = "Enter new value"
        };
        layout.Children.Add(entry);

        var saveButton = new Button
        {
            Text = "Save",
            BackgroundColor = Colors.Green,
            TextColor = Colors.White
        };

        saveButton.Clicked += (s, e) =>
        {
            if (int.TryParse(entry.Text, out int newValue))
            {
                onSave?.Invoke(newValue);
                Close();
            }
            else
            {
                Application.Current.Windows[0].Page.DisplayAlertAsync("Invalid Input", "Please enter a valid number.", "OK");
            }
        };

        layout.Children.Add(saveButton);

        Content = layout;
    }

    public GenericValueEditPopup(string label, string currentValue, Action<string> onSave)
    {
        var layout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 10,
            WidthRequest = 300
        };

        layout.Children.Add(new Label
        {
            Text = $"Edit {label}",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            HorizontalOptions = LayoutOptions.Center
        });

        var entry = new Entry
        {
            Text = currentValue.ToString(),
            Placeholder = "Enter new value"
        };
        layout.Children.Add(entry);

        var saveButton = new Button
        {
            Text = "Save",
            BackgroundColor = Colors.Green,
            TextColor = Colors.White
        };

        saveButton.Clicked += (s, e) =>
        {
            onSave?.Invoke(entry.Text);
            Close();
        };

        layout.Children.Add(saveButton);

        Content = layout;
    }
}
