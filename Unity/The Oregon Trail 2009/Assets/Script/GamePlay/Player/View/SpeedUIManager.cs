using UnityEngine;
using UnityEngine.UI;

public class SpeedUIManager : MonoBehaviour
{
    [Header("Speed Buttons")]
    public Image rabbitBtn;   // Fast
    public Image humanBtn;    // Run
    public Image turtleBtn;   // Walk
    public Image stopBtn;     // Stop

    private void Start()
    {
        SetActiveButton(stopBtn);
        PlayerControl.Instance.SetSpeed(PlayerSpeed.Stop);
    }

    public void OnRabbitClick()
    {
        SetActiveButton(rabbitBtn);
        PlayerControl.Instance.SetSpeed(PlayerSpeed.Fast);
    }

    public void OnHumanClick()
    {
        SetActiveButton(humanBtn);
        PlayerControl.Instance.SetSpeed(PlayerSpeed.Run);
    }

    public void OnTurtleClick()
    {
        SetActiveButton(turtleBtn);
        PlayerControl.Instance.SetSpeed(PlayerSpeed.Walk);
    }

    public void OnStopClick()
    {
        SetActiveButton(stopBtn);
        PlayerControl.Instance.SetSpeed(PlayerSpeed.Stop);
    }

    private void SetActiveButton(Image activeBtn)
    {
        // Reset hết về alpha = 0
        SetAlpha(rabbitBtn, 0f);
        SetAlpha(humanBtn, 0f);
        SetAlpha(turtleBtn, 0f);
        SetAlpha(stopBtn, 0f);

        // Button được chọn alpha = 1
        SetAlpha(activeBtn, 1f);
    }

    private void SetAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }
}
