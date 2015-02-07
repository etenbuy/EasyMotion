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
        public bool up;
        public bool upPrev;
    };

    private MotionGUI[] motionGui;

    //private void OnGUI() {
    //    for ( int i = 0 ; i < motionGui.Length ; ++i ) {
    //        var gui = motionGui[i];

    //        if ( !gui.upPrev && gui.up ) {
    //            OnUp();
    //        }

    //        gui.upPrev = gui.up;

    //        motionGui[i] = gui;
    //    }

    //    //if ( expansion[i].up && !upPrev ) {
    //    //    // TODO 押した瞬間だけ反応するようにしたい
    //    //    OnUp();
    //    //}

    //}

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

            GUILayout.RepeatButton("Down", GUILayout.Width(60));
            GUILayout.RepeatButton("Insert New", GUILayout.Width(80));
            GUILayout.RepeatButton("Remove", GUILayout.Width(80));
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
    private void OnUp() {
        Debug.Log("OnUp");
    }
}
