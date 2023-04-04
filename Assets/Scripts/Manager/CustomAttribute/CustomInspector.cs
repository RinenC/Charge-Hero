using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(UnityEngine.Object), true), CanEditMultipleObjects]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GetAllMethods(target).Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0).Any())
        {
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            DrawButtons();
        }
    }

    void DrawButtons()
    {
        IEnumerable<MethodInfo> _methods = GetAllMethods(target).Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);

        if (_methods.Any())
        {
            foreach (MethodInfo method in _methods)
            {
                Button(serializedObject.targetObject, method);
            }
        }
    }

    IEnumerable<MethodInfo> GetAllMethods(object target)
    {
        if (target == null)
            yield break;

        List<Type> types = GetSelfAndBaseTypes(target);

        for (int i = 0; i < types.Count; i++)
        {
            MethodInfo[] methodInfos = types[i].GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (MethodInfo methodInfo in methodInfos)
            {
                yield return methodInfo;
            }
        }
    }

    void Button(UnityEngine.Object target, MethodInfo mInfo)
    {
        GUIStyle btnStyle = new GUIStyle(GUI.skin.button) { richText = true };

        if (mInfo.GetParameters().All(p => p.IsOptional))
        {
            ButtonAttribute buttonAttribute = (ButtonAttribute)mInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
            string btnText = string.IsNullOrEmpty(buttonAttribute.Text) ? ObjectNames.NicifyVariableName(mInfo.Name) : buttonAttribute.Text;

            bool btnFlag = true;

            ButtonAttribute.ButtonEnableMode mode = buttonAttribute.SelectedEnableMode;
            btnFlag &= mode == ButtonAttribute.ButtonEnableMode.Always
                       || mode == ButtonAttribute.ButtonEnableMode.Editor && !Application.isPlaying
                       || mode == ButtonAttribute.ButtonEnableMode.Playmode && Application.isPlaying;

            bool isCo = mInfo.ReturnType == typeof(IEnumerator);
            if (isCo)
                btnFlag &= Application.isPlaying;

            EditorGUI.BeginDisabledGroup(!btnFlag);

            if (GUILayout.Button(btnText, btnStyle))
            {
                object[] defaultParams = mInfo.GetParameters().Select(p => p.DefaultValue).ToArray();
                IEnumerator methodResult = mInfo.Invoke(target, defaultParams) as IEnumerator;

                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(target);

                    PrefabStage pfStage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (pfStage != null)
                        EditorSceneManager.MarkSceneDirty(pfStage.scene);
                    else
                        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                }
                else if (methodResult != null && target is MonoBehaviour behaviour)
                {
                    behaviour.StartCoroutine(methodResult);
                }
            }

            EditorGUI.EndDisabledGroup();
        }
        //else
        //{
        //    string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
        //    Logger.LogError(warning);
        //}
    }

    List<Type> GetSelfAndBaseTypes(object target)
    {
        List<Type> types = new List<Type>()
            {
                target.GetType()
            };

        while (types.Last().BaseType != null)
        {
            types.Add(types.Last().BaseType);
        }

        return types;
    }
}
#endif
