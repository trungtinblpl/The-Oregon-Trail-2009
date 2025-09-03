using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterNameEditor : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData {
        public string name;
        public GameObject characterPrefab;
        public bool isMale;
    }

    [Header("UI Elements")]
    public Transform characterSlot; // slot chính giữa (parent object)
    public TMP_InputField nameInput;
    public Button nextBTN;
    public Button prevBTN;

    [Header("Side Character Slots")]
    public List<Transform> sideCharacterSlots; // vị trí nhân vật phụ

    [Header("Character Data")]
    public List<CharacterData> characters = new List<CharacterData>();

    private int currenIndex = 0;
    private string originalName;
    public CharacterEditPanel characterEditPanel;

    public EditUIManager editUIManager;

    private GameObject currentCharacterObj;
    private List<GameObject> currentSideObjs = new List<GameObject>();

    public delegate void OnCharacterNamesChanged();
    public event OnCharacterNamesChanged NamesChanged;


    void Start() {
        nextBTN.onClick.AddListener(NextCharacter);
        prevBTN.onClick.AddListener(PrevCharacter);
        nameInput.onValueChanged.AddListener(OnNameChanged);

        ShowCharacter();
    }

    void ShowCharacter() {
        // xoá nhân vật chính giữa
        if (currentCharacterObj != null) Destroy(currentCharacterObj);

        // Tạo lại nhân vật chính giữa
        var data = characters[currenIndex];
        currentCharacterObj = Instantiate(data.characterPrefab, characterSlot);
        nameInput.text = data.name;

        // viền trắng
        Outline outline = currentCharacterObj.GetComponent<Outline>();
        if(outline != null){
            outline.enabled = true;
            outline.effectColor = Color.white;
            outline.effectDistance = new Vector2(2f, -2f);
        }

        // scale nhẹ khi được chọn
        currentCharacterObj.transform.localScale = Vector3.one * 1.05f;

        // xoá nhân vật phụ cũ
        foreach (var obj in currentSideObjs) Destroy(obj);
        currentSideObjs.Clear();

        int total = characters.Count;

        for (int i = 0; i < sideCharacterSlots.Count; i++) {
            int index = (currenIndex + i + 1) % total;
            GameObject sideObj = Instantiate(characters[index].characterPrefab, sideCharacterSlots[i]);

            // giảm scale cho nhân vật phụ
            sideObj.transform.localScale = Vector3.one * 0.9f;

            currentSideObjs.Add(sideObj);
        }
    }

    public void EditName(int index)
    {
        if (index >= 0 && index < characters.Count)
        {
            currenIndex = index;
            // Debug.Log($"[CharacterNameEditor] Set character by index: {index} - {characters[index].name}");
            originalName = characters[index].name;
            ShowCharacter();

            characterEditPanel.EnableClickToExitIfNoChange();
            //  Debug.Log("Click overtime");
        }
    }

    public void RandomizeAll(){
        foreach (var character in characters)
        {
            character.name = editUIManager.GenerateRandomName(character.isMale);
        }
        editUIManager.UpdateAllDialog();
    }

    void OnNameChanged(string newName){
        characters[currenIndex].name = newName;
        NamesChanged?.Invoke();
    }

    public List<CharacterData> GetAllCharacters(){
        return characters;
    }

    public bool IsNameChanged(){
        return characters[currenIndex].name != originalName;
    }

    void NextCharacter() {
        currenIndex = (currenIndex + 1) % characters.Count;
        ShowCharacter();
    }

    void PrevCharacter() {
        currenIndex = (currenIndex - 1 + characters.Count) % characters.Count;
        ShowCharacter();
    }
}
