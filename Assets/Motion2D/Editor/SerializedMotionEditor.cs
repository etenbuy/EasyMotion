///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SerializedMotionEditor.cs                                                        //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   シリアライズ済みモーションエディタ。                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections;

/// <summary>
/// シリアライズ済みモーションエディタ。
/// </summary>
public class SerializedMotionEditor {
    /// <summary>
    /// SerializedMotionのインスペクタ上のレイアウト
    /// </summary>
    public static void OnInspectorGUI(SerializedProperty propery) {
        // モーション共通のGUI表示
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("type"));
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("delay"));
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("duration"));

        var fromCurrent = propery.FindPropertyRelative("fromCurrent");
        EditorGUILayout.PropertyField(fromCurrent);

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("from"));
        EditorGUI.EndDisabledGroup();

        // モーション個別のGUI表示
        switch ( (SerializedMotion.MotionType)propery.FindPropertyRelative("type").enumValueIndex ) {
        case SerializedMotion.MotionType.Line:
            // 直線
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("to"));
            break;

        case SerializedMotion.MotionType.Curve:
            // 旋回
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("fromAngle"));
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("rotateAngle"));
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("radius"));
            break;
        }
    }
}
