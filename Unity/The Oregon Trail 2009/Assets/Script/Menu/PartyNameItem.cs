using UnityEngine;

public class PartyNameItem : MonoBehaviour
{
    public int characterIndex;

    // public CharacterEditPanel characterEditPanel;

    [Header("References")]
    public CharacterNameEditor characterNameEditor;
    public GameObject editPanel;
    public GameObject partyNamePanel;

    public void OnItemClicked()
    {
        // Debug.Log($"[PartyNameItem] Clicked index: {characterIndex}");
        characterNameEditor.EditName(characterIndex);
        editPanel.SetActive(true);
        partyNamePanel.SetActive(false);

        // Debug.Log("Click overtime");
    }
}
