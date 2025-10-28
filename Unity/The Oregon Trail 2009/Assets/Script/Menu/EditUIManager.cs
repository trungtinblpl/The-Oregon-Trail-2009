using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditUIManager : MonoBehaviour
{
    public GameObject BtnYN;
    public GameObject BtnRF;
    public GameObject BtnDialog;

    public GameObject noteText;
    public GameObject noteTextChange;

    public CharacterNameEditor characterNameEditor;

    [Header("Main Character Slot")]
    public Transform mainCharacterSlot;

    [Header("Text Bubbles")]
    public List<TMP_Text> nameBubbles;

    public void OnYes(){

        noteText.SetActive(false);
        noteTextChange.SetActive(true);

        BtnYN.SetActive(false);
        BtnRF.SetActive(true);
        BtnDialog.SetActive(true);
    }

    public void UpdateAllDialog(){
        List<CharacterNameEditor.CharacterData> characters = 
        characterNameEditor.GetAllCharacters();

        for(int i = 0; i < nameBubbles.Count && i < characters.Count; i++){
            nameBubbles[i].text = characters[i].name;
        }
    }

    List<string> maleNames = new List<string> {
    "James", "John", "William", "George", "Charles",
    "Henry", "Joseph", "Thomas", "Edward", "Samuel",
    "David", "Benjamin", "Daniel", "Isaac", "Robert"
    };

    List<string> femaleNames = new List<string> {
        "Mary", "Elizabeth", "Sarah", "Margaret", "Emma",
        "Nancy", "Martha", "Jane", "Catherine", "Ann",
        "Caroline", "Susan", "Emily", "Julia", "Louisa"
    };


    public string GenerateRandomName(bool isMale){

        if (isMale)
        {
            return maleNames[Random.Range(0, maleNames.Count)];
        }
        else
        {
            return femaleNames[Random.Range(0, femaleNames.Count)];
        }
    }

    void OnEnable(){
        if(mainCharacterSlot != null && GameManager.instance != null 
        && GameManager.instance.selectedMainCharPrefab != null){
            foreach (Transform child in mainCharacterSlot){
                Destroy(child.gameObject);
            }
        }

        var newChar = Instantiate(GameManager.instance.selectedMainCharPrefab, mainCharacterSlot);
        newChar.transform.localPosition = Vector3.zero;
        newChar.transform.localScale = Vector3.one;

        Debug.Log($"[EditUIManager] Spawned {newChar.name} in PartyName panel");

        UpdateAllDialog();
    }
}
