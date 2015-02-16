///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EasyMotion2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   2D��̃��[�V�����Ǘ��B                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 2D��̃��[�V�����Ǘ��B
/// </summary>
public class EasyMotion2D : MonoBehaviour {
    /// <summary>
    /// ���[�V�����̎�ޒ�`
    /// </summary>
    public enum MotionType {
        Stop,
        MoveTo,
        MoveArc,
        Liner,
    };

    /// <summary>
    /// ���s���^�̒�`
    /// </summary>
    private Dictionary<MotionType, Type> runtimeType = new Dictionary<MotionType, Type>() {
        { MotionType.Stop, typeof(MotionBase2D) },
        { MotionType.MoveTo, typeof(MoveTo2D) },
    };

    /// <summary>
    /// ���[�V�����̎��
    /// </summary>
    [SerializeField]
    private MotionType type;

    /// <summary>
    /// �V���A���C�Y�ς݃��[�V�����f�[�^
    /// </summary>
    [SerializeField]
    private byte[] serializedMotion;

    /// <summary>
    /// ���s���̃��[�V�����I�u�W�F�N�g(�f�V���A���C�Y���ꂽ���[�V�����f�[�^)
    /// </summary>
    private MotionBase2D motion;

    /// <summary>
    /// �V���A���C�Y���ꂽ���[�V�����f�[�^����C���X�^���X�𐶐�����
    /// </summary>
    private void Awake() {
        // ���[�V�����I�u�W�F�N�g�쐬
        motion = Activator.CreateInstance(runtimeType[type]) as MotionBase2D;
        // ���[�V�����f�[�^�̃f�V���A���C�Y
        motion.Deserialize(serializedMotion);
    }

    /// <summary>
    /// �������B
    /// </summary>
    private void Start() {
        // ���[�V�������s�J�n
        motion.StartMotion(this);
    }
}
