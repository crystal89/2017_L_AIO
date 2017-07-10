using System;
using System.Collections;
using UnityEngine;

public class SystemBattery : MonoBehaviour {

    AndroidJavaClass jc;
    AndroidJavaObject jo;
    int batteryLevel;

    //string battery = string.Empty;

    void Start () {
        jc = new AndroidJavaClass("com.unity3d.palyer.unityplayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        jo = new AndroidJavaObject("com.hq.laio.battery", jo);
        batteryLevel = jo.Call<int>("GetBatteryLevel");
	}
    //void Start()
    //{
    //    StartCoroutine(GetBattery());
    //}

    private void OnGUI()
    {
        GUILayout.Label(batteryLevel.ToString());
    }

    private IEnumerator GetBattery()
    {
        while (true)
        {
            //battery = GetBatteryLevel().ToString();
            yield return new WaitForSeconds(300f);
        }
    }
    private int GetBatteryLevel()
    {
        try
        {
            string CapacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
            return int.Parse(CapacityString);
        }
        catch (Exception e)
        {
            Debug.Log("Failed to read battery power; " + e.Message);
        }
        return -1;
    }
}
