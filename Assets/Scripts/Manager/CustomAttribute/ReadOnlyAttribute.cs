using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ReadOnlyAttribute : PropertyAttribute
{
    string ViewName = string.Empty;

    public ReadOnlyAttribute() { }

    public ReadOnlyAttribute(string _viewName)
    {
        ViewName = _viewName;
    }

    public string GetViewName()
    {
        return ViewName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;

        var readOnlyAttribute = attribute as ReadOnlyAttribute;
        var viewName = readOnlyAttribute.GetViewName();

        if (string.IsNullOrEmpty(viewName))
            EditorGUI.PropertyField(position, property, label);
        else
            EditorGUI.PropertyField(position, property, new GUIContent(viewName, label.text));

        GUI.enabled = true;
    }
}
#endif