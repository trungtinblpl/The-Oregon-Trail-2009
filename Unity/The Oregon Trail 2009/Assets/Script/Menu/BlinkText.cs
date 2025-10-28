using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkSpeed = 0.5f;
    private TextMeshProUGUI text;
    private float timer;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= blinkSpeed)
        {
            text.enabled = !text.enabled;
            timer = 0;
        }
    }
}
