///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveArcEditor.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   旋回モーションエディタ拡張。                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

/// <summary>
/// 旋回モーションエディタ拡張
/// </summary>
[CustomEditor(typeof(MoveArc))]
public class MoveArcEditor : Editor {
    /// <summary>
    /// MoveArcのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        SerializedMotionEditor.OnInspectorGUI(serializedObject, SerializedMotion.MotionType.MoveArc);
        serializedObject.ApplyModifiedProperties();
    }
}
