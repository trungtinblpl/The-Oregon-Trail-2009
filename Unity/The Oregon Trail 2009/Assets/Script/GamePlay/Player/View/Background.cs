using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform mainCamera;
    public Transform midBg;
    public Transform sideBg;
    public float length;


    // Update is called once per frame
    void Update()
    {
        if (mainCamera.position.x > midBg.position.x)
        {
            UpdateBackground(Vector3.right);
        }
        else if (mainCamera.position.x < midBg.position.x)
        {
            UpdateBackground(Vector3.left);
        }
    }

    void UpdateBackground(Vector3 direction)
    {
        sideBg.position = midBg.position + direction * length;
        Transform temp = midBg;
        midBg = sideBg;
        sideBg = temp;
    }
}