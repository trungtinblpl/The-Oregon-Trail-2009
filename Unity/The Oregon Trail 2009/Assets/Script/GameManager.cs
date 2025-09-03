using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Character")]
    public GameObject selectedMainCharPrefab;
    public CharacterData selectedCharacter;

    [Header("Oxen")]
    public GameObject selectedOxenPrefab;
    public List<GameObject> allOxenPrefab;

    [Header("Weather")]
    public Sprite selectedBackGround;
    public List<Sprite> allBackgrounds; 

    public void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Debug.Log("[GameManager] Created singleton instance");
        } else {
            Destroy(gameObject);
            //  Debug.Log("[GameManager] Duplicate destroyed");
        }
    }

    // =============== CHARACTER ==================
    public void SetCharacter(CharacterData data){
        selectedCharacter = data;
        // Debug.Log($"[GameManager] Selected {data.displayName}");
    }

    public void SetMainCharacter(GameObject prefab){
        selectedMainCharPrefab = prefab;
        // Debug.Log($"[GameManager] Main character selected = {prefab.name}");
    }

    // =============== OXEN ==================
    public void SetOxen(GameObject oxenPrefab){
        selectedOxenPrefab = oxenPrefab;
    //     // Debug.Log($"[GameManager] Oxen selected = {oxenPrefab.name}");
    }

    // =============== BACKGROUND ==================
    public void SetWeather(Sprite bg){
        selectedBackGround = bg;
        Debug.Log($"[GameManager]  Background selected = {bg.name}");
    }
}