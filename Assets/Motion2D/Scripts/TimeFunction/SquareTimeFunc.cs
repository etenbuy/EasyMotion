///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SquareTimeFunc.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.24                                                                       //
//  Desc    :   �񎟎��Ԋ֐��B                                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �񎟎��Ԋ֐��B
/// </summary>
public class SquareTimeFunc : TimeFuncBase {
    /// <summary>
    /// �W��
    /// </summary>
    private float coefficient = 1;

    /// <summary>
    /// ���Ԃ��擾����
    /// </summary>
    /// <param name="time">���͒l</param>
    /// <returns>�o�͒l</returns>
    public override float GetTime(float time) {
        return time * time * coefficient;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(coefficient)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        coefficient = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        coefficient = UnityEditor.EditorGUILayout.FloatField("Coefficient", coefficient);
    }
#endif
}
