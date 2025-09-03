using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [System.Serializable]
    public class UIState
    {
        public GameObject panel;
        public string subName; // null nếu panel không có sub
    }

    [Header("UI Panels")]
    [SerializeField] private GameObject uiStartPanel;     // panel đầu tiên
    [SerializeField] private List<GameObject> uiScreens;  // danh sách tất cả panel (MainMenu, Character, Oxen, ...)

    [Header("Info Panels")]
    [SerializeField] private List<GameObject> infoScreens; // danh sách riêng cho Info (Info_Character, Info_Oxen, ...)

    private Stack<UIState> uiHistory = new Stack<UIState>();

    private void Start()
    {
        if (uiStartPanel != null)
        {
            ShowPanel(new UIState { panel = uiStartPanel, subName = null });
        }
    }

    // Hiển thị panel thường (không có sub)
    public void ShowPanel(GameObject newUI)
    {
        ShowPanel(new UIState { panel = newUI, subName = null });
    }

    // Hiển thị panel Info theo subName
    public void ShowInfoSub(string subName, string infoPanelName)
    {
        // Lấy đúng info panel (Info_char, Info_oxen, ...)
        GameObject infoPanel = uiScreens.Find(x => x.name == infoPanelName);
        if (infoPanel == null)
        {
            // Debug.LogError($"Không tìm thấy panel {infoPanelName}");
            return;
        }

        infoPanel.SetActive(true);

        var controller = infoPanel.GetComponent<InfoController>();
        if (controller != null)
        {
            controller.ShowInfo(subName);
            // Debug.Log($"[SelectItems] Show {subName} trong {infoPanelName}");
        }

        // Lưu cả subName vào stack
        ShowPanel(new UIState { panel = infoPanel, subName = subName });
    }


    // Gọi panel theo tên
    public void ShowPanel(string panelName)
    {
        GameObject target = uiScreens.Find(x => x != null && x.name == panelName);
        if (target == null)
        {
            // Debug.LogWarning($"[Menu] Không tìm thấy panel: {panelName}");
            return;
        }

        ShowPanel(new UIState { panel = target, subName = null });
    }

    // Hiển thị UIState
    private void ShowPanel(UIState newState)
    {
        if (newState == null || newState.panel == null) return;

        if (uiHistory.Count > 0)
        {
            UIState current = uiHistory.Peek();
            if (current.panel != newState.panel)
            {
                current.panel.SetActive(false);
            }
        }

        // Bật panel mới
        newState.panel.SetActive(true);

        var scroll = newState.panel.GetComponent<StoryScroll>();
        if (scroll != null) scroll.ResetScroll();

        // Push nếu khác panel hiện tại hoặc khác subName
        if (uiHistory.Count == 0 || uiHistory.Peek().panel != newState.panel || uiHistory.Peek().subName != newState.subName)
        {
            uiHistory.Push(newState);
        }

        ApplyInfo(newState);

        // Debug.Log($"[Menu] Show: {newState.panel.name} ({newState.subName}) | Stack: {StackToString()}");
    }

    // Quay lại
    public void BackUI()
    {
        if (uiHistory.Count > 1)
        {
            UIState closed = uiHistory.Pop();
            closed.panel.SetActive(false);

            UIState reopened = uiHistory.Peek();
            reopened.panel.SetActive(true);

            // Quan trọng: gọi lại ApplyInfo để show đúng subName cũ
            ApplyInfo(reopened);

            var scroll = reopened.panel.GetComponentInChildren<StoryScroll>(true);
            if (scroll != null)
            {
                scroll.ResetScroll();
            //     Debug.Log("[Menu] ResetScroll chạy OK trên: " + scroll.gameObject.name);
            // }
            // else
            // {
            //     Debug.Log("[Menu] Không tìm thấy StoryScroll trong " + reopened.panel.name);
            }

        //     Debug.Log($"[Menu] Back: {reopened.panel.name} ({reopened.subName}) | Stack: {StackToString()}");
        // }
        // else
        // {
        //     Debug.Log("[Menu] Đang ở Start Panel, không back được.");
        }
    }

    private void ApplyInfo(UIState state)
    {
        if (state.panel != null && !string.IsNullOrEmpty(state.subName))
        {
            var ctrl = state.panel.GetComponent<InfoController>();
            if (ctrl != null)
            {
                ctrl.ShowInfo(state.subName);
                // Debug.Log($"[Menu] ApplyInfo: {state.panel.name} -> {state.subName}");
            }
        }
    }

    public void ResetUI(){
        foreach (var ui in uiScreens)
            ui.SetActive(false);
        
        uiHistory.Clear();

        GameObject characterPanel = uiScreens.Find(x => x.name == "Character");

        if(characterPanel == null){
            // Debug.LogWarning("Character panel not found!");
            return;
        }
        
        ShowPanel(characterPanel);
        // Debug.Log("back character ui!");
    }


    // Debug stack
    private string StackToString()
    {
        List<string> names = new List<string>();
        foreach (var state in uiHistory)
        {
            if (state != null && state.panel != null)
                names.Add(state.panel.name + (string.IsNullOrEmpty(state.subName) ? "" : $"[{state.subName}]"));
        }
        return string.Join(" -> ", names);
    }

    public void Finish()
    {
        ShowPanel("Oxen");
    }
}
