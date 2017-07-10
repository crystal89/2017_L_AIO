using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppItem : CombinedAnimation
{
    //static Vector3 s_UpPosition;
    //static Vector3 s_DownPosition;
    static Vector3 s_RightPosition;
    static Vector3 s_LeftPosition;

    protected override void Awake()
    {
        base.Awake();
        if (!transform.parent)
            return;

        if (centerObject)
        {
            //s_UpPosition = new Vector3(transform.localPosition.x - 100, transform.localPosition.y + 180 + 30, 0);
            //s_DownPosition = new Vector3(transform.localPosition.x - 100, transform.localPosition.y - 180 - 30, 0);
        }

        SetAnimationObjectNode();
        m_CombinedAnimationManager.WaitOneFrame(SetAnimatonObjectState);
    }

    public override void Init()
    {
        base.Init();
    }

    protected override void HandleAddAnimationObject(CombinedAnimation animationObject)
    {
        base.HandleAddAnimationObject(animationObject);
        if (centerObject)
        {
            //s_UpPosition = new Vector3(transform.localPosition.x - 100, transform.localPosition.y + 180 + 30, 0);
            //s_DownPosition = new Vector3(transform.localPosition.x - 100, transform.localPosition.y - 180 - 30, 0);
        }
        SetAnimationObjectNode();
        m_CombinedAnimationManager.WaitOneFrame(SetAnimatonObjectState);
    }

    public override void SetAnimatonObjectState()
    {
        base.SetAnimatonObjectState();
        if (isLoop)
        {

        }
        else
        {

        }
    }

    protected override void HandleRemoveAnimationObject(CombinedAnimation animationObject)
    {
        base.HandleRemoveAnimationObject(animationObject);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

   
    //左右滑动
    public override void HorizontalAnimate(float duration)
    {

    }

    //上下滑动
    public override void VerticalAnimate(float duration)
    {

    }
}
