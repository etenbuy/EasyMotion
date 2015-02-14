///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SequenceMotion2DEditor.cs                                                        //
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
[CustomEditor(typeof(MotionSequence2D))]
public class SequenceMotion2DEditor : Editor {
    private bool[] expansion;

    /// <summary>
    /// MotionSequence�̃C���X�y�N�^��̃��C�A�E�g
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // �e���[�V�����̊i�[��ԍX�V
        var sequence = serializedObject.FindProperty("sequence");

        if ( expansion == null ) {
            expansion = new bool[sequence.arraySize];
        }

        // �z��UI�𓮓I�\������
        for ( int i = 0 ; i < sequence.arraySize ; ++i ) {
            var elem = sequence.GetArrayElementAtIndex(i);

            // �e�����̃w�b�_�\��
            GUILayout.BeginHorizontal();
            expansion[i] = EditorGUILayout.Foldout(expansion[i], "Motion" + (i + 1));

            if ( GUILayout.Button("Up", GUILayout.Width(60)) ) {
                OnUp(i);
            }
            if ( GUILayout.Button("Down", GUILayout.Width(60)) ) {
                OnDown(i);
            }
            if ( GUILayout.Button("Insert New", GUILayout.Width(80)) ) {
                OnInsertNew(i);
                serializedObject.Update();
            }
            if ( GUILayout.Button("Remove", GUILayout.Width(80)) ) {
                OnRemove(i--);
                GUILayout.EndHorizontal();
                serializedObject.Update();
                continue;
            }
            GUILayout.EndHorizontal();

            if ( !expansion[i] ) {
                // �܂肽���܂�Ă�����ȍ~�͔�\��
                continue;
            }

            ++EditorGUI.indentLevel;

            // ���[�V��������GUI�\��
            SerializedMotion2DEditor.OnInspectorGUI(elem);

            --EditorGUI.indentLevel;
        }

        // �����ւ̐V�K�ǉ��{�^���\��
        serializedObject.ApplyModifiedProperties();
        if ( GUILayout.Button("Add New", GUILayout.Width(80)) ) {
            OnInsertNew(sequence.arraySize);
        }
    }

    /// <summary>
    /// ��ړ��{�^�����N���b�N���ꂽ
    /// </summary>
    /// <param name="index">���[�V�����̃C���f�b�N�X</param>
    private void OnUp(int index) {
        if ( index == 0 ) {
            return;
        }

        Replace(index, index - 1);
    }

    /// <summary>
    /// ���ړ��{�^�����N���b�N���ꂽ
    /// </summary>
    /// <param name="index">���[�V�����̃C���f�b�N�X</param>
    private void OnDown(int index) {
        if ( index >= expansion.Length - 1 ) {
            return;
        }

        Replace(index, index + 1);
    }

    /// <summary>
    /// �}���{�^�����N���b�N���ꂽ
    /// </summary>
    /// <param name="index">���[�V�����̃C���f�b�N�X</param>
    private void OnInsertNew(int index) {
        (target as MotionSequence2D).InsertNew(index);

        var newExpansion = new List<bool>(expansion);
        newExpansion.Insert(index, false);
        expansion = newExpansion.ToArray();
    }

    /// <summary>
    /// �폜�{�^�����N���b�N���ꂽ
    /// </summary>
    /// <param name="index">���[�V�����̃C���f�b�N�X</param>
    private void OnRemove(int index) {
        (target as MotionSequence2D).Remove(index);

        var newExpansion = new List<bool>(expansion);
        newExpansion.RemoveAt(index);
        expansion = newExpansion.ToArray();
    }

    /// <summary>
    /// ���[�V���������ւ���
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    private void Replace(int index1, int index2) {
        (target as MotionSequence2D).Replace(index1, index2);

        var tmp = expansion[index1];
        expansion[index1] = expansion[index2];
        expansion[index2] = tmp;
    }
}