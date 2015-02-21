///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   AdjustSpeedWindow.cs                                                             //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.20                                                                       //
//  Desc    :   スピード補正用ウィンドウ。                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// スピード補正用ウィンドウ。
/// </summary>
public class AdjustSpeedWindow : EditorWindow {
    public float speed = 0;

    public delegate void OnOk(float speed);
    public OnOk onOk;

    public static void Open(float speed, OnOk onOk) {
        var window = EditorWindow.GetWindow<AdjustSpeedWindow>();
        window.speed = speed;
        window.onOk = onOk;
    }

    private void OnGUI() {
        speed = EditorGUILayout.FloatField("Speed", speed);

        GUILayout.BeginHorizontal();
        if ( GUILayout.Button("OK") ) {
            Close();
            if ( onOk != null ) {
                onOk(speed);
            }
        }
        if ( GUILayout.Button("Cancel") ) {
            Close();
        }
        GUILayout.EndHorizontal();
    }
}
#endif