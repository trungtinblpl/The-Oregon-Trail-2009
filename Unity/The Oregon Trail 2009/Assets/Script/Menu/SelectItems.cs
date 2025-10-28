// using System.Collections.Generic;
// using UnityEngine;

// public enum SelectType { Character, Oxen, Weather, Other }

// public class SelectItems : MonoBehaviour
// {
//     [Header("General Settings")]
//     public SelectType selectType;  // chọn loại (Character / Oxen / Weather)

//     [Header("Panels")]
//     public GameObject infoPanel;        
//     public List<string> itemNames;      
//     public List<GameObject> itemPanels; 
//     public GameObject nextPanel;        

//     private GameObject currentInfoPanel;
//     private Dictionary<string, GameObject> infoPanels = new Dictionary<string, GameObject>();

//     private Menu menu; 

//     [Header("Character Prefabs")] 
//     public GameObject bankerPrefab;
//     public GameObject farmerPrefab;
//     public GameObject carpenterPrefab;

//     [Header("Oxen Prefabs")] 
//     public GameObject conestoga_Prefab;
//     public GameObject prairie_Prefab;
//     public GameObject basic_Prefab;

//     [Header("Weather Sprites")]
//     public Sprite march_Sprite;
//     public Sprite april_Sprite;
//     public Sprite may_Sprite;


//     void Start()
//     {
//         menu = FindObjectOfType<Menu>();

//         for (int i = 0; i < itemNames.Count && i < itemPanels.Count; i++)
//         {
//             infoPanels[itemNames[i]] = itemPanels[i];
//         }
//     }
    
//     public void OnSelectItem(string itemName)
//     {
//         if (menu == null) return;

//         foreach (var panel in infoPanels.Values)
//             panel.SetActive(false);

//         if (infoPanels.TryGetValue(itemName, out var targetPanel))
//         {
//             targetPanel.SetActive(true);
//             currentInfoPanel = targetPanel;
//         }

//         if (infoPanel != null)
//             menu.ShowInfoSub(itemName, infoPanel.name);  

//         // ✅ Nếu là panel chọn nhân vật, thì lưu vào GameManager
//         if (selectType == SelectType.Character && GameManager.instance != null)
//         {
//             GameObject prefabToUse = null;
//             switch (itemName)
//             {
//                 case "Banker": prefabToUse = bankerPrefab; break;
//                 case "Farmer": prefabToUse = farmerPrefab; break;
//                 case "Carpenter": prefabToUse = carpenterPrefab; break;
//             }

//             if (prefabToUse != null)
//             {
//                 GameManager.instance.SetMainCharacter(prefabToUse);
//                 // Debug.Log($"[SelectItems] Saved {itemName} prefab to GameManager");
//             }
//         }

//         // ✅ Nếu là Oxen thì sau này có thể gọi GameManager.SetOxen(...)
//         if (selectType == SelectType.Oxen && GameManager.instance != null)
//         {
//             GameObject prefabToUse = null;
//             switch (itemName)
//             {
//                 case "Basic Wagon": prefabToUse = basic_Prefab; break;
//                 case "Prairie Schooner": prefabToUse = prairie_Prefab; break;
//                 case "Conestoga Wagon": prefabToUse = conestoga_Prefab; break;
//             }

//             if (prefabToUse != null)
//             {
//                 GameManager.instance.SetOxen(prefabToUse);
//                 // Debug.Log($"[SelectItems] Saved {itemName} prefab to GameManager");
//             }
//         }

//         // ✅ Nếu là Weather thì gọi GameManager.SetWeather(...)
//         if (selectType == SelectType.Weather && GameManager.instance != null)
//         {
//             Sprite spriteToUse = null;
//             switch (itemName)
//             {
//                 case "March": spriteToUse = march_Sprite; break;   // sửa thành Sprite
//                 case "April": spriteToUse = april_Sprite; break;
//                 case "May": spriteToUse = may_Sprite; break;
//             }

//             if (spriteToUse != null)
//             {
//                 GameManager.instance.SetWeather(spriteToUse);
//                 Debug.Log($"[SelectItems] Saved {itemName} sprite to GameManager");
//             }
//         }
//     }

