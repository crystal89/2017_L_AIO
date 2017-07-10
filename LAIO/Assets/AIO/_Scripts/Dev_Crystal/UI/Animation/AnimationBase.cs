using UnityEngine;
using UnityEngine.Events;

public class AnimationBase : MonoBehaviour
{

    public enum AnimationDirection
    {
        LeftToRight = 0,
        RightToLeft = 1,
        UpToDown = 2,
        DownToUp = 3
    };

    //持续时间
    public float m_Duration = 0.2f;
    protected bool m_IsAnimating = false;

    protected TweenRunner<FloatTween> m_FloatTweenRunner;
    protected TweenRunner<ColorTween> m_ColorTweenRunner;

    protected TweenRunner<VectorTween> m_VectorTweenRunner;

    protected virtual void Awake()
    {
        m_FloatTweenRunner = new TweenRunner<FloatTween>();
        m_FloatTweenRunner.Init(this);

        m_ColorTweenRunner = new TweenRunner<ColorTween>();
        m_ColorTweenRunner.Init(this);

        m_VectorTweenRunner = new TweenRunner<VectorTween>();
        m_VectorTweenRunner.Init(this);
    }

    protected virtual void Start() { }

    protected virtual void OnDestroy() { }

    //float插值
    public void CrossFadeFloat(TweenRunner<FloatTween> tweenRunner, float duration, float start, float end, UnityAction<float> callback)
    {
        CrossFadeFloat(tweenRunner, duration, start, end, callback, null);
    }

    public void CrossFadeFloat(TweenRunner<FloatTween> tweenRunner, float duration, float start, float end, UnityAction<float> callback, UnityAction endCallback)
    {
        var floatTween = new FloatTween { duration = duration, startValue = start, targetValue = end };

        floatTween.AddOnChangedCallback(callback);
        if (endCallback != null)
            floatTween.AddOnEndCallback(endCallback);

        floatTween.ignoreTimeScale = true;
        tweenRunner.StartTween(floatTween);
    }


    // color发生变化的colortween
    public void CrossFadeColor(Color startColor, Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, UnityAction<Color> callback)
    {
        CrossFadeColor(startColor, targetColor, duration, ignoreTimeScale, useAlpha, true, callback);
    }

    // 根据alpha值创建Color
    private Color CreateColorFromAlpha(Color color, float alpha)
    {
        var alphaColor = color;
        alphaColor.a = alpha;
        return alphaColor;
    }

    // alpha发生变化的colorTween
    public void CrossFadeAlpha(Color startColor, float alpha, float duration, bool ignoreTimeScale, UnityAction<Color> callback)
    {
        CrossFadeColor(startColor, CreateColorFromAlpha(startColor, alpha), duration, ignoreTimeScale, true, false, callback);
    }

    //自身动画 color值变
    private void CrossFadeColor(Color startColor, Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB, UnityAction<Color> callback)
    {
        //if (startColor == null || (!useRGB && !useAlpha))
        //    return;

        if (!useRGB && !useAlpha)
            return;

        Color currentColor = startColor;
        if (currentColor.Equals(targetColor))
            return;

        ColorTween.ColorTweenMode mode = (useRGB && useAlpha ?
            ColorTween.ColorTweenMode.All :
            (useRGB ? ColorTween.ColorTweenMode.RGB : ColorTween.ColorTweenMode.Alpha));
        var colorTween = new ColorTween { duration = duration, startColor = startColor, targetColor = targetColor };

        colorTween.AddOnChangedCallback(callback);

        colorTween.ignoreTimeScale = ignoreTimeScale;
        colorTween.tweenMode = mode;

        m_ColorTweenRunner.StartTween(colorTween);
    }

    //vector
    public void CrossFadeVector(TweenRunner<VectorTween> tweenRunner, float duration, Vector3 start, Vector3 end, UnityAction<Vector3> callback)
    {
        CrossFadeVector(tweenRunner, duration, start, end, callback, null);
    }

    public void CrossFadeVector(TweenRunner<VectorTween> tweenRunner, float duration, Vector3 start, Vector3 end, UnityAction<Vector3> callback, UnityAction endCallback)
    {
        var vectorTween = new VectorTween { duration = duration, startValue = start, targetValue = end };

        vectorTween.AddOnChangedCallback(callback);
        if (endCallback != null)
            vectorTween.AddOnEndCallback(endCallback);

        vectorTween.ignoreTimeScale = true;
        tweenRunner.StartTween(vectorTween);
    }
}
