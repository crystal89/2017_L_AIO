using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OvalMenu : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Transform mParent;

    //中间位置的对象
    public GameObject centerObject;
    private bool isSetCenter = false;

    public GameObject itemPerfab;
    private int itemsCount = 10;

    public Vector3 spacing = new Vector3(100f, 210f, 0);    //180+30 -> 192+30
    public int maxShowCount = 5;
    //public bool isLoop = true;
    public float duration = 0.2f;

    public List<Vector3> itemsPos;
    public List<Transform> items;
    public List<Sprite> sourceSprites;

    //private float dragBeginX;
    //private float dragBeginY;
    //private float dragCurrentX;
    //private float dragCurrentY;
    //private float dragSegmentX;
    //private float dragSegmentY;
    //private float dragSensitivity = Screen.height * 0.05f;

    public enum DragState
    {
        DRAG_NULL = 0,
        DRAG_BEGIN = 1,
        DRAG_END = 2
    }
    public DragState dragState = DragState.DRAG_NULL;

    public enum DragDirection
    {
        NONE,
        HORIZONTAL_LEFT,
        HORIZONTAL_RIGHT,
        VERTICAL_UP,
        VERTICAL_DOWN
    }
    private DragDirection mDirection = DragDirection.NONE;

    private void UpdateItems()
    {
        //Debug.Log(mDirection);
        switch (mDirection)
        {
            case DragDirection.VERTICAL_UP:
                for (int i = 0; i < items.Count - 1; i++)
                {
                    Transform temp = items[i];
                    items[i] = items[i + 1];
                    items[i + 1] = temp;
                }
                break;

            case DragDirection.VERTICAL_DOWN:
                for (int i = items.Count - 1; i > 0; i--)
                {
                    Transform temp = items[i];
                    items[i] = items[i - 1];
                    items[i - 1] = temp;
                }
                break;
        }
        UpdateItemsPos(mDirection);
    }

    private void InitItems()
    {
        if (itemsPos == null)
            return;

        if (items != null)
            items.Clear();

        if (itemPerfab == null)
            return;


        if (itemsCount < maxShowCount / 2)
        {
            //当前加载的items数量比要显示的数量小时
        }
        else
        {
            for (int i = 0; i < itemsCount; i++)
            {
                Transform item = Instantiate(itemPerfab, transform).transform;
                item.name = "appItem_" + i;

                if (i < (itemsPos.Count - 1))
                {
                    item.localPosition = itemsPos[i + 1];
                }
                else
                {
                    if (i == (itemsCount - 1))
                    {
                        item.localPosition = itemsPos[0];
                    }
                    else
                    {
                        item.localPosition = itemsPos[itemsPos.Count - 1];
                        item.gameObject.SetActive(false);
                    }
                }

                item.localScale = Vector3.one;
                item.localRotation = Quaternion.identity;

                //给每个item设定icon
                if (sourceSprites.Count > 0)
                {
                    if (i == 0)
                    {
                        item.GetComponent<Image>().sprite = sourceSprites[0];
                        item.GetComponent<OvalMenuItem>().ShowType = AppShowPlane.ShowType.NONE;
                    }
                    else
                    {
                        if (i < sourceSprites.Count)
                        {
                            item.GetComponent<OvalMenuItem>().ShowType = AppShowPlane.ShowType.IMAGE;
                            item.GetComponent<Image>().sprite = sourceSprites[i];
                        }
                        else
                        {
                            item.GetComponent<OvalMenuItem>().ShowType = AppShowPlane.ShowType.MODEL;
                            item.GetComponent<Image>().sprite = sourceSprites[i % sourceSprites.Count];
                        }
                    }
                }
                items.Add(item);
            }

            //设置center GameObject
            if (maxShowCount % 2 == 0)
            {
                centerObject = items[(maxShowCount / 2)].gameObject;
            }
            else
            {
                centerObject = items[(maxShowCount / 2) + (maxShowCount % 2) - 1].gameObject;
            }
        }
    }
    private void UpdateItemsPos(DragDirection direction)
    {
        isSetCenter = false;

        for (int i = 0; i < items.Count; i++)
        {
            //if (i < (itemsPos.Count - 1))
            //{
            //    items[i].gameObject.SetActive(true);

            //    items[i].GetComponent<OvalMenuItem>().UpdatePosition(duration, items[i].localPosition, itemsPos[i + 1]);
            //}
            //else
            //{      

            switch (direction)
            {
                case DragDirection.VERTICAL_UP:
                    if (i < (itemsPos.Count - 1))
                    {
                        items[i].gameObject.SetActive(true);

                        items[i].GetComponent<OvalMenuItem>().UpdatePosition(duration, items[i].localPosition, itemsPos[i + 1]);
                    }
                    else
                    {
                        if (i < (items.Count - 1))
                        {
                            items[i].localPosition = itemsPos[itemsPos.Count - 1];
                            items[i].gameObject.SetActive(false);
                        }
                        else if (i == (items.Count - 1))
                        {
                            items[i].gameObject.SetActive(true);
                            items[i].GetComponent<OvalMenuItem>().UpdatePosition(duration, items[i].localPosition, itemsPos[0]);
                        }
                    }

                    if (centerObject.Equals(items[i].gameObject) && !isSetCenter)
                    {
                        isSetCenter = true;
                        centerObject = items[i + 1].gameObject;
                    }

                    break;

                case DragDirection.VERTICAL_DOWN:
                    if (i < (itemsPos.Count - 1))
                    {
                        items[i].gameObject.SetActive(true);

                        items[i].GetComponent<OvalMenuItem>().UpdatePosition(duration, items[i].localPosition, itemsPos[i + 1]);
                    }
                    else
                    {
                        if (i == (items.Count - 1))
                        {
                            items[i].localPosition = itemsPos[0];
                            items[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            items[i].GetComponent<OvalMenuItem>().UpdatePosition(duration, items[i].localPosition, itemsPos[itemsPos.Count - 1]);
                            items[i].gameObject.SetActive(false);
                        }
                    }

                    if (centerObject.Equals(items[i].gameObject) && !isSetCenter)
                    {
                        isSetCenter = true;
                        centerObject = items[i - 1].gameObject;
                    }

                    break;
            }
            //}
        }
    }

    private void InitPosition()
    {
        if (itemsPos != null)
            itemsPos.Clear();

        if (maxShowCount > 0)
        {
            for (int i = -1; i <= maxShowCount; i++)
            {
                //最大显示数为偶数
                if (maxShowCount % 2 == 0)
                {
                    if (i <= (maxShowCount / 2))
                    {
                        itemsPos.Add(new Vector3(i * spacing.x + 130f, -(i * spacing.y) - 90f, spacing.z));
                    }
                    else
                    {
                        itemsPos.Add(new Vector3((maxShowCount - i) * spacing.x + 130f, -(i * spacing.y) - 90f, spacing.z));
                    }
                }
                //最大显示数为奇数
                else
                {
                    if (i < ((maxShowCount / 2) + (maxShowCount % 2)))
                    {
                        itemsPos.Add(new Vector3(i * spacing.x + 130f, -(i * spacing.y) - 90f, spacing.z));
                    }
                    else
                    {
                        itemsPos.Add(new Vector3((maxShowCount - 1 - i) * spacing.x + 130f, -(i * spacing.y) - 90f, spacing.z));
                    }
                }
            }
        }
    }

    private void Awake()
    {
        itemsPos = new List<Vector3>();
        items = new List<Transform>();

        mParent = transform.parent;
        if (mParent)
        {
            //可滑动的rect区域
            Vector2 rectWH = new Vector2(260f + maxShowCount / 2 * spacing.x, 0);
            mParent.GetComponent<RectTransform>().sizeDelta = rectWH;

            //初始化位置
            InitPosition();

            //实例化Items
            InitItems();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragState == DragState.DRAG_NULL)
        {
            dragState = DragState.DRAG_BEGIN;

            //dragBeginY = Input.mousePosition.y;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragState != DragState.DRAG_BEGIN)
            return;

        //dragCurrentY = Input.mousePosition.y;
        ////计算上下划动的长度
        //dragSegmentY = dragCurrentY - dragBeginY;

        ////Debug.Log(dragSegmentY);
        //if (dragSegmentY > 10f)
        //{
        //    //上划
        //    mDirection = DragDirection.VERTICAL_UP;

        //    dragState = DragState.DRAG_END;
        //}
        //else if (dragSegmentY < -10f)
        //{
        //    //下划
        //    mDirection = DragDirection.VERTICAL_DOWN;

        //    dragState = DragState.DRAG_END;
        //}
        if (eventData.delta.y > 5f)
        {
            mDirection = DragDirection.VERTICAL_UP;
            dragState = DragState.DRAG_END;
        }
        else if (eventData.delta.y < -5f)
        {
            mDirection = DragDirection.VERTICAL_DOWN;
            dragState = DragState.DRAG_END;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragState = DragState.DRAG_NULL;
        UpdateItems();
        mDirection = DragDirection.NONE;
    }
}
