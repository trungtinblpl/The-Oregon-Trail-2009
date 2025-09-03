using UnityEngine;

public class StoryScroll : MonoBehaviour
{
    public RectTransform storyGroup;
    public float speed = 50f;
    public float stopRoll = 190f;

    public GameObject tapToContinueText;
    public GameObject storyPanel;           // panel cốt truyện
    public GameObject characterSelectPanel; // panel chọn nhân vật

    private bool isScrolling = true;
    private Menu menu; // thêm reference tới Menu
    private Vector2 startPos;

    void Start()
    {
        startPos = storyGroup.anchoredPosition; // lưu lại vị trí ban đầu (-298, 0)
        if (tapToContinueText != null)
            tapToContinueText.SetActive(true);

        menu = FindObjectOfType<Menu>();
    }

    void Update()
    {
        if (isScrolling)
        {
            if (storyGroup.anchoredPosition.y < stopRoll)
            {
                storyGroup.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
                if (storyGroup.anchoredPosition.y >= stopRoll)
                {
                    storyGroup.anchoredPosition = new Vector2(0, stopRoll);
                    isScrolling = false; // tự dừng khi tới điểm cuối
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isScrolling = false;
            ShowCharacterSelect();
        }
    }


    void ShowCharacterSelect()
    {
        if (menu == null)
        {
            // fallback nếu chưa gắn Menu
            if (storyPanel != null) storyPanel.SetActive(false);
            if (characterSelectPanel != null) characterSelectPanel.SetActive(true);
            return;
        }

        // Đóng storyPanel và mở characterSelectPanel qua Menu để lưu vào stack
        menu.ShowPanel(characterSelectPanel.name);

    }

    public void ResetScroll()
    {
        isScrolling = true;
        storyGroup.anchoredPosition = startPos; // reset về vị trí gốc
        // Debug.Log("[StoryScroll] Reset về: " + startPos);

    }

}
