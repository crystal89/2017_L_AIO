using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AddcombinedAnimationCallBack : UnityEvent<CombinedAnimation> { };

public class RemoveCombinedAnimationCallBack : UnityEvent<CombinedAnimation> { };

public class CombinedAnimationManager : UIBehaviour
{
    //协程中需要执行的内容
    public delegate void ExecuteInCoroutine();

    //动画对象
    public CombinedAnimation m_AnimationObject;

    [SerializeField]
    private bool m_IsLoop = false;

    public bool isLoop { get { return m_IsLoop; } private set { m_IsLoop = value; } }

    [SerializeField]
    private float m_Spacing;

    public float spacing { get { return m_Spacing; } private set { m_Spacing = value; } }

    AddcombinedAnimationCallBack m_AddAnimationObjectCallback;

    RemoveCombinedAnimationCallBack m_RemoveAnimationObjectCallback;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void AddAddAnimationObjectCallBack(UnityAction<CombinedAnimation> callback)
    {
        if (m_AddAnimationObjectCallback == null)
            m_AddAnimationObjectCallback = new AddcombinedAnimationCallBack();
        m_AddAnimationObjectCallback.AddListener(callback);
    }

    public void RemoveAddAnimationObjectCallBack(UnityAction<CombinedAnimation> callback)
    {
        if (m_AddAnimationObjectCallback != null)
            m_AddAnimationObjectCallback.RemoveListener(callback);
    }

    public void InvokeAddAnimationObjectCallBack(CombinedAnimation animationObject)
    {
        if (m_AddAnimationObjectCallback != null)
            m_AddAnimationObjectCallback.Invoke(animationObject);
    }

    public void AddRemoveAnimationObjectCallBack(UnityAction<CombinedAnimation> callback)
    {
        if (m_RemoveAnimationObjectCallback == null)
            m_RemoveAnimationObjectCallback = new RemoveCombinedAnimationCallBack();
        m_RemoveAnimationObjectCallback.AddListener(callback);
    }

    public void RemoveRemoveAnimationObjectCallBack(UnityAction<CombinedAnimation> callback)
    {
        if (m_RemoveAnimationObjectCallback != null)
            m_RemoveAnimationObjectCallback.RemoveListener(callback);
    }

    public void InvokeRemoveAnimationObjectCallBack(CombinedAnimation animationObject)
    {
        if (m_RemoveAnimationObjectCallback != null)
            m_RemoveAnimationObjectCallback.Invoke(animationObject);
    }

    public virtual GameObject AddAnimationObject()
    {
        CombinedAnimation animationObject = Instantiate(m_AnimationObject);
        animationObject.transform.SetParent(transform);
        animationObject.transform.localScale = Vector3.one;
        animationObject.transform.localPosition = Vector3.zero;
        animationObject.name = "App_" + animationObject.transform.GetSiblingIndex();

        if (animationObject.transform.GetSiblingIndex() == 0)
        {
            animationObject.isCenter = true;
        }

        animationObject.Init();
        InvokeAddAnimationObjectCallBack(animationObject);

        return animationObject.gameObject;
    }

    public virtual void RemoveAnimationObject(CombinedAnimation _animationObject)
    {
        Destroy(_animationObject.gameObject);
        InvokeRemoveAnimationObjectCallBack(_animationObject);
    }

    public int GetAnimatonObjectCount()
    {
        return GetComponentsInChildren<CombinedAnimation>().Length;
    }

    public void ExecuteAnimateRight()
    {
        AIOExecuteEvents.ExecuteHierarchyChildren<IAnimateEventHandler>(gameObject, null, (x, y) => x.OnMoveRight(y));
    }

    public void ExecuteAnimateLeft()
    {
        AIOExecuteEvents.ExecuteHierarchyChildren<IAnimateEventHandler>(gameObject, null, (x, y) => x.OnMoveLeft(y));
    }

    public void WaitOneFrame(ExecuteInCoroutine func)
    {
        StartCoroutine(_WaitOneFrame(func));
    }

    IEnumerator _WaitOneFrame(ExecuteInCoroutine func)
    {
        yield return new WaitForEndOfFrame();
        if (func != null)
            func();
    }
}
