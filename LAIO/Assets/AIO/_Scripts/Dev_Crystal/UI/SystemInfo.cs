using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfo
{
    private static SystemInfo instance = new SystemInfo();
    public static SystemInfo GetInstance()
    {
        if (instance == null)
            instance = new SystemInfo();
        return instance;
    }

    public virtual void Do() { }
}

public class SysBattery : SystemInfo
{
    internal static SysBattery instance = new SysBattery();
    internal static new SysBattery GetInstance()
    {
        if (instance == null)
            instance = new SysBattery();
        return instance;
    }

    public override void Do()
    {
        base.Do();
    }
}