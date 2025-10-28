using UnityEngine;

public class ArrowBounce : MonoBehaviour
{
    public float amplitude = 10f;   // độ cao nhảy (pixel/units)
    public float speed = 2f;        // tốc độ nhảy
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition; // lấy vị trí ban đầu
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
