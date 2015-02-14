///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SerializedMotion2DEditor.cs                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   �V���A���C�Y�ς݃��[�V�����G�f�B�^�B                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections;

/// <summary>
/// �V���A���C�Y�ς݃��[�V�����G�f�B�^�B
/// </summary>
public class SerializedMotion2DEditor {
    /// <summary>
    /// SerializedMotion�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    /// <param name="obj"></param>
    public static void OnInspectorGUI(SerializedObject obj, SerializedMotion2D.MotionType type) {
        // ���[�V�������ʂ�GUI�\��
        EditorGUILayout.PropertyField(obj.FindProperty("delay"));
        EditorGUILayout.PropertyField(obj.FindProperty("duration"));

        var fromCurrent = obj.FindProperty("fromCurrent");
        EditorGUILayout.PropertyField(fromCurrent);

        EditorGUI.BeginDisabledGroup(!fromCurrent.boolValue);
        var relative = obj.FindProperty("relative");
        if ( !fromCurrent.boolValue ) {
            relative.boolValue = false;
        }
        EditorGUILayout.PropertyField(relative);
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(obj.FindProperty("from"));
        EditorGUI.EndDisabledGroup();

        // ���[�V�����ʂ�GUI�\��
        switch ( type ) {
        case SerializedMotion2D.MotionType.MoveTo:
            // ����
            EditorGUILayout.PropertyField(obj.FindProperty("to"));
            break;

        case SerializedMotion2D.MotionType.MoveArc:
            // ����
            EditorGUILayout.PropertyField(obj.FindProperty("fromAngle"));
            EditorGUILayout.PropertyField(obj.FindProperty("rotateAngle"));
            EditorGUILayout.PropertyField(obj.FindProperty("radius"));
            break;
        }
    }

    /// <summary>
    /// SerializedMotion�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    /// <param name="propery"></param>
    public static void OnInspectorGUI(SerializedProperty propery) {
        // ���[�V�������ʂ�GUI�\��
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("type"));
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("delay"));
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("duration"));

        var fromCurrent = propery.FindPropertyRelative("fromCurrent");
        EditorGUILayout.PropertyField(fromCurrent);

        EditorGUI.BeginDisabledGroup(!fromCurrent.boolValue);
        var relative = propery.FindPropertyRelative("relative");
        if ( !fromCurrent.boolValue ) {
            relative.boolValue = false;
        }
        EditorGUILayout.PropertyField(relative);
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("from"));
        EditorGUI.EndDisabledGroup();

        // ���[�V�����ʂ�GUI�\��
        switch ( (SerializedMotion2D.MotionType)propery.FindPropertyRelative("type").enumValueIndex ) {
        case SerializedMotion2D.MotionType.MoveTo:
            // ����
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("to"));
            break;

        case SerializedMotion2D.MotionType.MoveArc:
            // ����
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("fromAngle"));
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("rotateAngle"));
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("radius"));
            break;
        }
    }
}
