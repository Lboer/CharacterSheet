using DnD_Character_Sheet.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DnD_Character_Sheet;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    private async void OnLoadClicked(object sender, EventArgs e)
    {
        try
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/json" } },
                { DevicePlatform.iOS, new[] { "public.json" } },
                { DevicePlatform.WinUI, new[] { ".json" } },
                { DevicePlatform.MacCatalyst, new[] { "public.json" } },
                { DevicePlatform.Tizen, new[] { "application/json" } }
            });

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a character file",
                FileTypes = customFileType
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                var character = JsonSerializer.Deserialize<CharacterSheet>(json);

                if (character == null)
                {
                    await DisplayAlertAsync("Error", "Character is null.", "OK");
                    return;
                }
                var context = new ValidationContext(character);
                var results = new List<ValidationResult>();


                bool isValid = Validator.TryValidateObject(character, context, results, true);

                if (isValid)
                {
                    if (Shell.Current.CurrentPage is MainPage mainPage)
                    {
                        mainPage.LoadCharacter(character);
                    }
                }
                else
                {
                    string errors = string.Empty;
                    foreach (var error in results)
                    {
                        errors += error.ErrorMessage + "\n";
                    }
                    await DisplayAlertAsync("Error", errors, "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"File selection failed: {ex.Message}", "OK");
        }
    }


    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (Shell.Current.CurrentPage is MainPage mainPage)
        {
            if (mainPage.CurrentCharacter != null)
            {
                await mainPage.SaveCharacterToDownloadsAsync();
            }
        }
    }

    private async void OnNewClicked(object sender, EventArgs e)
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("Blank.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var character = JsonSerializer.Deserialize<CharacterSheet>(json);

            if (character != null)
            {
                if (Shell.Current.CurrentPage is MainPage mainPage)
                {
                    mainPage.CreateCharacter(character);
                }
            }
            else
            {
                await DisplayAlertAsync("Error", "Failed to load blank character.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Could not load blank character: {ex.Message}", "OK");
        }
    }
}
