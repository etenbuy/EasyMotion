///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotionEditor.cs                                                             //
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
[CustomEditor(typeof(LinerMotion))]
public class LinerMotionEditor : Editor {
    /// <summary>
    /// ������
    /// </summary>
    private void Awake() {
        SceneView.onSceneGUIDelegate += OnSceneView;
    }

    /// <summary>
    /// �㏈��
    /// </summary>
    private void OnDestroy() {
        SceneView.onSceneGUIDelegate -= OnSceneView;
    }

    /// <summary>
    /// LinerMotion�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        var fromCurrent = serializedObject.FindProperty("fromCurrent");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fromCurrent"));

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("from"));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("to"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// �O�Ղ̕`��
    /// </summary>
    /// <param name="sceneView"></param>
    private void OnSceneView(SceneView sceneView) {
        Handles.BeginGUI();

        var from = HandleUtility.WorldToGUIPoint(serializedObject.FindProperty("from").vector2Value);
        var to = HandleUtility.WorldToGUIPoint(serializedObject.FindProperty("to").vector2Value);

        Handles.DrawLine(from, to);

        Handles.EndGUI();
    }
}
