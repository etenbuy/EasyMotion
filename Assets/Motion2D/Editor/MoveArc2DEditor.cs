///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveArc2DEditor.cs                                                               //
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
[CustomEditor(typeof(MoveArc2D))]
public class MoveArc2DEditor : Editor {
    /// <summary>
    /// MoveArcのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        SerializedMotion2DEditor.OnInspectorGUI(serializedObject, SerializedMotion2D.MotionType.MoveArc);
        serializedObject.ApplyModifiedProperties();
    }
}
