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
    private struct MotionGUI {
        public bool expansion;
        public bool up;
        public bool upPrev;
    };

    private MotionGUI[] motionGui;

    //private void OnGUI() {
    //    for ( int i = 0 ; i < motionGui.Length ; ++i ) {
    //        var gui = motionGui[i];

    //        if ( !gui.upPrev && gui.up ) {
    //            OnUp();
    //        }

    //        gui.upPrev = gui.up;

    //        motionGui[i] = gui;
    //    }

    //    //if ( expansion[i].up && !upPrev ) {
    //    //    // TODO �������u�Ԃ�����������悤�ɂ�����
    //    //    OnUp();
    //    //}

    //}

    /// <summary>
    /// MotionSequence�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // �e���[�V�����̊i�[��ԍX�V
        var sequence = serializedObject.FindProperty("sequence");
        var arraySize = sequence.arraySize;

        if ( motionGui == null ) {
            motionGui = new MotionGUI[arraySize];
        }

        // �z��UI�𓮓I�\������
        for ( int i = 0 ; i < arraySize ; ++i ) {
            var elem = sequence.GetArrayElementAtIndex(i);

            // �e�����̃w�b�_�\��
            GUILayout.BeginHorizontal();
            motionGui[i].expansion = EditorGUILayout.Foldout(motionGui[i].expansion, "Motion" + (i + 1));

            GUILayout.RepeatButton("Down", GUILayout.Width(60));
            GUILayout.RepeatButton("Insert New", GUILayout.Width(80));
            GUILayout.RepeatButton("Remove", GUILayout.Width(80));
            GUILayout.EndHorizontal();

            if ( !motionGui[i].expansion ) {
                // �܂肽���܂�Ă�����ȍ~�͔�\��
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

    /// <summary>
    /// ��ړ��{�^�����N���b�N���ꂽ
    /// </summary>
    private void OnUp() {
        Debug.Log("OnUp");
    }
}
