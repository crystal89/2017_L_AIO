using UnityEngine;
using UnityEngine.UI;

public class GetSysTime : MonoBehaviour
{
    private Text _hour;
    private Text _minute;
    private SystemTime systemTime;

    void Start()
    {
        _hour = transform.Find("Hour").GetComponent<Text>();
        _minute = transform.Find("Minute").GetComponent<Text>();

        systemTime = SystemTime.GetInstance();
        InvokeRepeating("GetTime", 1, 10);
        
    }

    private void GetTime()
    {
        systemTime.GetTime();
        _hour.text = systemTime.Hour;
        _minute.text = systemTime.Minute;
    }
}
