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
    private struct MotionGUI {
        public bool expansion;
    };

    private MotionGUI[] motionGui;

    /// <summary>
    /// MotionSequenceのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // 各モーションの格納状態更新
        var sequence = serializedObject.FindProperty("sequence");
        var arraySize = sequence.arraySize;

        if ( motionGui == null ) {
            motionGui = new MotionGUI[arraySize];
        }

        // 配列UIを動的表示する
        for ( int i = 0 ; i < arraySize ; ++i ) {
            var elem = sequence.GetArrayElementAtIndex(i);

            // 各動きのヘッダ表示
            GUILayout.BeginHorizontal();
            motionGui[i].expansion = EditorGUILayout.Foldout(motionGui[i].expansion, "Motion" + (i + 1));

            if ( GUILayout.Button("Up", GUILayout.Width(60)) ) {
                OnUp(i);
            }
            if ( GUILayout.Button("Down", GUILayout.Width(60)) ) {
                OnDown(i);
            }
            if ( GUILayout.Button("Insert New", GUILayout.Width(80)) ) {
                OnInsertNew(i);
            }
            if ( GUILayout.Button("Remove", GUILayout.Width(80)) ) {
                OnRemove(i);
            }
            GUILayout.EndHorizontal();

            if ( !motionGui[i].expansion ) {
                // 折りたたまれていたら以降は非表示
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

    /// <summary>
    /// 上移動ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnUp(int index) {
        Debug.Log("OnUp : " + index);
    }

    /// <summary>
    /// 下移動ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnDown(int index) {
        Debug.Log("OnDown : " + index);
    }

    /// <summary>
    /// 挿入ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnInsertNew(int index) {
        Debug.Log("OnInsertNew : " + index);
    }

    /// <summary>
    /// 削除ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnRemove(int index) {
        Debug.Log("OnRemove : " + index);
    }
}
