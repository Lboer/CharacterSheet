using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Services.EditorServices;

public class FeatureEditorService
{
    private readonly Page _parentPage;

    public FeatureEditorService(Page parentPage)
    {
        _parentPage = parentPage;
    }

    public async Task ShowEditorAsync(CharacterSheet character)
    {
        var featurePopup = new FeaturesPopup(character);
        await _parentPage.ShowPopupAsync(featurePopup);

        var languagePopup = new LanguagesPopup(character);
        await _parentPage.ShowPopupAsync(languagePopup);
    }
}
