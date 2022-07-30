using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoneCopy : EditorWindow
{
    static BoneCopy _window;

    private Object _CopyData;
    private Object _CopyTarget;

    [MenuItem("SimG/Bone")]
    static void Open()
    {
        if(_window == null)
        {
            _window = CreateInstance<BoneCopy>();
        }
        _window.Show();
    }

    private void OnGUI()
    {
        _CopyData = EditorGUILayout.ObjectField("복사할데이터", _CopyData, typeof(GameObject), true);

        _CopyTarget = EditorGUILayout.ObjectField("덮어쓰기할오브젝트", _CopyTarget, typeof(GameObject), true);

        if(GUILayout.Button("복사하기", GUILayout.Width(128)))
        {
            if(_CopyData != null && _CopyTarget != null)
            {
                var data = _CopyData as GameObject;
                var data_transform =  data.GetComponentsInChildren<Transform>();

                var target = _CopyTarget as GameObject;
                var target_transform = target.GetComponentsInChildren<Transform>();

                if (data_transform.Length.Equals(target_transform.Length))
                {
                    for (int i = 1; i < data_transform.Length; i++)
                    {
                        target_transform[i].localPosition = data_transform[i].localPosition;
                        target_transform[i].localRotation = data_transform[i].localRotation;
                    }
                }
            }
        }
    }
}
