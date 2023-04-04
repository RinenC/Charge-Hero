using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class HorizontalLineAttribute : PropertyAttribute
{
    public enum eLineType
    {
        Single,
        Double,
    }

    public const float DefaultHeight = 1f;

    public float Height { get; private set; }
    public eLineType LineType = eLineType.Single;

    public HorizontalLineAttribute(eLineType lineType = eLineType.Single)
    {
        Height = DefaultHeight;
        LineType = lineType;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
public class HorizontalLinePropertyDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        //HorizontalLineAttribute lineAttr = (HorizontalLineAttribute)attribute;
        return EditorGUIUtility.singleLineHeight;// + lineAttr.Height;
    }

    public override void OnGUI(Rect position)
    {
        HorizontalLineAttribute lineAttr = (HorizontalLineAttribute)attribute;

        Rect rect = EditorGUI.IndentedRect(position);
        rect.x += 1;
        rect.width -= 2;

        if (lineAttr.LineType == HorizontalLineAttribute.eLineType.Single)
        {
            rect.y += (EditorGUIUtility.singleLineHeight / 2f) - lineAttr.Height;
            rect.height = lineAttr.Height;
            EditorGUI.DrawRect(rect, Color.gray);
        }
        else
        {
            float emptyYSize = (EditorGUIUtility.singleLineHeight - lineAttr.Height * 2) / 5f;

            rect.y += emptyYSize * 2;
            rect.height = lineAttr.Height;
            EditorGUI.DrawRect(rect, Color.gray);

            rect.y += emptyYSize;
            EditorGUI.DrawRect(rect, Color.gray);
        }
    }
}
#endif
