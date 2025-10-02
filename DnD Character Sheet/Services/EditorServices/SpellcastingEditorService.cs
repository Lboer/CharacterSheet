using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Services.EditorServices;

public class SpellcastingEditorService
{
    private readonly Page _parentPage;

    public SpellcastingEditorService(Page parentPage)
    {
        _parentPage = parentPage;
    }

    public async Task ShowEditorAsync(CharacterSheet character)
    {
        var spellcastingstartPopup = new SpellcastingStartPopup(character);
        await _parentPage.ShowPopupAsync(spellcastingstartPopup);

        var spellcastingendPopup = new SpellcastingEndPopup(character);
        await _parentPage.ShowPopupAsync(spellcastingendPopup);
    }
}