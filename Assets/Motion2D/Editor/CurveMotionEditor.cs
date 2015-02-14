///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   CurveMotionEditor.cs                                                             //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   ���񃂁[�V�����G�f�B�^�g���B                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

/// <summary>
/// ���񃂁[�V�����G�f�B�^�g��
/// </summary>
[CustomEditor(typeof(CurveMotion))]
public class CurveMotionEditor : Editor {
    /// <summary>
    /// CurveMotion�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        SerializedMotionEditor.OnInspectorGUI(serializedObject, SerializedMotion.MotionType.Curve);
        serializedObject.ApplyModifiedProperties();
    }
}
