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
        DrawDefaultInspector();

        serializedObject.Update();

        // TODO 配列UIを動的表示する

        serializedObject.ApplyModifiedProperties();
    }
}