//     public void OnConfirm()
//     {
//         if (menu == null) return;

//         if (currentInfoPanel != null)
//         {
//             currentInfoPanel.SetActive(false);
//             currentInfoPanel = null;
//         }

//         if (nextPanel != null)
//             menu.ShowPanel(nextPanel.name);
//     }
// }

using System.Collections.Generic;
using UnityEngine;

public enum SelectType { Character, Oxen, Weather, Other }

[System.Serializable]
public class NamedPrefab
{
    public string name;        // ví dụ "Banker", "Farmer", "Carpenter"
    public GameObject prefab;
}

[System.Serializable]
public class NamedSprite
{
    public string name;        // ví dụ "March", "April", "May"
    public Sprite sprite;
}

public class SelectItems : MonoBehaviour
{
    [Header("General Settings")]
    public SelectType selectType;  // Character / Oxen / Weather

    [Header("Panels")]
    public GameObject infoPanel;
    public List<string> itemNames;      
    public List<GameObject> itemPanels;
    public GameObject nextPanel;

    private GameObject currentInfoPanel;
    private Dictionary<string, GameObject> infoPanels = new Dictionary<string, GameObject>();
    private Menu menu;

    [Header("Characters")]
    public List<NamedPrefab> characterPrefabs;

    [Header("Oxens")]
    public List<NamedPrefab> oxenPrefabs;

    [Header("Weather")]
    public List<NamedSprite> weatherSprites;


    void Start()
    {
        menu = FindObjectOfType<Menu>();

        for (int i = 0; i < itemNames.Count && i < itemPanels.Count; i++)
        {
            infoPanels[itemNames[i]] = itemPanels[i];
        }
    }

    public void OnSelectItem(string itemName)
    {
        if (menu == null) return;

        // Ẩn toàn bộ sub-panel
        foreach (var panel in infoPanels.Values)
            panel.SetActive(false);

        // Hiện panel tương ứng
        if (infoPanels.TryGetValue(itemName, out var targetPanel))
        {
            targetPanel.SetActive(true);
            currentInfoPanel = targetPanel;
        }

        // Gọi panel Info
        if (infoPanel != null)
            menu.ShowInfoSub(itemName, infoPanel.name);

        // === Lưu vào GameManager theo loại ===
        switch (selectType)
        {
            case SelectType.Character:
                var charPrefab = GetPrefabByName(characterPrefabs, itemName);
                if (charPrefab != null)
                {
                    GameManager.instance.SetMainCharacter(charPrefab);
                    Debug.Log($"[SelectItems] Saved Character {itemName}");
                }
                break;

            case SelectType.Oxen:
                var oxenPrefab = GetPrefabByName(oxenPrefabs, itemName);
                if (oxenPrefab != null)
                {
                    GameManager.instance.SetOxen(oxenPrefab);
                    Debug.Log($"[SelectItems] Saved Oxen {itemName}");
                }
                break;

            case SelectType.Weather:
                var sprite = GetSpriteByName(weatherSprites, itemName);
                if (sprite != null)
                {
                    GameManager.instance.SetWeather(sprite);
                    Debug.Log($"[SelectItems] Saved Weather {itemName}");
                }
                break;
        }
    }

    public void OnConfirm()
    {
        if (menu == null) return;

        if (currentInfoPanel != null)
        {
            currentInfoPanel.SetActive(false);
            currentInfoPanel = null;
        }

        if (nextPanel != null)
            menu.ShowPanel(nextPanel.name);
    }

    // ==== Helpers ====
    private GameObject GetPrefabByName(List<NamedPrefab> list, string itemName)
    {
        foreach (var entry in list)
        {
            if (entry.name == itemName)
                return entry.prefab;
        }
        return null;
    }

    private Sprite GetSpriteByName(List<NamedSprite> list, string itemName)
    {
        foreach (var entry in list)
        {
            if (entry.name == itemName)
                return entry.sprite;
        }
        return null;
    }
}
