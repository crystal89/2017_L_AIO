using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{

    [SerializeField]
    private float targetZoom = 1;
    [SerializeField]
    private float durationTime = 0.5f;

    private float normalZoom;
    private Vector3 transZoom = Vector3.one;

    void Start()
    {
        normalZoom = transform.localScale.x;
        transZoom = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetZoom != normalZoom)
        {
            transform.localScale = transZoom * Mathf.Lerp(normalZoom, targetZoom, durationTime);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = transZoom * normalZoom;
    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
