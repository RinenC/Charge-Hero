using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class LabelAttribute : PropertyAttribute
{
    string ViewName = string.Empty;

    public LabelAttribute(string _viewName)
    {
        ViewName = _viewName;
    }

    public string GetViewName()
    {
        return ViewName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LabelAttribute))]
public class LabelPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var MyAttribute = attribute as LabelAttribute;
        var viewName = MyAttribute.GetViewName();

        //if (string.IsNullOrEmpty(viewName))
        //    EditorGUI.PropertyField(position, property, label);
        //else
        EditorGUI.PropertyField(position, property, new GUIContent(viewName, label.text));
    }
}
#endif