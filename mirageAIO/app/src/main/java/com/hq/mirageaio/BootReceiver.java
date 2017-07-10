package com.hq.mirageaio;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

/**
 * Created by Crystal on 2017/6/19.
 * 监听应用程序的安装与卸载，通知Unity在应用list中进行增删改
 */

public class BootReceiver extends BroadcastReceiver {
    public PackageChangeListener packageChangeListener;
    private static final int FLAG_DEFAULT_PACKAGE = 0;
    private static final int FLAG_ADD_PACKAGE = 1;
    private static final int FLAG_REMOVE_PACKAGE = 2;

    @Override
    public void onReceive(Context context, Intent intent) {
        Log.i("Boot Receiver intent:",intent.getAction());
        String packageName = "";
        //接收广播，系统启动完成后
        if (Intent.ACTION_BOOT_COMPLETED.equals(intent.getAction())) {
            packageChangeListener.onPackageChangeListener(FLAG_DEFAULT_PACKAGE, null);
            //校对数据库
        }
        //接收广播，有应用被安装
        if (Intent.ACTION_INSTALL_PACKAGE.equals(intent.getAction())) {
            packageName = intent.getDataString().substring(8);
            packageName = "install";
            packageChangeListener.onPackageChangeListener(FLAG_ADD_PACKAGE, packageName);
            //新增package信息到数据库
        }
        //接收广播，有应用被卸载
        if (Intent.ACTION_UNINSTALL_PACKAGE.equals(intent.getAction())) {
            packageName = intent.getDataString().substring(8);
            packageName = "uninstall";
            packageChangeListener.onPackageChangeListener(FLAG_REMOVE_PACKAGE, packageName);
            //删除package信息到数据库
        }
        Log.i("Boot Receiver: ", packageName);
    }

    public interface PackageChangeListener {
        void onPackageChangeListener(int flag, String pk_name);
    }

    public void setPackageChangeListener(PackageChangeListener packageChangeListener) {
        this.packageChangeListener = packageChangeListener;
    }
}
