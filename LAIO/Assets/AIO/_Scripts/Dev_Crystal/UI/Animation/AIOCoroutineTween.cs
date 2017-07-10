using System.Collections;
using UnityEngine.Events;
using UnityEngine;

// Base interface for tweeners,
// using an interface instead of
// an abstract class as we want the
// tweens to be structs.
public interface IAIOTweenValue
{
    void TweenValue(float floatPercentage);
    bool ignoreTimeScale { get; }
    float duration { get; }
    bool ValidTarget();
    bool endState { get; set; }

}

// ColorFade tween class, receives the
// TweenValue callback and then sets
// the value on the target.
public struct ColorTween : IAIOTweenValue
{
    public enum ColorTweenMode
    {
        All,
        RGB,
        Alpha
    }

    public class ColorTweenCallback : UnityEvent<Color> { }

    private ColorTweenCallback m_Target;
    private Color m_StartColor;
    private Color m_TargetColor;
    private ColorTweenMode m_TweenMode;

    private float m_Duration;
    private bool m_IgnoreTimeScale;
    public bool endState { get; set; }
    public Color startColor
    {
        get { return m_StartColor; }
        set { m_StartColor = value; }
    }

    public Color targetColor
    {
        get { return m_TargetColor; }
        set { m_TargetColor = value; }
    }

    public ColorTweenMode tweenMode
    {
        get { return m_TweenMode; }
        set { m_TweenMode = value; }
    }

    public float duration
    {
        get { return m_Duration; }
        set { m_Duration = value; }
    }

    public bool ignoreTimeScale
    {
        get { return m_IgnoreTimeScale; }
        set { m_IgnoreTimeScale = value; }
    }

    public void TweenValue(float floatPercentage)
    {
        if (!ValidTarget())
            return;

        var newColor = Color.Lerp(m_StartColor, m_TargetColor, floatPercentage);

        if (m_TweenMode == ColorTweenMode.Alpha)
        {
            newColor.r = m_StartColor.r;
            newColor.g = m_StartColor.g;
            newColor.b = m_StartColor.b;
        }
        else if (m_TweenMode == ColorTweenMode.RGB)
        {
            newColor.a = m_StartColor.a;
        }
        m_Target.Invoke(newColor);
    }

    public void AddOnChangedCallback(UnityAction<Color> callback)
    {
        if (m_Target == null)
            m_Target = new ColorTweenCallback();

        m_Target.AddListener(callback);
    }

    public bool GetIgnoreTimescale()
    {
        return m_IgnoreTimeScale;
    }

    public float GetDuration()
    {
        return m_Duration;
    }

    public bool ValidTarget()
    {
        return m_Target != null;
    }
}

// Float tween class, receives the
// TweenValue callback and then sets
// the value on the target.
public struct FloatTween : IAIOTweenValue
{
    public class FloatTweenCallback : UnityEvent<float> { }
    public class FloatTweenEndCallback : UnityEvent { }

    private FloatTweenCallback m_Target;
    private FloatTweenEndCallback m_TargetEnd;

    public bool endState { get; set; }

    private float m_StartValue;
    private float m_TargetValue;

    private float m_Duration;
    private bool m_IgnoreTimeScale;

    public float startValue
    {
        get { return m_StartValue; }
        set { m_StartValue = value; }
    }

    public float targetValue
    {
        get { return m_TargetValue; }
        set { m_TargetValue = value; }
    }

    public float duration
    {
        get { return m_Duration; }
        set { m_Duration = value; }
    }

    public bool ignoreTimeScale
    {
        get { return m_IgnoreTimeScale; }
        set { m_IgnoreTimeScale = value; }
    }

    public void TweenValue(float floatPercentage)
    {
        if (!ValidTarget())
            return;

        //差值
        var newValue = Mathf.Lerp(m_StartValue, m_TargetValue, floatPercentage);

        //回调
        m_Target.Invoke(newValue);

        //结束时回调
        if (floatPercentage == 1.0f && !endState)
        {
            if (m_TargetEnd != null)
                m_TargetEnd.Invoke();
            endState = true;
        }
    }

    public void AddOnChangedCallback(UnityAction<float> callback)
    {
        if (m_Target == null)
            m_Target = new FloatTweenCallback();

        m_Target.AddListener(callback);
    }

