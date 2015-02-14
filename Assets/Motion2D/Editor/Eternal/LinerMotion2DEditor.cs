///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotion2DEditor.cs                                                           //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   �����ړ��̃G�f�B�^�g���B                                                         //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using System.Collections;

/// <summary>
/// �����ړ��̃G�f�B�^�g���N���X�B
/// </summary>
[CustomEditor(typeof(LinerMotion2D))]
public class LinerMotion2DEditor : Editor {
    /// <summary>
    /// LinerMotion�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // ���[�V�������ʂ�GUI�\��
        EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));

        var fromCurrent = serializedObject.FindProperty("fromCurrent");
        EditorGUILayout.PropertyField(fromCurrent);

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("from"));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("velocity"));

        serializedObject.ApplyModifiedProperties();
    }
}
