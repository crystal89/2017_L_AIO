using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemWiFi
{
    private static SystemWiFi instance;
    public static SystemWiFi GetInstance()
    {
        if (instance == null)
            instance = new SystemWiFi();
        return instance;
    }


    //获取系统wifi状态进行更新

    public int GetWifiState()
    {
        int currentWiFiState = 0;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //提示连接wifi使用
            //wifi未连接
            currentWiFiState = 0;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            //推荐使用wifi
            currentWiFiState = 1;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            currentWiFiState = 2;
        }
        return currentWiFiState;
    }
}
