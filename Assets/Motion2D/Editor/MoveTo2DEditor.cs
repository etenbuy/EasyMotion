///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveTo2DEditor.cs                                                                //
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
[CustomEditor(typeof(MoveTo2D))]
public class MoveTo2DEditor : Editor {
    /// <summary>
    /// MoveToのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        SerializedMotion2DEditor.OnInspectorGUI(serializedObject, SerializedMotion2D.MotionType.MoveTo);
        serializedObject.ApplyModifiedProperties();
    }
}
