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
    /// �ω��O�̎��Ԃ̐i�s���x
    /// </summary>
    private float fromTimeScale = 1;

    /// <summary>
    /// �ω���̎��Ԃ̐i�s���x
    /// </summary>
    private float toTimeScale = 1;

    /// <summary>
    /// �ω�����
    /// </summary>
    private float duration = 1;

    /// <summary>
    /// ���Ԃ��擾����
    /// </summary>
    /// <param name="time">���͒l</param>
    /// <returns>�o�͒l</returns>
    public override float GetTime(float time) {
        float result;
        if ( time < duration ) {
            result = fromTimeScale + (toTimeScale - fromTimeScale) / (duration * 2) * time;
            result *= time;
        } else {
            result = toTimeScale * time + (fromTimeScale - toTimeScale) * duration / 2;
        }
        return result;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(fromTimeScale))
            .Concat(BitConverter.GetBytes(toTimeScale))
            .Concat(BitConverter.GetBytes(duration)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        fromTimeScale = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        toTimeScale = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        duration = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        fromTimeScale = UnityEditor.EditorGUILayout.FloatField("From Time Scale", fromTimeScale);
        toTimeScale = UnityEditor.EditorGUILayout.FloatField("To Time Scale", toTimeScale);
        duration = UnityEditor.EditorGUILayout.FloatField("Duration", duration);
    }
#endif
}
