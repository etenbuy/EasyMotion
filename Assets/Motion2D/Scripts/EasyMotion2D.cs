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
    private static Dictionary<MotionType, Type> runtimeType = new Dictionary<MotionType, Type>() {
        { MotionType.Stop, typeof(MotionBase2D) },
        { MotionType.MoveTo, typeof(MoveTo2D) },
        { MotionType.MoveArc, typeof(MoveArc2D) },
    };

    /// <summary>
    /// ���[�V�����̎��
    /// </summary>
    public MotionType type = MotionType.Stop;

    /// <summary>
    /// �V���A���C�Y�ς݃��[�V�����f�[�^
    /// </summary>
    public byte[] serializedMotion = null;

    /// <summary>
    /// ���s���̃��[�V�����I�u�W�F�N�g(�f�V���A���C�Y���ꂽ���[�V�����f�[�^)
    /// </summary>
    [HideInInspector]
    public MotionBase2D motion = null;

    /// <summary>
    /// �V���A���C�Y���ꂽ���[�V�����f�[�^����C���X�^���X�𐶐�����
    /// </summary>
    private void Awake() {
        // �f�V���A���C�Y���ꂽ���[�V�����f�[�^���X�V
        UpdateDeserializedMotion();
    }

    /// <summary>
    /// �������B
    /// </summary>
    private void Start() {
        // ���[�V�������s�J�n
        motion.StartMotion(this);
    }

    /// <summary>
    /// �f�V���A���C�Y���ꂽ���[�V�����f�[�^���X�V����
    /// </summary>
    public void UpdateDeserializedMotion() {
        // ���[�V�����I�u�W�F�N�g�쐬
        motion = GetDeserializedMotion(type, serializedMotion);
    }

    /// <summary>
    /// �f�V���A���C�Y���ꂽ���[�V�����I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="type">���[�V�����^</param>
    /// <returns>���s�����[�V�����I�u�W�F�N�g</returns>
    public static MotionBase2D GetDeserializedMotion(EasyMotion2D.MotionType type, byte[] bytes) {
        // ���[�V�����I�u�W�F�N�g�쐬
        var motion = CreateInstance(type);
        // ���[�V�����f�[�^�̃f�V���A���C�Y
        motion.Deserialize(bytes);
        return motion;
    }

    /// <summary>
    /// �V�K�̎��s�����[�V�����I�u�W�F�N�g�𐶐�����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static MotionBase2D CreateInstance(EasyMotion2D.MotionType type) {
        return Activator.CreateInstance(runtimeType[type]) as MotionBase2D;
    }

#if UNITY_EDITOR
    /// <summary>
    /// ����`�悩�ǂ���
    /// </summary>
    private bool isFirstDraw = true;

    /// <summary>
    /// ���[�V������Gizmo�`��
    /// </summary>
    private void OnDrawGizmos() {
        if ( isFirstDraw ) {
            if ( !Application.isPlaying ) {
                motion = GetDeserializedMotion(type, serializedMotion);
            }
            isFirstDraw = false;
        }

        motion.DrawGizmos(transform);
    }
#endif
}
