using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private List<GameObject> gameNewScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private RectTransform cow; // kéo Cow vô trong Inspec
    [SerializeField] private int targetSceneIndex = 1;

    private float cowStartX;
    private float cowEndX;

    private void Start()
    {
        // Lưu vị trí mặc định của cow (lúc chưa chạy loading)
        cowStartX = cow.localPosition.x;

        // Tính điểm kết thúc dựa theo chiều rộng slider
        float width = slider.GetComponent<RectTransform>().rect.width;
        cowEndX = cowStartX + width;  // nếu muốn đi từ trái sang phải
        // cowEndX = cowStartX - width; // nếu muốn đi từ phải sang trái
    }

    public void NewGameScene()
    {
        loadingScreen.SetActive(true);

        foreach (GameObject panel in gameNewScreen)
            if (panel != null) panel.SetActive(false);

        // fake loading 2-3 giây
        StartCoroutine(FakeLoading());
    }

    private IEnumerator FakeLoading()
    {
        float duration = 3f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            slider.value = progress;
            cow.localPosition = new Vector2(Mathf.Lerp(cowStartX, cowEndX, progress), cow.localPosition.y);

            yield return null;
        }

        SceneManager.LoadScene(targetSceneIndex);
    }

    public void SetTargetScene(int index)
    {
        targetSceneIndex = index;
    }
}

