using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Services.EditorServices;

public class CharacterEditorService
{
    private readonly Page _parentPage;

    public CharacterEditorService(Page parentPage)
    {
        _parentPage = parentPage;
    }

    public async Task ShowEditorAsync(CharacterSheet character)
    {
        var infoPopup = new CharacterInfoPopup(character);
        await _parentPage.ShowPopupAsync(infoPopup);

        var statsPopup = new CharacterStatsPopup(character);
        await _parentPage.ShowPopupAsync(statsPopup);

        var abilityPopup = new AbilityScoresPopup(character);
        await _parentPage.ShowPopupAsync(abilityPopup);

        var savingPopup = new SavingThrowsPopup(character);
        await _parentPage.ShowPopupAsync(savingPopup);
    }
}
