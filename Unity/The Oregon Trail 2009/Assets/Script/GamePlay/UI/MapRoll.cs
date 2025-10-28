using System.Collections.Generic;
using UnityEngine;

public class MapRoll : MonoBehaviour
{
    public RectTransform maskArea;
    public RectTransform mapContent_Prefab;
    public float rollSpeed = 10f;
    public List<RectTransform> mapContent = new List<RectTransform>();
    public float mapWidth = 1920f;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            RectTransform map = Instantiate(mapContent_Prefab, maskArea);
            map.anchoredPosition = new Vector2(-mapWidth * i, 0);
            mapContent.Add(map);
        }
    }
    void Update()
    {
        foreach (var map in mapContent)
        {
            map.anchoredPosition += Vector2.right * rollSpeed * Time.deltaTime;
        }

        RectTransform first = mapContent[0];
        if (first.anchoredPosition.x > mapWidth)
        {
            RectTransform last = mapContent[mapContent.Count - 1];
            first.anchoredPosition = last.anchoredPosition - new Vector2(mapWidth, 0);

            mapContent.RemoveAt(0);
            mapContent.Add(first);
        }

    }
}
