using System.Collections;
using UnityEngine.UI;

public class VolumeSlider : ArcSlider
{
    protected override  void Start()
    {
        base.Start();
        ///GetComponent<Image>().fillAmount = 0.5f;

        //ReSet();
    }

    protected override void ReSet()
    {
        base.ReSet();
    }
}
