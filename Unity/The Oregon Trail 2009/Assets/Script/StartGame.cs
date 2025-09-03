using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartGame : MonoBehaviour
{
    public CharacterNameEditor characterNameEditor;
    public List<TMP_Text> nameBubbles;

    void OnEnable()
    {
        UpdateAllDialog();
    }

    public void UpdateAllDialog()
    {
        var characters = characterNameEditor.GetAllCharacters();

        for (int i = 0; i < nameBubbles.Count && i < characters.Count; i++)
        {
            nameBubbles[i].text = characters[i].name;
        }
    }

}
