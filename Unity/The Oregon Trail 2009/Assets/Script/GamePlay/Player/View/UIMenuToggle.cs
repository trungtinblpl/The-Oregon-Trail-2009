using UnityEngine;

public class UIMenuToggle : MonoBehaviour
{
    public Animator animator;
    private bool isVisiable = true;

    public void HideMenu()
    {
        if (isVisiable)
        {
            animator.SetTrigger("Show");
            isVisiable = false;
            Debug.Log("show");
        }
    }

    private void ShowMenu()
    {
        if (!isVisiable)
        {
            animator.SetTrigger("Hide");
            isVisiable = true;
            Debug.Log("hide");
        }
    }

    public void OnSwipeUp()
    {
        HideMenu();
        Debug.Log("hide");
    }
    
    public void OnPausePressed()
    {
        ShowMenu();
        Debug.Log("show");
    }
}
