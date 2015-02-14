///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotion2DEditor.cs                                                           //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   直線移動のエディタ拡張。                                                         //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using System.Collections;

/// <summary>
/// 直線移動のエディタ拡張クラス。
/// </summary>
[CustomEditor(typeof(LinerMotion2D))]
public class LinerMotion2DEditor : Editor {
    /// <summary>
    /// LinerMotionのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // モーション共通のGUI表示
        EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));

        var fromCurrent = serializedObject.FindProperty("fromCurrent");
        EditorGUILayout.PropertyField(fromCurrent);

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("from"));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("velocity"));

        serializedObject.ApplyModifiedProperties();
    }
}
