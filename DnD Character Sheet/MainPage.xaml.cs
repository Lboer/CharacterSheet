using DnD_Character_Sheet.Flows;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.Services;
using System.Text.Json;
#if ANDROID
using Android.OS;
using Android.App;
#endif

namespace DnD_Character_Sheet;

public partial class MainPage : ContentPage
{
    public MainPage() => InitializeComponent();
    public CharacterSheet CurrentCharacter;

    public void LoadCharacter(CharacterSheet character)
    {
        WelcomeLabel.IsVisible = false;
        CharacterContent.Content = null;
        CharacterContent.Content = GenerateCarouselViewService.LoadCharacterIntoView(character);
        CurrentCharacter = character;
    }

    public async void CreateCharacter(CharacterSheet character)
    {
        CurrentCharacter = character;

        var characterCreationWizard = new CharacterCreationFlow(this);
        await characterCreationWizard.RunAsync(CurrentCharacter);

        LoadCharacter(CurrentCharacter);
    }

    public async Task SaveCharacterToDownloadsAsync()
    {
        var permissionService = new PermissionService();

        // guard clause
        if (Microsoft.Maui.Controls.Application.Current?.MainPage is not null)
        {
            if (!await permissionService.EnsureStoragePermissionAsync())
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Permission Denied", "Storage permission is required to save the file.", "OK");
                return;
            }

            string fileName = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayPromptAsync(
                "Save Character",
                "Enter a name for your character file:",
                placeholder: "e.g. Elowen.json"
            );

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            if (!fileName.EndsWith(".json"))
                fileName += ".json";

            string json = JsonSerializer.Serialize(CurrentCharacter, new JsonSerializerOptions
            {
                WriteIndented = true
            });

#if ANDROID
    // Get public Downloads directory
    var downloadsDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
    string filePath = Path.Combine(downloadsDir.AbsolutePath, fileName);
#else
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
#endif

            File.WriteAllText(filePath, json);

            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Saved", $"Character saved to:\n{filePath}", "OK");
        }

        else
        {
            Console.WriteLine("Application Mainpage is null");
        }
    }
}
