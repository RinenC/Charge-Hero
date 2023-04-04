using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 매개변수가 없거나, Optional한 매개변수만 있는경우만 사용가능.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ButtonAttribute : Attribute
{
    public enum ButtonEnableMode
    {
        Always,
        Editor,
        Playmode,
    }

    public string Text { get; private set; }
    public ButtonEnableMode SelectedEnableMode { get; private set; }

    /// <summary>
    /// 매개변수가 없거나, Optional한 매개변수만 있는경우만 사용가능.
    /// </summary>
    public ButtonAttribute(string text = null, ButtonEnableMode enabledMode = ButtonEnableMode.Playmode)
    {
        Text = text;
        SelectedEnableMode = enabledMode;
    }
}
