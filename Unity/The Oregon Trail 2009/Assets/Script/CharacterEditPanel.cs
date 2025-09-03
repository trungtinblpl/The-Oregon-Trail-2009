using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterEditPanel : MonoBehaviour
{
    public CharacterNameEditor characterNameEditor;

    public GameObject EditNamePanel;
    public GameObject PartyNamePanel;

    public RectTransform area;

    public EditUIManager bubbleUpdater;

    private bool waitingForClick = false;

    public void EnableClickToExitIfNoChange()
    {
        if (!characterNameEditor.IsNameChanged())
        {
            // Debug.Log("Tên không thay đổi");
            waitingForClick = true;
        }
        else
        {
            // Nếu tên có thay đổi, có thể xử lý logic khác tại đây nếu cần
            
            // Debug.Log("Tên đã thay đổi");
            waitingForClick = false;
        }
    }

    void Update()
    {
        if (!waitingForClick) return; 

        if (Input.GetMouseButtonDown(0)) {
            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                area, Input.mousePosition, null, out localMousePos
            );

            if(area.rect.Contains(localMousePos)){
                // Debug.Log("Click vào vùng Edit UI");
                return;
            }

            // Debug.Log("Thoát EditPanel");
            EditNamePanel.SetActive(false);
            PartyNamePanel.SetActive(true);

            bubbleUpdater.UpdateAllDialog();

            waitingForClick = false;
        }

    }   
}
