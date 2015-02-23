///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   TargetBase2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.23                                                                       //
//  Desc    :   2D��ԏ�̖ڕW�������Ǘ�������N���X�B                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 2D��ԏ�̖ڕW�������Ǘ�������N���X�B
/// </summary>
public abstract class TargetBase2D {
    /// <summary>
    /// ���
    /// </summary>
    public enum TargetType {
        Name,
    };

    /// <summary>
    /// ���s���^�̒�`
    /// </summary>
    private static Dictionary<TargetType, Type> runtimeType = new Dictionary<TargetType, Type>() {
        { TargetType.Name, typeof(TargetName2D) },
    };

    /// <summary>
    /// �ڕW����Transform
    /// </summary>
    public abstract Transform transform { get; }

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
    public static TargetBase2D CreateInstance(TargetType type) {
        return Activator.CreateInstance(runtimeType[type]) as TargetBase2D;
    }

    /// <summary>
    /// �f�V���A���C�Y���ꂽ�I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="type">���</param>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <param name="nextOffset">���̃f�[�^�̊J�n�ʒu</param>
    /// <returns>���s���I�u�W�F�N�g</returns>
    public static TargetBase2D GetDeserialized(TargetType type, byte[] bytes, int offset, out int nextOffset) {
        // �I�u�W�F�N�g�쐬
        var target = CreateInstance(type);
        // �f�V���A���C�Y
        nextOffset = target.Deserialize(bytes, offset);
        return target;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public virtual void DrawGUI() {
    }
#endif
}