    public void AddOnEndCallback(UnityAction callback)
    {
        if (m_TargetEnd == null)
            m_TargetEnd = new FloatTweenEndCallback();

        m_TargetEnd.AddListener(callback);
    }

    public bool GetIgnoreTimescale()
    {
        return m_IgnoreTimeScale;
    }

    public float GetDuration()
    {
        return m_Duration;
    }

    public bool ValidTarget()
    {
        return m_Target != null;
    }
}

// Vector3 tween class, receives the
// TweenValue callback and then sets
// the value on the target.
public struct VectorTween : IAIOTweenValue
{
    public class VectorTweenCallback : UnityEvent<Vector3> { }
    public class VectorTweenEndCallback : UnityEvent { }

    private VectorTweenCallback m_Target;
    private VectorTweenEndCallback m_TargetEnd;

    public bool endState { get; set; }

    private Vector3 m_StartValue;
    private Vector3 m_TargetValue;

    private float m_Duration;
    private bool m_IgnoreTimeScale;

    public Vector3 startValue
    {
        get { return m_StartValue; }
        set { m_StartValue = value; }
    }

    public Vector3 targetValue
    {
        get { return m_TargetValue; }
        set { m_TargetValue = value; }
    }

    public float duration
    {
        get { return m_Duration; }
        set { m_Duration = value; }
    }

    public bool ignoreTimeScale
    {
        get { return m_IgnoreTimeScale; }
        set { m_IgnoreTimeScale = value; }
    }

    public void TweenValue(float floatPercentage)
    {
        if (!ValidTarget())
            return;

        //差值

        var newValue = Vector3.Lerp(m_StartValue, m_TargetValue, floatPercentage);
        //var newValueX = Mathf.Lerp(m_StartValue.x, m_TargetValue.x, floatPercentage);
        //var newValueY = Mathf.Lerp(m_StartValue.y, m_TargetValue.y, floatPercentage);
        //var newValueZ = Mathf.Lerp(m_StartValue.z, m_TargetValue.z, floatPercentage);

        //回调
        m_Target.Invoke(newValue);


        //结束时回调
        if (floatPercentage == 1.0f && !endState)
        {
            if (m_TargetEnd != null)
                m_TargetEnd.Invoke();
            endState = true;
        }
    }

    public void AddOnChangedCallback(UnityAction<Vector3> callback)
    {
        if (m_Target == null)
            m_Target = new VectorTweenCallback();

        m_Target.AddListener(callback);
    }

    public void AddOnEndCallback(UnityAction callback)
    {
        if (m_TargetEnd == null)
            m_TargetEnd = new VectorTweenEndCallback();

        m_TargetEnd.AddListener(callback);
    }

    public bool GetIgnoreTimescale()
    {
        return m_IgnoreTimeScale;
    }

    public float GetDuration()
    {
        return m_Duration;
    }

    public bool ValidTarget()
    {
        return m_Target != null;
    }
}

// Tween runner, executes the given tween.
// The coroutine will live within the given
// behaviour container.
public class TweenRunner<T> where T : struct, IAIOTweenValue
{
    protected MonoBehaviour m_CoroutineContainer;
    protected IEnumerator m_Tween;

    // utility function for starting the tween
    private static IEnumerator Start(T tweenInfo)
    {
        if (!tweenInfo.ValidTarget())
            yield break;

        var elapsedTime = 0.0f;
        while (elapsedTime < tweenInfo.duration)
        {
            elapsedTime += tweenInfo.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            var percentage = Mathf.Clamp01(elapsedTime / tweenInfo.duration);
            tweenInfo.TweenValue(percentage);
            yield return null;
        }
        tweenInfo.TweenValue(1.0f);
    }

    public void Init(MonoBehaviour coroutineContainer)
    {
        m_CoroutineContainer = coroutineContainer;
    }

    public void StartTween(T info)
    {
        if (m_CoroutineContainer == null)
        {
            Debug.LogWarning("Coroutine container not configured... did you forget to call Init?");
            return;
        }

        if (m_Tween != null)
        {
            m_CoroutineContainer.StopCoroutine(m_Tween);
            m_Tween = null;
        }

        if (!m_CoroutineContainer.gameObject.activeInHierarchy)
        {
            info.TweenValue(1.0f);
            return;
        }

        m_Tween = Start(info);
        info.endState = false;
        m_CoroutineContainer.StartCoroutine(m_Tween);
    }
}

