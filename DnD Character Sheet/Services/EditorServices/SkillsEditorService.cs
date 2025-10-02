using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;

namespace DnD_Character_Sheet.Services.EditorServices;

public class SkillsEditorService
{
    private readonly Page _parentPage;

    public SkillsEditorService(Page parentPage)
    {
        _parentPage = parentPage;
    }

    public async Task ShowEditorAsync(CharacterSheet character)
    {
        var skillsstartPopup = new SkillsStartPopup(character);
        await _parentPage.ShowPopupAsync(skillsstartPopup);

        var skillsendPopup = new SkillsEndPopup(character);
        await _parentPage.ShowPopupAsync(skillsendPopup);
    }
}