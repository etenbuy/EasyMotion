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
    private bool[] expansion;

    /// <summary>
    /// MotionSequenceのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // 各モーションの格納状態更新
        var sequence = serializedObject.FindProperty("sequence");
        var arraySize = sequence.arraySize;

        if ( expansion == null || expansion.Length != arraySize ) {
            expansion = new bool[arraySize];
        }

        // 配列UIを動的表示する
        for ( int i = 0 ; i < arraySize ; ++i ) {
            var elem = sequence.GetArrayElementAtIndex(i);

            // 各動きのヘッダ表示
            GUILayout.BeginHorizontal();
            expansion[i] = EditorGUILayout.Foldout(expansion[i], "Motion" + (i + 1));

            GUILayout.RepeatButton("Up", GUILayout.Width(60));
            GUILayout.RepeatButton("Down", GUILayout.Width(60));
            GUILayout.RepeatButton("Insert New", GUILayout.Width(80));
            GUILayout.RepeatButton("Remove", GUILayout.Width(80));
            GUILayout.EndHorizontal();

            if ( !expansion[i] ) {
                continue;
            }

            ++EditorGUI.indentLevel;

            // モーション共通のGUI表示
            EditorGUILayout.PropertyField(elem.FindPropertyRelative("type"));
            EditorGUILayout.PropertyField(elem.FindPropertyRelative("delay"));
            EditorGUILayout.PropertyField(elem.FindPropertyRelative("duration"));
            EditorGUILayout.PropertyField(elem.FindPropertyRelative("from"));

            // モーション個別のGUI表示
            switch ( (MotionSequence.MotionType)elem.FindPropertyRelative("type").enumValueIndex ) {
            case MotionSequence.MotionType.Base:
                // 基本
                break;

            case MotionSequence.MotionType.Line:
                // 直線
                EditorGUILayout.PropertyField(elem.FindPropertyRelative("to"));
                break;

            case MotionSequence.MotionType.Curve:
                // 旋回
                EditorGUILayout.PropertyField(elem.FindPropertyRelative("fromAngle"));
                EditorGUILayout.PropertyField(elem.FindPropertyRelative("rotateAngle"));
                EditorGUILayout.PropertyField(elem.FindPropertyRelative("radius"));
                break;

            default:
                break;
            }

            --EditorGUI.indentLevel;
        }

        if ( arraySize == 0 ) {
            // 動きが存在しない場合は新規追加ボタンのみ表示
            GUILayout.RepeatButton("Insert New", GUILayout.Width(80));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
