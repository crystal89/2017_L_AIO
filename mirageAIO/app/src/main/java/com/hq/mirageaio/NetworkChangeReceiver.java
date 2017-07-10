package com.hq.mirageaio;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.widget.Toast;

/**
 * Created by Crystal on 2017/6/16.
 * 监听网络状态的变化，通知Unity
 */

public class NetworkChangeReceiver extends BroadcastReceiver {

    public NetworkChangeListener networkChangeListener;

    @Override
    public void onReceive(Context context, Intent intent) {
        ConnectivityManager connectivityManager = (ConnectivityManager) context.getSystemService(context.CONNECTIVITY_SERVICE);
        NetworkInfo networkInfo = connectivityManager.getActiveNetworkInfo();
        if (networkInfo != null && networkInfo.isAvailable()) {
            Toast.makeText(context, networkInfo.getState().toString(), Toast.LENGTH_SHORT).show();
        } else {
            Toast.makeText(context, "network is unavailable", Toast.LENGTH_SHORT).show();
        }
        networkChangeListener.onNetworkChangeListener("当前网络状态：" + networkInfo.getState().toString());
    }

    public interface NetworkChangeListener {
        void onNetworkChangeListener(String state);
    }

    public void setNetworkChangeListener(NetworkChangeListener networkChangeListener) {
        this.networkChangeListener = networkChangeListener;
    }
}
