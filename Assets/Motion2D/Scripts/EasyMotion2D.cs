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
using System.Linq;

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
        MoveAdd,
        MoveArc,
        MoveLiner,
        Sequence,
        Chase,
        Direction,
        LimitedLiner,
        TweenVelocity,
    };

    /// <summary>
    /// ���s���^�̒�`
    /// </summary>
    private static Dictionary<MotionType, Type> runtimeType = new Dictionary<MotionType, Type>() {
        { MotionType.Stop, typeof(MotionBase2D) },
        { MotionType.MoveTo, typeof(MoveTo2D) },
        { MotionType.MoveAdd, typeof(MoveAdd2D) },
        { MotionType.MoveArc, typeof(MoveArc2D) },
        { MotionType.MoveLiner, typeof(MoveLiner2D) },
        { MotionType.Sequence, typeof(MotionSequence2D) },
        { MotionType.Chase, typeof(ChaseMotion2D) },
        { MotionType.Direction, typeof(MoveDirection2D) },
        { MotionType.LimitedLiner, typeof(MoveVelocity2D) },
        { MotionType.TweenVelocity, typeof(TweenVelocity2D) },
    };

    /// <summary>
    /// ���[�V�����̎��
    /// </summary>
    public MotionType type = MotionType.Stop;

    /// <summary>
    /// �V���A���C�Y�ς݃��[�V�����f�[�^
    /// </summary>
    [HideInInspector]
    public byte[] serializedMotion = null;

    /// <summary>
    /// ���s���̃��[�V�����I�u�W�F�N�g(�f�V���A���C�Y���ꂽ���[�V�����f�[�^)
    /// </summary>
    [HideInInspector]
    public MotionBase2D motion = null;

    /// <summary>
    /// ���[�V�����͏I���������ǂ���
    /// </summary>
    private bool motionEnd = false;

    /// <summary>
    /// ��]����̎��
    /// </summary>
    public RotationBase2D.RotationType rotationType = RotationBase2D.RotationType.None;

    /// <summary>
    /// �V���A���C�Y�ς݉�]����f�[�^
    /// </summary>
    [HideInInspector]
    public byte[] serializedRotation = null;

    /// <summary>
    /// ���s���̉�]����I�u�W�F�N�g(�f�V���A���C�Y���ꂽ��]����f�[�^)
    /// </summary>
    [HideInInspector]
    public RotationBase2D rotation = null;

    /// <summary>
    /// ��]����͏I���������ǂ���
    /// </summary>
    private bool rotationEnd = false;

    /// <summary>
    /// �V���A���C�Y���ꂽ���[�V�����f�[�^����C���X�^���X�𐶐�����
    /// </summary>
    private void Awake() {
        // �f�[�^�̃f�V���A���C�Y
        motion = GetDeserializedMotion(type, serializedMotion);
        rotation = RotationBase2D.GetDeserializedRotation(rotationType, serializedRotation);
        rotation.motion = motion;
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Start() {
        // ���[�V�������s�J�n
        var trans = transform;
        motion.InitMotion(trans);
        motion.StartMotion();
        rotation.StartRotation(trans);
    }

    /// <summary>
    /// �t���[�����̍X�V����
    /// </summary>
    private void Update() {
        if ( !motionEnd ) {
            // ���[�V�����̏�ԍX�V
            if ( !motion.UpdateMotion() ) {
                // �I���Ȃ牽�����Ȃ�
                motionEnd = true;
            }
        }

        if ( !rotationEnd ) {
            // ��]����̏�ԍX�V
            if ( !rotation.UpdateRotation() ) {
                // �I���Ȃ牽�����Ȃ�
                rotationEnd = true;
            }
        }
    }

    /// <summary>
    /// �f�V���A���C�Y���ꂽ���[�V�����I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="type">���[�V�����^</param>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <returns>���s�����[�V�����I�u�W�F�N�g</returns>
    public static MotionBase2D GetDeserializedMotion(EasyMotion2D.MotionType type, byte[] bytes) {
        // ���[�V�����I�u�W�F�N�g�쐬
        var motion = CreateInstance(type);
        if ( bytes != null ) {
            // ���[�V�����f�[�^�̃f�V���A���C�Y
            motion.Deserialize(bytes, 0);
        }
        return motion;
    }

    /// <summary>
    /// �f�V���A���C�Y���ꂽ���[�V�����I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="type">���[�V�����^</param>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <param name="nextOffset">���̃��[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>���s�����[�V�����I�u�W�F�N�g</returns>
    public static MotionBase2D GetDeserializedMotion(EasyMotion2D.MotionType type, byte[] bytes, int offset, out int nextOffset) {
        // ���[�V�����I�u�W�F�N�g�쐬
        var motion = CreateInstance(type);
        // ���[�V�����f�[�^�̃f�V���A���C�Y
        nextOffset = motion.Deserialize(bytes, offset);
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

    /// <summary>
    /// �V���A���C�Y�ς݃��[�V�����^���擾����
    /// </summary>
    /// <param name="type">���s�����[�V�����^</param>
    /// <returns>�V���A���C�Y�ς݃��[�V�����^</returns>
    public static MotionType GetSerializedType(Type type) {
        return runtimeType.First(x => x.Value == type).Key;
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
        if ( !enabled ) {
            // �X�N���v�g�������͕`�悵�Ȃ�
            return;
        }

        if ( isFirstDraw ) {
            if ( !Application.isPlaying ) {
                // �G�f�B�^��ł̏��������̓��[�V�����f�[�^���f�V���A���C�Y
                motion = GetDeserializedMotion(type, serializedMotion);
            }
            isFirstDraw = false;
        }

        // Gizmo�`��
        motion.DrawGizmos(transform);
    }

    /// <summary>
    /// ���������E�B���h�E���J��
    /// </summary>
    [ContextMenu("Adjust Speed")]
    private void OpenAdjustSpeedWindow() {
        AdjustSpeedWindow.Open(motion.GetSpeed(motion.position), (speed) => {
            motion.SetSpeed(motion.position, speed);
            serializedMotion = motion.Serialize();
        });
    }
#endif
}
