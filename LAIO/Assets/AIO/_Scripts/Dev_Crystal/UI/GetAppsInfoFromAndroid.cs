using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAppsInfoFromAndroid : MonoBehaviour
{
    AndroidJavaClass jc;
    AndroidJavaObject jo;
    AndroidJavaObject packageInfo;

    int flag;
    new string name;

    void Start()
    {
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        InvokeRepeating("GetPackageInfoFromAndroid", 1, 5);
    }

    private void GetPackageInfoFromAndroid()
    {
        packageInfo = jo.Call<AndroidJavaObject>("GetPackageInfo");
        flag = packageInfo.Call<int>("getFlag");
        name = packageInfo.Call<string>("getName");

    }

    private void OnGUI()
    {
        GUILayout.Label(flag + ":" + name);
    }
}
