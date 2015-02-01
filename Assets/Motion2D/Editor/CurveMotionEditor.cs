///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   CurveMotionEditor.cs                                                             //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   ���񃂁[�V�����G�f�B�^�g���B                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
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

        var fromCurrent = serializedObject.FindProperty("fromCurrent");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fromCurrent"));

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("from"));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("fromAngle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateAngle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("radius"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));

        serializedObject.ApplyModifiedProperties();
    }
}
