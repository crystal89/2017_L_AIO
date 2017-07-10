using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArcSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image handle;
    Vector3 handleLocation;

    public Image baseImage;
    public Text valueText;

    float circleRadius = 0.0f;
    bool isPointerDown = false;
    //忽略弧形内部的交互
    public float ignoreInTouchRadiusHandleOffset = 10;
    [Tooltip("初始角度到终止角度")]
    public float firstAngle = 0;
    public float secondAngle = 180;
    float tempAngle = 10;//用来缓动


    protected virtual void Start()
    {    
        //获取handle的在圆弧上的半径
        circleRadius = Mathf.Sqrt(Mathf.Pow(handle.GetComponent<RectTransform>().localPosition.x, 2) + Mathf.Pow(handle.GetComponent<RectTransform>().localPosition.y, 2));
        ignoreInTouchRadiusHandleOffset = circleRadius - ignoreInTouchRadiusHandleOffset;
        handleLocation = handle.GetComponent<RectTransform>().localPosition;
        this.transform.GetComponent<Image>().fillAmount = 0;
    }

    //重置handle的位置
    protected virtual void ReSet()
    {
        handle.GetComponent<RectTransform>().localPosition = handleLocation;

        //重置图片的filled值
        this.transform.GetComponent<Image>().fillAmount = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(TrackPointer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(TrackPointer());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    IEnumerator TrackPointer()
    {
        var ray = GetComponentInParent<GraphicRaycaster>();
        var input = FindObjectOfType<StandaloneInputModule>();

        if (ray != null && input != null)
        {
            while (Application.isPlaying)
            {
                //Handle位于左侧
                if (isPointerDown)
                {
                    Vector2 localPos;
                    //获取鼠标当前位置out里赋值  
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, ray.eventCamera, out localPos);

                    localPos.x = -localPos.x;

                    //半径  
                    float mouseRadius = Mathf.Sqrt(localPos.x * localPos.x + localPos.y * localPos.y);
                    //Debug.Log(mouseRadius.ToString()+" , "+ignoreInTouchRadiusHandleOffset.ToString());
                    //阻止圆内部点击的响应，只允许在一个圆环上进行响应  
                    if (mouseRadius > ignoreInTouchRadiusHandleOffset)// && handleButton.GetComponent<RectTransform>().localPosition.x <= 0  
                    {
                        //0-180  -180-0偏移后的角度 从第一象限校正到0-360  
                        float angle = (Mathf.Atan2(localPos.y, localPos.x)) * Mathf.Rad2Deg;
                        if (angle < 0)
                            angle = 360 + angle; ;

                        if (angle < firstAngle)
                            angle = firstAngle;
                        if (angle > secondAngle)
                            angle = secondAngle;

                        angle = (tempAngle + angle) / 2f;
                        tempAngle = angle;

                        //改变小圆的位置  
                        handle.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Cos(-angle / Mathf.Rad2Deg + 45.0f * Mathf.PI) * circleRadius, Mathf.Sin(-angle / Mathf.Rad2Deg + 45.0f * Mathf.PI) * circleRadius, 0);

                        //根据数值修改图片的颜色值
                        //this.transform.GetComponent<Image>().color = Color.Lerp(Color.green, Color.blue, (angle - firstAngle) / (secondAngle - firstAngle));
                        //根据数值修改图片Filled值
                        this.transform.GetComponent<Image>().fillAmount =(angle - firstAngle) / (secondAngle - firstAngle);

                        //数值的偏移值  
                        //float temp = secondAngle - firstAngle;// 360 - 285 + 64;
                        float tempangle = (angle - firstAngle) / (secondAngle - firstAngle) * 100;

                        if (valueText != null)
                        {
                            //可能会出现很小的数 注意保留小数位数  
                            valueText.text = ((int)tempangle).ToString();
                        }
                    }
                }
                yield return 0;
            }
        }
    }
}