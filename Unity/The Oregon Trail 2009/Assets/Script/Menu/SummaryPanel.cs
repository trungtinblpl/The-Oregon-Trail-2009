using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryPanel : MonoBehaviour
{
    [Header("Character Slot")]
    public Transform mainCharacterSlot;

    [Header("Oxen Slot")]
    public Transform OxenSlot;

    [Header("Weather Slot")]
    public Image weatherImage;

    void OnEnable()
    {
        // ===== Spawn Char =====
        if (GameManager.instance != null && GameManager.instance.selectedMainCharPrefab != null)
        {
            foreach (Transform child in mainCharacterSlot)
                Destroy(child.gameObject);

            var newChar = Instantiate(GameManager.instance.selectedMainCharPrefab, mainCharacterSlot);
            newChar.transform.localPosition = Vector3.zero;
            newChar.transform.localScale = Vector3.one;

            // Debug.Log($"[PartyName] Spawn {newChar.name}");
        } 
        
        // ===== Spawn Oxen =====
        if (GameManager.instance != null && GameManager.instance.selectedOxenPrefab != null)
        {
            foreach (Transform child in OxenSlot)
                Destroy(child.gameObject);

            var newOxen = Instantiate(GameManager.instance.selectedOxenPrefab, OxenSlot);
            newOxen.transform.localPosition = Vector3.zero;
            newOxen.transform.localScale = Vector3.one;

            // Debug.Log($"[PartyName] Spawn {newOxen.name}");
        }

        // ===== Spawn Weather =====
        if (GameManager.instance.selectedBackGround != null && weatherImage != null)
        {
            weatherImage.sprite = GameManager.instance.selectedBackGround;
            // Debug.Log($"[Summary] Set Weather Sprite: {GameManager.instance.selectedBackGround.name}");
        }
    }

}
