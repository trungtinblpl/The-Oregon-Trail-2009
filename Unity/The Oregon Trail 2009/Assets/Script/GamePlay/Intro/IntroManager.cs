using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector introTimeline;
    public GameObject npc;
    public GameObject functionUI;
    public GameObject settingUI;
    public GameObject[] roll;
    public GameObject mainDialog;
    public TMP_Text mainText;
    public GameObject secondaryDialog;
    public TMP_Text secondaryText;
    public GameObject toughUI;
    public GameObject[] arrows;
    public GameObject food;
    public GameObject money;

    [Header("Settings")]
    public float npcDelayAfterTimeline = 2.8f;
    [TextArea(2, 5)] public string[] dialogLines;

    private int dialogStep = -1;
    private bool tutorialActive = false;
    private bool timelineEnded = false;

    [Header("Tough")]
    private bool canTap = true;
    public float tapCooldown = 2f;
    private Vector2 originalToughAnchoredPos;

    void Start()
    {
        HideAllUI();

        if (introTimeline != null)
        {
            introTimeline.stopped += OnTimelineEnd;
            introTimeline.Play();
            StartCoroutine(DetectTimelineEndFallback());
            Debug.Log("off");
        }
        else
        {
            StartCoroutine(DelayShowNPC());
            Debug.Log("on");
        }

        if (toughUI != null)
        {
            RectTransform rt = toughUI.GetComponentInChildren<RectTransform>();
            if (rt != null) originalToughAnchoredPos = rt.anchoredPosition;
        }
    }

    private void SetActiveSafe(GameObject[] objs, bool active)
    {
        if (objs == null) return;
        foreach (var obj in objs)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }

    private void HideAllUI()
    {
        SetActiveSafe(npc, false);
        SetActiveSafe(mainDialog, false);
        SetActiveSafe(functionUI, false);
        SetActiveSafe(settingUI, false);
        SetActiveSafe(roll, false);
        SetActiveSafe(toughUI, false);
        SetActiveSafe(secondaryDialog, false);
        SetActiveSafe(food, false);
        SetActiveSafe(money, false);
        SetArrowsActive(false);
    }

    private void SetActiveSafe(GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }

    private void SetArrowsActive(bool active)
    {
        if (arrows == null) return;
        foreach (var arrow in arrows)
        {
            if (arrow != null) arrow.SetActive(active);
        }
        Debug.Log($"Arrows {(active ? "ON" : "OFF")}");
    }

    private IEnumerator DetectTimelineEndFallback()
    {
        if (introTimeline == null) yield break;
        while (!timelineEnded && introTimeline.state == PlayState.Playing)
            yield return null;

        if (!timelineEnded) OnTimelineEnd(introTimeline);
    }

    private void OnTimelineEnd(PlayableDirector pd)
    {
        if (timelineEnded) return;
        timelineEnded = true;
        StartCoroutine(DelayShowNPC());
    }

    private IEnumerator DelayShowNPC()
    {
        yield return new WaitForSeconds(npcDelayAfterTimeline);

        SetActiveSafe(npc, true);
        SetActiveSafe(mainDialog, true);
        SetActiveSafe(toughUI, true);
        Debug.Log("Tutorial ON");

        if (secondaryDialog != null)
        {
            secondaryDialog.SetActive(true);
            if (secondaryText != null)
            {
                secondaryText.text = "Remember: rest is important as it conserves your energy, and use the map or you’ll get lost!";
            }
        }

        SetArrowsActive(true);

        // Đảm bảo chỉ 1 dialog bật
        if (mainDialog.activeSelf) SetActiveSafe(secondaryDialog, false);
        if (secondaryDialog.activeSelf) SetActiveSafe(mainDialog, false);

        dialogStep = -1;
        tutorialActive = true;
        ShowNextDialog();
    }

    public void OnScreenTouched()
    {
        if (!tutorialActive || !canTap) return;
        ShowNextDialog();
        StartCoroutine(TapCooldownCoroutine());
    }

    private IEnumerator TapCooldownCoroutine()
    {
        canTap = false;
        yield return new WaitForSeconds(tapCooldown);
        canTap = true;
    }

    void Update()
    {
        if (!tutorialActive) return;

        if (Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            || Input.GetKeyDown(KeyCode.Space))
        {
            OnScreenTouched();
        }
    }

    public void ShowNextDialog()
    {
        dialogStep++;

        if (dialogLines == null || dialogLines.Length == 0 || mainText == null)
            return;

        if (dialogStep < dialogLines.Length)
        {
            string currentLine = dialogLines[dialogStep];

            if (string.IsNullOrWhiteSpace(currentLine))
            {
                SetActiveSafe(mainDialog, false);
            }
            else
            {
                SetActiveSafe(mainDialog, true);
                SetActiveSafe(secondaryDialog, false);
                mainText.text = currentLine;
            }

            HandleSpecialSteps(dialogStep);
        }
        else
        {
            EndTutorial();
        }
    }

    private void ShowArrow(params GameObject[] arrowsToShow)
    {
        foreach (var a in arrows)
        {
            if (a != null) a.SetActive(false);
        }

        foreach (var a in arrowsToShow)
        {
            if (a != null) a.SetActive(true);
        }
    }

    private void HandleSpecialSteps(int step)
    {
        switch (step)
        {
            case 2: // Show function
                SetActiveSafe(functionUI, true);
                SetActiveSafe(roll, false);
                ShowArrow(arrows[0]);
                break;

            case 3: // Switch to setting
                SetActiveSafe(settingUI, true);
                SetActiveSafe(roll, false);
                SetActiveSafe(functionUI, false);
                SetActiveSafe(secondaryDialog, true);
                SetActiveSafe(mainDialog, false);

                if (toughUI != null)
                {
                    RectTransform rt = toughUI.GetComponentInChildren<RectTransform>();
                    if (rt != null)
                    {
                        rt.anchoredPosition = new Vector2(60f, -43f); // dùng tọa độ local trong Canvas
                    }
                }

                ShowArrow(arrows[1], arrows[2]);
                Debug.Log("Function OFF, Setting ON");
                break;

            case 4: // Back to main dialog
                SetActiveSafe(secondaryDialog, false);
                SetActiveSafe(mainDialog, true);
                SetActiveSafe(functionUI, true);
                SetActiveSafe(settingUI, false);
                SetActiveSafe(food, true);

                if (toughUI != null)
                {
                    RectTransform rt = toughUI.GetComponentInChildren<RectTransform>();
                    rt.anchoredPosition = originalToughAnchoredPos;
                    toughUI.SetActive(true);
                }

                ShowArrow(arrows[3]);
                Debug.Log("Back to Main Dialog");
                break;

            case 5: //intro food
                SetActiveSafe(functionUI, true);
                SetActiveSafe(money, true);
                ShowArrow(arrows[4]);
                break;

            case 6: //intro money
                SetActiveSafe(functionUI, false);
                SetArrowsActive(false);
                break;
        }
    }

    private void EndTutorial()
    {
        HideAllUI();
        SetActiveSafe(functionUI, true);
        SetActiveSafe(settingUI, true);
        SetActiveSafe(roll, true);
        SetActiveSafe(food, true);
        SetActiveSafe(money, true);
        tutorialActive = false;

        var toggle = settingUI.GetComponent<UIMenuToggle>();
        if (toggle != null)
        {
            toggle.enabled = true;
        }

        Debug.Log("Tutorial END");
    }

    private void OnDestroy()
    {
        if (introTimeline != null)
            introTimeline.stopped -= OnTimelineEnd;
    }
}
