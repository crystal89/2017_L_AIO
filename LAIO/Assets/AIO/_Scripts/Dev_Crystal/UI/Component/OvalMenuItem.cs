using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OvalMenuItem : AnimationBase, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public enum LoadType
    {
        None,
        GameCenter,
        App,
        Setting
    }

    [SerializeField]
    private LoadType loadType = LoadType.None;

    public LoadType ItemLoadType
    {
        get { return loadType; }
        set { loadType = value; }
    }

    private GameObject loadPage;

    [SerializeField]
    protected bool m_IsCenter = false;
    public bool IsCenter { get { return m_IsCenter; } set { m_IsCenter = value; } }
    private Vector3 scaleNormal = Vector3.one;
    private float scaleTargetFactor = 1.25f;

    private OvalMenu mParent;

    //public GameObject preItem;

    //public GameObject nextItem;

    [SerializeField]
    private AppShowPlane.ShowType mShowType = AppShowPlane.ShowType.NONE;
    public AppShowPlane.ShowType ShowType { get { return mShowType; } set { mShowType = value; } }

    private bool isPointerDown = false;
    private float recordTime;

    //按下的时候超过这个值就认为是长按状态
    private float invertalTime = 0.15f;
    private Vector3 initPosition = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        if (transform.GetSiblingIndex() == 0)
        {
            ItemLoadType = LoadType.Setting;
            loadPage = GameObject.FindGameObjectWithTag("Setting");
            ShowType = AppShowPlane.ShowType.NONE;
            //Debug.Log(loadPage);
        }
        else
        {
            ItemLoadType = LoadType.App;
        }
    }

    protected override void Start()
    {
        base.Start();

        initPosition = transform.localPosition;

        mParent = transform.parent.GetComponent<OvalMenu>();
        scaleNormal = transform.localScale;
        DoShow();
    }

    private void Update()
    {
        if (m_IsCenter)
        {
            if (loadPage && !loadPage.activeSelf)
                loadPage.SetActive(true);

            //显示卸载面板
            if (isPointerDown && ItemLoadType == LoadType.App)
            {
                if ((Time.time - recordTime) > invertalTime)
                {
                    AppShowPlane.Instance.Uninstall(true);
                }
            }
        }
        else
        {
            if (loadPage && loadPage.activeSelf)
                loadPage.SetActive(false);
        }

        if(isPointerDown && (Time.time - recordTime) > invertalTime)
        {
            Debug.Log("moving");
            //追随鼠标位置
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        recordTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;

        //归位
        transform.localPosition = initPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void DoShow()
    {
        if (mParent == null)
            return;

        if (mParent.centerObject.Equals(this.gameObject))
        {
            IsCenter = true;

            transform.localScale = Vector3.Lerp(scaleNormal, scaleNormal * scaleTargetFactor, 1f);

            StartCoroutine(AppShowPlane.Instance.DoShow(ShowType));
        }
        else
        {
            IsCenter = false;

            transform.localScale = scaleNormal;

            //隐藏卸载面板
            AppShowPlane.Instance.Uninstall(false);
        }
    }

    public void UpdatePosition(float duration, Vector3 startPosition, Vector3 targetPosition)
    {
        CrossFadeVector(m_VectorTweenRunner, duration, startPosition, targetPosition, MovingCallback, MoveEndCallback);
        m_IsAnimating = true;

        initPosition = targetPosition;
    }

    private void MovingCallback(Vector3 vector)
    {
        transform.localPosition = vector;
    }

    private void MoveEndCallback()
    {
        m_IsAnimating = false;
        //当划动停止时，显示需要展示的内容
        if (mParent && mParent.dragState == OvalMenu.DragState.DRAG_NULL)
        {
            DoShow();
        }
    }
}
