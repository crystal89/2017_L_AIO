using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArcGallery : UIBehaviour    //, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public float spacing = 30f;

    public float cell_Spacing_X = 100f;

    public float cell_Spacing_Y = 180f;

    public float cell_Spacing_Z = 0f;

    //最大显示数量->控制中间显示的对象
    public int maxShow = 5;

    [SerializeField]
    private GameObject content;

    //public GameObject childInCenter;

    //Gallery下的所有items的数量
    private int itemsCount;

    //Gallery下的所有items
    private List<Transform> items;

    //Gallery下的所有items的位置
    private List<Vector3> itemsPos;

    //SmoothDamp->滑动阻尼
    public float smoothTime = 0.2f;

    //滑动时的速度
    private Vector3 velocity = Vector3.zero;

    protected override void Awake()
    {
        content = transform.Find("Content").gameObject;
        //添加Content子对象
        if (content == null)
        {
            content = new GameObject("Content");
            RectTransform rectTransform = content.AddComponent<RectTransform>();
            rectTransform.sizeDelta = transform.GetComponent<RectTransform>().sizeDelta;
            content.transform.parent = transform;
            content.transform.localScale = Vector3.one;
            content.transform.localRotation = Quaternion.identity;
        }

        items = new List<Transform>();
        itemsPos = new List<Vector3>();

        SetContent();

        base.Awake();
    }

    private void SetContent()
    {
        items.Clear();
        itemsPos.Clear();

        if (content)
        {
            itemsCount = content.transform.childCount;
            //Debug.Log(itemsCount);
            for (int i = 0; i < itemsCount; i++)
            {
                Transform child = content.transform.GetChild(i);
                if (child)  //i < maxShow &&
                {
                    child.GetComponent<AppItem>().isCenter = false;

                    if (i <= (maxShow / 2))
                    {
                        child.localPosition = new Vector3(i * cell_Spacing_X, -(i * (cell_Spacing_Y + spacing)), cell_Spacing_Z);
                        if (i == maxShow / 2)
                        {
                            //child.gameObject.AddComponent<ChildInCenter>();
                            //childInCenter = child.gameObject;
                            child.GetComponent<AppItem>().isCenter = true;
                        }
                        itemsPos.Add(child.localPosition);
                    }
                    else
                    {
                        child.localPosition = new Vector3((maxShow - 1 - i) * cell_Spacing_X, -(i * (cell_Spacing_Y + spacing)), cell_Spacing_Z);
                        itemsPos.Add(child.localPosition);
                    }
                    if (child.GetComponent<AppItem>() == null)
                        child.gameObject.AddComponent<AppItem>();
                    items.Add(child);
                }
            }
            //Debug.Log(260f + maxShow / 2 * cell_Spacing_X + " , " + itemsCount * (cell_Spacing_Y + spacing));
            Vector2 rectWH = new Vector2(260f + maxShow / 2 * cell_Spacing_X, 0);
            //修改scrollview的rect区域，用来控制滑动
            transform.GetComponent<RectTransform>().sizeDelta = rectWH;
            content.GetComponent<RectTransform>().sizeDelta = rectWH;
        }
    }


    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (content == null)
    //        return;

    //    if (itemsPos == null)
    //        return;

    //    //Debug.Log("Drag Event: " + eventData.delta);
    //    //通知content下的item进行移动
    //    if (eventData.delta.y > 0)
    //    {
    //        //drag up
    //        StartCoroutine(UpRollItems());
    //    }
    //    else if (eventData.delta.y < 0)
    //    {
    //        //drag down
    //    }
    //}

    private IEnumerator UpRollItems()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            //Debug.Log(i);
            Transform child = content.transform.GetChild(i);

            if (i == itemsCount - 1)
            {
                child.localPosition = Vector3.SmoothDamp(child.localPosition, itemsPos[i], ref velocity, smoothTime);
                break;
            }

            Vector3 temp = itemsPos[i];
            itemsPos[i] = itemsPos[i + 1];
            child.localPosition = Vector3.SmoothDamp(child.localPosition, itemsPos[i], ref velocity, smoothTime);
            itemsPos[i + 1] = temp;
        }
        yield return new WaitForSeconds(smoothTime);
    }
    private IEnumerator DownRollItems()
    {
        yield return new WaitForEndOfFrame();
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{

    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{

    //}
}
