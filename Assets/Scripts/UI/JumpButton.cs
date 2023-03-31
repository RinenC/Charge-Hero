using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isDown;

    public Action<bool> buttonCb;

    public bool clicked;

    public void OnClickEvent()
    {
        GameManager.instance.go_Player.GetComponent<PlayerControl>().Jump_Input();
    }

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
