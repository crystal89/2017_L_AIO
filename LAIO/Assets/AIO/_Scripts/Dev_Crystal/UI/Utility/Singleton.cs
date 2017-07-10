using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //实例对象
    private static T instance;

    public static T Instance
    {
        get {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.Log(typeof(T) + " was no attached Gameobject");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        this.CheckInstance();
    }    

    //是否唯一，如果不是唯一则销毁多余的
    protected bool CheckInstance()
    {
        if (this == Instance)
            return true;
        Destroy(this);
        return false;
    }
}
