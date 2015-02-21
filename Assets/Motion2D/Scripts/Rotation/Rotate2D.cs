///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   Rotate2D.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   ������]����B                                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ������]����B
/// </summary>
public class Rotate2D : RotationBase2D {
    /// <summary>
    /// ��]�p���x
    /// </summary>
    public float speed;

    /// <summary>
    /// �����p�x
    /// </summary>
    private float initAngle;

    /// <summary>
    /// ��]����̏���������
    /// </summary>
    /// <returns>true:��]����p�� / false:�ȍ~�̉�]������p�����Ȃ�</returns>
    protected override bool OnStart() {
        initAngle = angle;
        return true;
    }

    /// <summary>
    /// ��]����̍X�V����
    /// </summary>
    /// <returns>true:��]����p�� / false:�ȍ~�̉�]������p�����Ȃ�</returns>
    protected override bool OnUpdate() {
        angle = initAngle + speed * (Time.time - startTime);
        return true;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(speed)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݉�]����f�[�^</param>
    /// <param name="offset">��]����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }
}
