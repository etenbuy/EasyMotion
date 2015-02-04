///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SequenceMotionEditor.cs                                                          //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   連続したモーションのエディタ拡張。                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;
using System.Collections.Generic;

/// <summary>
/// 連続したモーションのエディタ拡張。
/// </summary>
[CustomEditor(typeof(MotionSequence))]
public class SequenceMotionEditor : Editor {
    /// <summary>
    /// MotionSequenceのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        //DrawDefaultInspector();

        serializedObject.Update();

        // 配列UIを動的表示する
        var sequence = serializedObject.FindProperty("sequence");
        var arraySize = sequence.arraySize;

        for ( int i = 0 ; i < arraySize ; ++i ) {
            //EditorGUILayout.PropertyField(sequence.GetArrayElementAtIndex(i), true);

            var elem = sequence.GetArrayElementAtIndex(i);

            switch ( (MotionSequence.MotionType)elem.FindPropertyRelative("type").enumValueIndex ) {
            case MotionSequence.MotionType.Line:
                DrawLineGUI(elem);
                break;

            default:
                EditorGUILayout.PropertyField(elem, true);
                break;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 直線移動のGUI描画
    /// </summary>
    /// <param name="prop"></param>
    private void DrawLineGUI(SerializedProperty prop) {
        EditorGUILayout.LabelField("Element");
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("type"));
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("delay"));
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("duration"));
        EditorGUILayout.PropertyField(prop.FindPropertyRelative("from"));
    }
}
