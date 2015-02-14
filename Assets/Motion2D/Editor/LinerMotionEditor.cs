///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotionEditor.cs                                                             //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   直線モーションエディタ拡張。                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

/// <summary>
/// 直線モーションエディタ拡張
/// </summary>
[CustomEditor(typeof(LinerMotion))]
public class LinerMotionEditor : Editor {
    /// <summary>
    /// LinerMotionのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        SerializedMotionEditor.OnInspectorGUI(serializedObject, SerializedMotion.MotionType.Line);
        serializedObject.ApplyModifiedProperties();
    }
}
