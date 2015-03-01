///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   TimeFuncBase.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.24                                                                       //
//  Desc    :   ���Ԋ֐��̊��N���X�B                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ���Ԋ֐��̊��N���X�B
/// </summary>
public class TimeFuncBase {
    /// <summary>
    /// �֐��̎��
    /// </summary>
    public enum FuncType {
        None,
        Liner,
        Square,
        FromTo,
        Acc,
    };

    /// <summary>
    /// ���s���^�̒�`
    /// </summary>
    private static Dictionary<FuncType, Type> runtimeType = new Dictionary<FuncType, Type>() {
        { FuncType.None, typeof(TimeFuncBase) },
        { FuncType.Liner, typeof(LinerTimeFunc) },
        { FuncType.Square, typeof(SquareTimeFunc) },
        { FuncType.FromTo, typeof(FromToTimeFunc) },
        { FuncType.Acc, typeof(AccTimeFunc) },
    };

    /// <summary>
    /// ���Ԃ��擾����
    /// </summary>
    /// <param name="time">���͒l</param>
    /// <returns>�o�͒l</returns>
    public virtual float GetTime(float time) {
        return time;
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
    public static TimeFuncBase CreateInstance(TimeFuncBase.FuncType type) {
        return Activator.CreateInstance(runtimeType[type]) as TimeFuncBase;
    }

    /// <summary>
    /// �f�V���A���C�Y���ꂽ�I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="type">�֐��̎��</param>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <param name="nextOffset">���̃f�[�^�̊J�n�ʒu</param>
    /// <returns>���s���I�u�W�F�N�g</returns>
    public static TimeFuncBase GetDeserialized(FuncType type, byte[] bytes, int offset, out int nextOffset) {
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
