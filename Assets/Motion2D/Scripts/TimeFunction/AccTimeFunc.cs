///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   AccTimeFunc.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   ���Ԑi�s�̉������֐��B                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ���Ԑi�s�̉������֐��B
/// </summary>
public class AccTimeFunc : TimeFuncBase {
    /// <summary>
    /// �����̎��Ԑi�s���x
    /// </summary>
    private float initTimeScale = 1;

    /// <summary>
    /// ���Ԑi�s�̉����x
    /// </summary>
    private float acceleration = 0;

    /// <summary>
    /// ���Ԃ��擾����
    /// </summary>
    /// <param name="time">���͒l</param>
    /// <returns>�o�͒l</returns>
    public override float GetTime(float time) {
        return (acceleration * time / 2 + initTimeScale) * time;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(initTimeScale))
            .Concat(BitConverter.GetBytes(acceleration)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        initTimeScale = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        acceleration = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        initTimeScale = UnityEditor.EditorGUILayout.FloatField("Init Time Scale", initTimeScale);
        acceleration = UnityEditor.EditorGUILayout.FloatField("Acceleration", acceleration);
    }
#endif
}