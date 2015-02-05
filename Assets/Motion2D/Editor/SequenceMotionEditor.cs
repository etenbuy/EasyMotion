///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SequenceMotionEditor.cs                                                          //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   �A���������[�V�����̃G�f�B�^�g���B                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;
using System.Collections.Generic;

/// <summary>
/// �A���������[�V�����̃G�f�B�^�g���B
/// </summary>
[CustomEditor(typeof(MotionSequence))]
public class SequenceMotionEditor : Editor {
    private bool[] expansion;

    /// <summary>
    /// MotionSequence�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // �e���[�V�����̊i�[��ԍX�V
        var sequence = serializedObject.FindProperty("sequence");
        var arraySize = sequence.arraySize;

        if ( expansion == null || expansion.Length != arraySize ) {
            expansion = new bool[arraySize];
        }

        // �z��UI�𓮓I�\������
        for ( int i = 0 ; i < arraySize ; ++i ) {
            var elem = sequence.GetArrayElementAtIndex(i);

            // �e�����̃w�b�_�\��
            GUILayout.BeginHorizontal();
            expansion[i] = EditorGUILayout.Foldout(expansion[i], "Motion" + (i + 1));

            GUILayout.RepeatButton("Up", GUILayout.Width(60));
            GUILayout.RepeatButton("Down", GUILayout.Width(60));
            GUILayout.RepeatButton("Insert New", GUILayout.Width(80));
            GUILayout.RepeatButton("Remove", GUILayout.Width(80));
            GUILayout.EndHorizontal();

            if ( !expansion[i] ) {
                continue;
            }

            ++EditorGUI.indentLevel;

            // ���[�V�������ʂ�GUI�\��
            EditorGUILayout.PropertyField(elem.FindPropertyRelative("type"));
            EditorGUILayout.PropertyField(elem.FindPropertyRelative("delay"));
            EditorGUILayout.PropertyField(elem.FindPropertyRelative("duration"));
            EditorGUILayout.PropertyField(elem.FindPropertyRelative("from"));

            // ���[�V�����ʂ�GUI�\��
            switch ( (MotionSequence.MotionType)elem.FindPropertyRelative("type").enumValueIndex ) {
            case MotionSequence.MotionType.Base:
                // ��{
                break;

            case MotionSequence.MotionType.Line:
                // ����
                EditorGUILayout.PropertyField(elem.FindPropertyRelative("to"));
                break;

            case MotionSequence.MotionType.Curve:
                // ����
                EditorGUILayout.PropertyField(elem.FindPropertyRelative("fromAngle"));
                EditorGUILayout.PropertyField(elem.FindPropertyRelative("rotateAngle"));
                EditorGUILayout.PropertyField(elem.FindPropertyRelative("radius"));
                break;

            default:
                break;
            }

            --EditorGUI.indentLevel;
        }

        if ( arraySize == 0 ) {
            // ���������݂��Ȃ��ꍇ�͐V�K�ǉ��{�^���̂ݕ\��
            GUILayout.RepeatButton("Insert New", GUILayout.Width(80));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
