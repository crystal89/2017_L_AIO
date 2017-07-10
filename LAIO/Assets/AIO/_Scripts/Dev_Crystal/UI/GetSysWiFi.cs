using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSysWiFi : MonoBehaviour
{
    private Image wifi;
    private SystemWiFi systemWiFi;

    void Start()
    {
        wifi = GetComponent<Image>();
        systemWiFi = SystemWiFi.GetInstance();
        InvokeRepeating("GetWiFiState", 0, 3);
    }

    private void GetWiFiState()
    {
        int currentWiFiState = systemWiFi.GetWifiState();
        //Debug.Log(currentWiFiState);
        switch (currentWiFiState)
        {
            case 0:
                wifi.enabled = false;
                break;
            case 1:
                wifi.enabled = false;
                break;
            case 2:
                wifi.enabled = true;
                break;
        }
    }
}
