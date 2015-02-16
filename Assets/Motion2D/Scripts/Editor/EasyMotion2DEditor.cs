///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EasyMotion2DEditor.cs                                                            //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   EasyMotion2D�N���X�̃G�f�B�^�g���B                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// EasyMotion2D�N���X�̃G�f�B�^�g���B
/// </summary>
[CustomEditor(typeof(EasyMotion2D))]
public class EasyMotion2DEditor : Editor {
    /// <summary>
    /// �������B
    /// </summary>
    public void Awake() {
        var type = (EasyMotion2D.MotionType)serializedObject.FindProperty("type").enumValueIndex;

        var script = target as EasyMotion2D;
        script.motion = EasyMotion2D.GetDeserializedMotion(type, script.serializedMotion);
    }

    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(Application.isPlaying);

        var script = target as EasyMotion2D;

        // ���[�V�����^�I��`��
        var currentType = script.type;
        var newType = script.type = (EasyMotion2D.MotionType)EditorGUILayout.EnumPopup("Motion Type", currentType);

        var motion = script.motion;
        if ( motion == null ) {
            motion = EasyMotion2D.GetDeserializedMotion(currentType, script.serializedMotion);
        }

        if ( newType != currentType ) {
            // ���[�V�����^���ύX���ꂽ
            motion = EasyMotion2D.CreateInstance(newType);
            script.serializedMotion = motion.Serialize();

            script.motion = motion;
        }

        // ���[�V�����ݒ�p��GUI�`��
        motion.DrawGUI();

        EditorGUI.EndDisabledGroup();

        if ( GUI.changed ) {
            // �l���ҏW���ꂽ��V���A���C�Y�f�[�^���X�V
            script.serializedMotion = motion.Serialize();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
