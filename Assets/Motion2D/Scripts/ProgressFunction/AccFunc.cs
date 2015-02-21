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
public class AccFunc : ProgFuncBase {
    /// <summary>
    /// ��������(0�`0.5)
    /// </summary>
    private float accSpan;

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
        if ( accSpan == 0 ) {
            return progress;
        } else if ( progress <= accSpan ) {
            progress = progress * progress / accSpan / 2;
        } else if ( progress <= 1 - accSpan ) {
            progress = progress - accSpan / 2;
        } else {
            progress = progress * (1 - progress / 2) + (accSpan * accSpan - 1) / 2;
            progress /= accSpan;
            progress += 1 - 1.5f * accSpan;
        }

        return progress * maxSpeed;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(accSpan)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        accSpan = BitConverter.ToSingle(bytes, offset);
        maxSpeed = 1 / (1 - accSpan);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        accSpan = UnityEditor.EditorGUILayout.Slider("Accelerate Span", accSpan, 0, 0.5f);
    }
#endif
}
