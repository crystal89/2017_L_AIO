package com.hq.mirageaio;

import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.widget.Toast;

import com.unity3d.player.UnityPlayerActivity;

/**
 * Created by Crystal on 2017/6/16.
 */

public class MainActivity extends UnityPlayerActivity implements BatteryChangeReceiver.BatteryChangeListener, NetworkChangeReceiver.NetworkChangeListener, BootReceiver.PackageChangeListener {

    private BootReceiver bootReceiver;
    private PackageChangeInfo packageChangeInfo = new PackageChangeInfo();

    private NetworkChangeReceiver networkChangeReceiver;
    private String networkState = null;

    private BatteryChangeReceiver batteryChangeReceiver;
    private String batteryLevel = null;

    @Override
    protected void onCreate(Bundle bundle) {
        super.onCreate(bundle);
        try {
            //注册系统应用监听
            IntentFilter intentFilter_package = new IntentFilter();
            intentFilter_package.addAction(Intent.ACTION_PACKAGE_ADDED);
            intentFilter_package.addAction(Intent.ACTION_PACKAGE_REMOVED);
            bootReceiver = new BootReceiver();
            registerReceiver(bootReceiver, intentFilter_package);
            bootReceiver.setPackageChangeListener(this);

            //注册网络监听
            IntentFilter intentFilter_network = new IntentFilter();
            intentFilter_network.addAction("android.net.conn.CONNECTIVITY_CHANGE");
            networkChangeReceiver = new NetworkChangeReceiver();
            registerReceiver(networkChangeReceiver, intentFilter_network);
            networkChangeReceiver.setNetworkChangeListener(this);

            //注册电池状态监听
            IntentFilter intentFilter_battery = new IntentFilter();
            intentFilter_battery.addAction(Intent.ACTION_BATTERY_CHANGED);
            intentFilter_battery.addAction(Intent.ACTION_BATTERY_LOW);
            intentFilter_battery.addAction(Intent.ACTION_BATTERY_OKAY);
            batteryChangeReceiver = new BatteryChangeReceiver();
            registerReceiver(batteryChangeReceiver, intentFilter_battery);
            batteryChangeReceiver.setBatteryChangeListener(this);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onPackageChangeListener(int flag, String pk_name) {
        packageChangeInfo.setFlag(flag);
        packageChangeInfo.setName(pk_name);
    }

    public PackageChangeInfo GetPackageInfo() {
        return packageChangeInfo;
    }

    @Override
    public void onNetworkChangeListener(String state) {
        networkState = state;
    }

    public String getNetworkState() {
        return networkState;
    }

    @Override
    public void onBatteryChangeListener(String level) {
        batteryLevel = level;
    }

    public String getBatteryLevel() {
        return batteryLevel;
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        //结束监听
        unregisterReceiver(bootReceiver);
        unregisterReceiver(networkChangeReceiver);
        unregisterReceiver(batteryChangeReceiver);
    }

    // 定义一个显示Toast的方法，在Unity中调用此方法
    public void ShowToast(final String mStr2Show) {
        // 同样需要在UI线程下执行
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(getApplicationContext(), mStr2Show, Toast.LENGTH_LONG).show();
            }
        });
    }
}
