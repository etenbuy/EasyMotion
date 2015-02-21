///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   AccFunc.cs                                                                       //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   �����֐��B                                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �����֐��B
/// </summary>
public class AccFunc : ProgFunc {
    /// <summary>
    /// ��������(0�`0.5)
    /// </summary>
    private float accTime;

    /// <summary>
    /// �ō����x(�����Ŏ����v�Z�����)
    /// </summary>
    private float maxSpeed;

    /// <summary>
    /// �i���x�����擾����
    /// </summary>
    /// <param name="progress">���͒l</param>
    /// <returns>�o�͒l</returns>
    public override float GetProgress(float progress) {
        if ( progress <= accTime ) {
            progress = maxSpeed / accTime / 2 * progress * progress;
        } else if ( progress < 1 - accTime ) {
            progress = maxSpeed * progress + maxSpeed * accTime / 2;
        } else {
            progress = -progress / accTime * (0.5f * progress + 1) + 1 - 1.5f * accTime;
            progress *= maxSpeed;
        }

        return progress;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(accTime)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        accTime = BitConverter.ToSingle(bytes, offset);
        maxSpeed = 1 / (1 - accTime);
        offset += sizeof(float);

        return offset;
    }
}
