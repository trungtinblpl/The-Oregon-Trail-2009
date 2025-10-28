using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UISettingSwipeToggle : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    public Animator animator;
    public IntroManager introManager;

    public GameObject[] linkedObjects;

    [Header("Settings")]
    public float swipeThreshold = 100f;
    public float autoShowDelay = 5f;
    public bool allowMouseControl = true;

    private bool isVisible = true;
    private Vector2 startTouch;
    private bool swipeDetected = false;
    private Coroutine autoShowCoroutine;

    void Update()
    {
        HandleTochInput();

        if (allowMouseControl)
            HandleMouseInput();
        // if (Input.touchCount == 1)
        // {
        //     Touch t = Input.GetTouch(0);

        //     if (t.phase == TouchPhase.Began)
        //     {
        //         startTouch = t.position;
        //         swipeDetected = false;
        //     }
        //     else if (t.phase == TouchPhase.Moved && !swipeDetected)
        //     {
        //         Vector2 delta = t.position - startTouch;

        //         if (delta.y > swipeThreshold)
        //         {
        //             HideSetting();
        //             swipeDetected = true;
        //         }
        //         else if (delta.y < -swipeThreshold)
        //         {
        //             ShowSetting();
        //             swipeDetected = true;
        //         }
        //     }
        // }

        // #if UNITY_EDITOR
        //         if (Input.GetKeyDown(KeyCode.UpArrow))
        //             HideSetting();
        //         if (Input.GetKeyDown(KeyCode.DownArrow))
        //             ShowSetting();
        // #endif
    }

    private void HandleTochInput()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                startTouch = t.position;
                swipeDetected = false;
            }
            else if (t.phase == TouchPhase.Moved && !swipeDetected)
            {
                Vector2 delta = t.position - startTouch;

                if (delta.y > swipeThreshold)
                {
                    HideSetting();
                    swipeDetected = true;
                }
                else if (delta.y < -swipeThreshold)
                {
                    ShowSetting();
                    swipeDetected = true;
                }
            }
        }

#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    HideSetting();
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    ShowSetting();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            swipeDetected = false;
        }
        else if (Input.GetMouseButtonDown(0) && !swipeDetected)
        {
            Vector2 delta = (Vector2)Input.mousePosition - startTouch;

            if (delta.y > swipeThreshold)
            {
                HideSetting();
                swipeDetected = true;
            }
            else if (delta.y < -swipeThreshold)
            {
                ShowSetting();
                swipeDetected = true;
            }
        }
    }

    public void HideSetting()
    {
        if (!isVisible) return;

        animator.SetTrigger("Hide");
        isVisible = false;

        SetActiveLinked(false);

        Debug.Log("Setting hidden, linked objects off");

        // if (introManager != null && introManager.roll != null)
        // {
        //     introManager.roll.SetActive(false);
        //     Debug.Log("Roll hidden when setting hidden");
        // }
    }

    public void ShowSetting()
    {
        if (isVisible) return;

        animator.SetTrigger("Show");
        isVisible = true;

        SetActiveLinked(true);

        Debug.Log("Setting shown, linked objects on");

        // if (introManager != null && introManager.roll != null)
        // {
        //     introManager.roll.SetActive(true);
        //     Debug.Log("Roll shown when setting shown");
        // }

    }

    private void SetActiveLinked(bool state)
    {
        if (linkedObjects == null) return;

        foreach (var obj in linkedObjects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }

    public void OnFunctionPressed()
    {
        Debug.Log("[UISettingSwipeToggle] Function pressed → Hide all");
        HideSetting();
        SetActiveLinked(false);

        if (autoShowCoroutine != null) StopCoroutine(autoShowCoroutine);
        autoShowCoroutine = StartCoroutine(AutoShowRollAfterDelay());
    }

    private IEnumerator AutoShowRollAfterDelay()
    {
        yield return new WaitForSeconds(autoShowDelay);
        SetActiveLinked(true);
        Debug.Log("[UISettingSwipeToggle] Auto re-show roll after delay");
    }

    public void OnPointerClick(PointerEventData evenData)
    {
        if (!allowMouseControl) return;

        if (isVisible) HideSetting();
        else ShowSetting();
    }
}

