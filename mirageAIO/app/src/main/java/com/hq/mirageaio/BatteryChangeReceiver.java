package com.hq.mirageaio;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

/**
 * Created by Crystal on 2017/6/16.
 * 监听电池电量的变化，通知Unity
 */

public class BatteryChangeReceiver extends BroadcastReceiver {

    public BatteryChangeListener batteryChangeListener;

    @Override
    public void onReceive(Context context, Intent intent) {
        //获取当前电量
        int currentLevel = intent.getIntExtra("level", 0);
        //总电量
        int totalLevel = intent.getIntExtra("scale", 0);

        String info = "当前电量：" + (currentLevel / totalLevel) * 100 + "%";

        switch (intent.getAction()) {
            case Intent.ACTION_BATTERY_OKAY:
                Toast.makeText(context, "电量已充满", Toast.LENGTH_SHORT).show();
                break;
            case Intent.ACTION_BATTERY_CHANGED:
                Toast.makeText(context, info, Toast.LENGTH_SHORT).show();
                break;
            case Intent.ACTION_BATTERY_LOW:
                Toast.makeText(context, "电量过低，请及时充电", Toast.LENGTH_SHORT).show();
                break;
        }
        batteryChangeListener.onBatteryChangeListener(info);
        Log.i("Battery Level Change : ", currentLevel + " , " + totalLevel);
    }


    //设置监听回调，将电量传递给activity
    public interface BatteryChangeListener {
        void onBatteryChangeListener(String level);
    }

    public void setBatteryChangeListener(BatteryChangeListener batteryChangeListener) {
        this.batteryChangeListener = batteryChangeListener;
    }
}