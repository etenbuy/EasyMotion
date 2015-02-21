///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   ProgFuncBase.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   �i���x���֐��̊��N���X�B                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �i���x���֐��̊��N���X�B
/// </summary>
public class ProgFuncBase {
    /// <summary>
    /// �֐��̎��
    /// </summary>
    public enum FuncType {
        Liner,
        Acc,
    };

    /// <summary>
    /// ���s���^�̒�`
    /// </summary>
    private static Dictionary<FuncType, Type> runtimeType = new Dictionary<FuncType, Type>() {
        { FuncType.Liner, typeof(ProgFuncBase) },
        { FuncType.Acc, typeof(AccFunc) },
    };

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

    /// <summary>
    /// �V�K�̎��s���I�u�W�F�N�g�𐶐�����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ProgFuncBase CreateInstance(ProgFuncBase.FuncType type) {
        return Activator.CreateInstance(runtimeType[type]) as ProgFuncBase;
    }

    /// <summary>
    /// �f�V���A���C�Y���ꂽ�I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="type">�֐��̎��</param>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <param name="nextOffset">���̃f�[�^�̊J�n�ʒu</param>
    /// <returns>���s���I�u�W�F�N�g</returns>
    public static ProgFuncBase GetDeserialized(FuncType type, byte[] bytes, int offset, out int nextOffset) {
        // �I�u�W�F�N�g�쐬
        var func = CreateInstance(type);
        // �f�V���A���C�Y
        nextOffset = func.Deserialize(bytes, offset);
        return func;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public virtual void DrawGUI() {
    }
#endif
}
