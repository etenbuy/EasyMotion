///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveTo2DEditor.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   �������[�V�����G�f�B�^�g���B                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

/// <summary>
/// �������[�V�����G�f�B�^�g��
/// </summary>
[CustomEditor(typeof(MoveTo2D))]
public class MoveTo2DEditor : Editor {
    /// <summary>
    /// MoveTo�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        SerializedMotion2DEditor.OnInspectorGUI(serializedObject, SerializedMotion2D.MotionType.MoveTo);
        serializedObject.ApplyModifiedProperties();
    }
}
