using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isDown;

    public Action<bool> buttonCb;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;

        buttonCb?.Invoke(isDown);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;

        buttonCb?.Invoke(isDown);
    }

    public void SetCb(Action<bool> _buttonCb)
    {
        buttonCb = _buttonCb;
    }
}
