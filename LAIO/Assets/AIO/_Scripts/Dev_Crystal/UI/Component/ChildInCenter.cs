using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChildInCenter : MonoBehaviour
{
    public bool zoom = false;
    public float currentScale = 1;
    public float targetScale = 1;
    public float durationTime = 0.2f;

    void Start()
    {

    }

    private void Zoom()
    {
        if (zoom)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(currentScale, targetScale, durationTime);
        }
        else
        {
            transform.localScale = Vector3.one * currentScale;
        }
    }
}
