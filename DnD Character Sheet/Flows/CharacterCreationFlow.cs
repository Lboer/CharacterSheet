using CommunityToolkit.Maui.Views;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.PopUps;
using DnD_Character_Sheet.Services.EditorServices;

namespace DnD_Character_Sheet.Flows;

public class CharacterCreationFlow
{
    private readonly Page _parentPage;

    public CharacterCreationFlow(Page parentPage) => _parentPage = parentPage;

    public async Task RunAsync(CharacterSheet character)
    {
        await new CharacterEditorService(_parentPage).ShowEditorAsync(character);
        await new SkillsEditorService(_parentPage).ShowEditorAsync(character);
        await new FeatureEditorService(_parentPage).ShowEditorAsync(character);
        await _parentPage.ShowPopupAsync(new WeaponsPopup(character));
        await _parentPage.ShowPopupAsync(new InventoryPopup(character));
        await _parentPage.ShowPopupAsync(new BackstoryPopup(character));
        await new SpellcastingEditorService(_parentPage).ShowEditorAsync(character);
    }
}