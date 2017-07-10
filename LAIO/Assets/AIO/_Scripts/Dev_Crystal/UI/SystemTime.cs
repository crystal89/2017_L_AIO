using System;

public class SystemTime
{
    string hour = string.Empty;
    string minute = string.Empty;
    //string second = string.Empty;

    private static SystemTime instance;
    public static SystemTime GetInstance()
    {
        if (instance == null)
        {
            instance = new SystemTime();
        }
        return instance;
    }

    //获取系统设置的时间显示格式->24/12小时制

    public string Hour
    {
        get { return hour; }
    }
    public string Minute
    {
        get { return minute; }
    }
    
    public void GetTime()
    {
        DateTime dateTime = DateTime.Now;
        hour = "00";
        if (dateTime.Hour < 10)
        {
            hour = "0" + dateTime.Hour;
        }
        else
        {
            hour = dateTime.Hour.ToString();
        }
        minute = "00";
        if (dateTime.Minute < 10)
        {
            minute = "0" + dateTime.Minute;
        }
        else
        {
            minute = dateTime.Minute.ToString();
        }
        //second = "00";
        //if (dateTime.Second < 10)
        //{
        //    second = "0" + dateTime.Second;
        //}
        //else
        //{
        //    second = dateTime.Second.ToString();
        //}

        //switch (flag)
        //{
        //    case 0:
        //        time = hour + ":" + minute + ":" + second;
        //        break;
        //    case 1:
        //        time = hour + ":" + minute;
        //        break;
        //    default:
        //        time = "获取时间失败！";
        //        break;
        //}
        //return time;
    }
}
