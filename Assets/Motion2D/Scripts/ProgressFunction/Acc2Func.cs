///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   Acc2Func.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.22                                                                       //
//  Desc    :   �����֐�(Acc2Func�̊g����)�B                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �����֐�(Acc2Func�̊g����)�B
/// </summary>
public class Acc2Func : ProgFuncBase {
    /// <summary>
    /// ��������1(0�`0.5)
    /// </summary>
    private float accSpan1;

    /// <summary>
    /// ��������2(0�`0.5)
    /// </summary>
    private float accSpan2;

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
        if ( accSpan1 == 0 && accSpan2 == 0 ) {
            return progress;
        } else if ( progress <= accSpan1 ) {
            if ( accSpan1 == 0 ) {
                return progress;
            }
            progress = progress * progress / accSpan1 / 2;
        } else if ( progress <= 1 - accSpan2 ) {
            progress = progress - accSpan1 / 2;
        } else {
            if ( accSpan2 == 0 ) {
                return progress;
            }
            progress = progress * (1 - progress / 2) + (accSpan2 * accSpan2 - 1) / 2;
            progress /= accSpan2;
            progress += 1 - 0.5f * accSpan1 - accSpan2;
        }

        return progress * maxSpeed;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(accSpan1))
            .Concat(BitConverter.GetBytes(accSpan2)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        accSpan1 = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        accSpan2 = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        maxSpeed = 2 / (2 - accSpan1 - accSpan2);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        accSpan1 = UnityEditor.EditorGUILayout.Slider("Accelerate Span1", accSpan1, 0, 0.5f);
        accSpan2 = UnityEditor.EditorGUILayout.Slider("Accelerate Span2", accSpan2, 0, 0.5f);
    }
#endif
}
