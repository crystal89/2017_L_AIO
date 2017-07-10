using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CombinedAnimation : AnimationBase, IAnimateEventHandler
{

    [SerializeField]
    private bool m_IsCenter = false;

    public bool isCenter
    {
        get { return m_IsCenter; }
        set { m_IsCenter = value; }
    }

    //循环状态
    public static bool isLoop { get; private set; }

    //间距
    public static float spacing { get; private set; }

    //索引号
    public int index { get; protected set; }

    //设置中心对象
    public static CombinedAnimation centerObject { get; set; }

    public CombinedAnimation leftObject { get; set; }

    public CombinedAnimation rightObject { get; set; }

    public CombinedAnimation upObject { get; set; }

    public CombinedAnimation downObject { get; set; }

    //滑动方向
    public AnimationDirection direction { get; set; }

    protected static CombinedAnimation m_AnimationObject;

    protected CombinedAnimationManager m_CombinedAnimationManager;

    protected override void Awake()
    {
        base.Awake();
        if (!transform.parent)
            return;
        Init();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        m_CombinedAnimationManager.RemoveAddAnimationObjectCallBack(HandleAddAnimationObject);
        m_CombinedAnimationManager.AddRemoveAnimationObjectCallBack(HandleRemoveAnimationObject);
        base.OnDestroy();
    }

    protected virtual void HandleAddAnimationObject(CombinedAnimation animationObject)
    {

    }

    protected virtual void HandleRemoveAnimationObject(CombinedAnimation animationObject)
    {

    }

    void VerifyIsCenterBoolean()
    {
        if (isCenter && !centerObject)
        {
            centerObject = this;
        }
        else if (isCenter && centerObject)
        {
            Debug.LogError("Onle one center object,please restart!");
            return;
        }
    }

    void VerifyHasCenterObject()
    {
        if (centerObject == null)
        {
            Debug.LogError("You need set a center object!");
            return;
        }
    }

    public virtual void Init()
    {
        if (GetComponentInParent<CombinedAnimationManager>())
        {
            m_CombinedAnimationManager = GetComponentInParent<CombinedAnimationManager>();
        }
        else
        {
            Debug.Log("You mush have a Manager.");
        }

        //VerifyIsCenterBoolean();
        //m_CombinedAnimationManager.WaitOneFrame(VerifyHasCenterObject);

        isLoop = m_CombinedAnimationManager.isLoop;
        spacing = m_CombinedAnimationManager.spacing;

        m_CombinedAnimationManager.AddAddAnimationObjectCallBack(HandleAddAnimationObject);
        m_CombinedAnimationManager.AddRemoveAnimationObjectCallBack(HandleAddAnimationObject);
    }

    public void OnMoveLeft(BaseEventData data)
    {
        if (!centerObject)
            return;
        if (!m_IsAnimating)
        {
            direction = AnimationDirection.RightToLeft;
            HorizontalAnimate(m_Duration);
        }
    }
    public void OnMoveRight(BaseEventData data)
    {
        if (!centerObject)
            return;
        if (!m_IsAnimating)
        {
            direction = AnimationDirection.LeftToRight;
            HorizontalAnimate(m_Duration);
        }
    }

    public void OnMoveDown(BaseEventData data)
    {
        if (!centerObject)
            return;
        if (!m_IsAnimating)
        {
            direction = AnimationDirection.UpToDown;
            VerticalAnimate(m_Duration);
        }
    }

    public void OnMoveUp(BaseEventData data)
    {
        if (!centerObject)
            return;
        if (!m_IsAnimating)
        {
            direction = AnimationDirection.DownToUp;
            VerticalAnimate(m_Duration);
        }
    }

    public abstract void HorizontalAnimate(float duration);

    public abstract void VerticalAnimate(float duration);

    public virtual void SetAnimationObjectNode()
    {
        int thisIndex = transform.GetSiblingIndex();
        if (isLoop)
        {
            if (thisIndex == 0)
            {
                upObject = (transform.parent.GetChild(transform.parent.childCount - 1)).GetComponent<CombinedAnimation>();
                downObject = (transform.parent.GetChild(thisIndex + 1)).GetComponent<CombinedAnimation>();
            }
            else if (thisIndex == (transform.parent.childCount - 1))
            {
                upObject = (transform.parent.GetChild(thisIndex - 1)).GetComponent<CombinedAnimation>();
                downObject = (transform.parent.GetChild(0)).GetComponent<CombinedAnimation>();
            }
            else
            {
                upObject = (transform.parent.GetChild(thisIndex - 1)).GetComponent<CombinedAnimation>();
                downObject = (transform.parent.GetChild(thisIndex + 1)).GetComponent<CombinedAnimation>();
            }
        }
        else
        {
            if (transform.parent.childCount > 1)
            {
                if (thisIndex == 0)
                {
                    downObject = (transform.parent.GetChild(thisIndex + 1)).GetComponent<CombinedAnimation>();
                }
                else if (thisIndex == (transform.parent.childCount - 1))
                {
                    upObject = (transform.parent.GetChild(thisIndex - 1)).GetComponent<CombinedAnimation>();
                }
                else
                {
                    upObject = (transform.parent.GetChild(thisIndex - 1)).GetComponent<CombinedAnimation>();
                    downObject = (transform.parent.GetChild(thisIndex + 1)).GetComponent<CombinedAnimation>();
                }
            }
        }
        Debug.LogError("set animation object node: " + upObject.name + " / " + downObject.name);
    }

    public virtual void SetAnimatonObjectState()
    {

    }

    protected virtual void AnimatingCallback(float value)
    {

    }

    protected virtual void AnimatedCallback()
    {

    }
}
