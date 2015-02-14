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

        if ( expansion == null ) {
            expansion = new bool[sequence.arraySize];
        }

        // 配列UIを動的表示する
        for ( int i = 0 ; i < sequence.arraySize ; ++i ) {
            var elem = sequence.GetArrayElementAtIndex(i);

            // 各動きのヘッダ表示
            GUILayout.BeginHorizontal();
            expansion[i] = EditorGUILayout.Foldout(expansion[i], "Motion" + (i + 1));

            if ( GUILayout.Button("Up", GUILayout.Width(60)) ) {
                OnUp(i);
            }
            if ( GUILayout.Button("Down", GUILayout.Width(60)) ) {
                OnDown(i);
            }
            if ( GUILayout.Button("Insert New", GUILayout.Width(80)) ) {
                OnInsertNew(i);
                serializedObject.Update();
            }
            if ( GUILayout.Button("Remove", GUILayout.Width(80)) ) {
                OnRemove(i--);
                GUILayout.EndHorizontal();
                serializedObject.Update();
                continue;
            }
            GUILayout.EndHorizontal();

            if ( !expansion[i] ) {
                // 折りたたまれていたら以降は非表示
                continue;
            }

            ++EditorGUI.indentLevel;

            // モーション情報のGUI表示
            SerializedMotionEditor.OnInspectorGUI(elem);

            --EditorGUI.indentLevel;
        }

        if ( sequence.arraySize == 0 ) {
            // 動きが存在しない場合は新規追加ボタンのみ表示
            if ( GUILayout.RepeatButton("Insert New", GUILayout.Width(80)) ) {
                OnInsertNew(0);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 上移動ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnUp(int index) {
        Debug.Log("OnUp : " + index);

        if ( index == 0 ) {
            return;
        }

        Replace(index, index - 1);
    }

    /// <summary>
    /// 下移動ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnDown(int index) {
        Debug.Log("OnDown : " + index);

        if ( index >= expansion.Length - 1 ) {
            return;
        }

        Replace(index, index + 1);
    }

    /// <summary>
    /// 挿入ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnInsertNew(int index) {
        Debug.Log("OnInsertNew : " + index);

        (target as MotionSequence).InsertNew(index);

        var newExpansion = new List<bool>(expansion);
        newExpansion.Insert(index, false);
        expansion = newExpansion.ToArray();
    }

    /// <summary>
    /// 削除ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnRemove(int index) {
        Debug.Log("OnRemove : " + index);

        (target as MotionSequence).Remove(index);

        var newExpansion = new List<bool>(expansion);
        newExpansion.RemoveAt(index);
        expansion = newExpansion.ToArray();
    }

    /// <summary>
    /// モーションを入れ替える
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    private void Replace(int index1, int index2) {
        (target as MotionSequence).Replace(index1, index2);

        var tmp = expansion[index1];
        expansion[index1] = expansion[index2];
        expansion[index2] = tmp;
    }
}
