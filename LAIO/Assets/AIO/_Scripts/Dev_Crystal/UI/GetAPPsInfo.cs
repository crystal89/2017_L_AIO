using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAPPsInfo : MonoBehaviour
{
    private Dictionary<string, string> m_AppName;
    public Dictionary<string, string> appName
    {
        get
        {
            if (m_AppName == null)
                m_AppName = new Dictionary<string, string>();
            return m_AppName;
        }
        private set { m_AppName = value; }
    }
    private Dictionary<string, Texture2D> m_AppIcon;
    public Dictionary<string, Texture2D> appIcon
    {
        get
        {
            if (m_AppIcon == null)
                m_AppIcon = new Dictionary<string, Texture2D>();
            return m_AppIcon;
        }
        private set { m_AppIcon = value; }
    }

    //获取AndroidInfo - PackageManager
    private void GetAndroidInfo()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject pm = jo.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject unityplug = new AndroidJavaObject("com.huaqin.appinfo.UnityPlug");
            AndroidJavaObject[] androidInfo = unityplug.Call<AndroidJavaObject[]>("getAppinfoFromAndroid", pm);

            //将获取的appname、pkgname、icon信息存储在Dictionary中
            for (int i = 0; i < androidInfo.Length; i++)
            {
                appName.Add(androidInfo[i].Call<string>("getPkgName"), androidInfo[i].Call<string>("getAppLabel"));

                byte[] icon = androidInfo[i].Call<byte[]>("getAppIcon");
                Texture2D t2d = new Texture2D(80, 80);
                t2d.LoadImage(icon);
                appIcon.Add(androidInfo[i].Call<string>("getPkgName"), t2d);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }


    public GameObject perfab;
    private void Start()
    {
        GetAndroidInfo();

        if (appIcon.Count > 0)
        {
            foreach (KeyValuePair<string, Texture2D> key in appIcon)
            {
                Transform appTrans = Instantiate(perfab).transform;
                appTrans.localScale = Vector3.one;
                appTrans.localRotation = Quaternion.identity;
                appTrans.SetParent(transform);
                appTrans.GetComponent<Image>().sprite = Sprite.Create(key.Value, new Rect(0, 0, key.Value.texelSize.x, key.Value.texelSize.y), Vector2.zero);
                appTrans.GetComponentInChildren<Text>().text = key.Key;
            }
        }
    }
}
