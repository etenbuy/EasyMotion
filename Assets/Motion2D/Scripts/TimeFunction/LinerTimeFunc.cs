///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerTimeFunc.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.24                                                                       //
//  Desc    :   ���`���Ԋ֐��B                                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ���`���Ԋ֐��B
/// </summary>
public class LinerTimeFunc : TimeFuncBase {
    /// <summary>
    /// �{��
    /// </summary>
    private float magnification;

    /// <summary>
    /// �i���x�����擾����
    /// </summary>
    /// <param name="progress">���͒l</param>
    /// <returns>�o�͒l</returns>
    public override float GetProgress(float progress) {
        return progress;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(magnification)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        magnification = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        magnification = UnityEditor.EditorGUILayout.Slider("Magnification", magnification, 0, 0.5f);
    }
#endif
}
