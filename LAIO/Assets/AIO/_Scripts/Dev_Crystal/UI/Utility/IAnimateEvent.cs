using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 定义动画事件 - 左/右/上/下 滑动
/// </summary>
public interface IAnimateEventHandler : IEventSystemHandler
{
    //左移
    void OnMoveLeft(BaseEventData data);

    //右移
    void OnMoveRight(BaseEventData data);

    //上移
    void OnMoveUp(BaseEventData data);

    //下移
    void OnMoveDown(BaseEventData data);
}
