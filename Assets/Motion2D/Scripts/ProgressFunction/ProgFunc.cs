///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   ProgFunc.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   �i���x���֐��̊��N���X�B                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// �i���x���֐��̊��N���X�B
/// </summary>
public class ProgFunc {
    /// <summary>
    /// �i���x�����擾����
    /// </summary>
    /// <param name="progress">���͒l</param>
    /// <returns>�o�͒l</returns>
    public virtual float GetProgress(float progress) {
        return progress;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public virtual byte[] Serialize() {
        return new byte[0];
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public virtual int Deserialize(byte[] bytes, int offset) {
        return offset;
    }
}
