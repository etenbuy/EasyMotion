///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   AdvancedMotion2D.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.15                                                                       //
//  Desc    :   �g�����[�V�����B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// �g�����[�V�����B
/// </summary>
public class AdvancedMotion2D : MotionBase2D {
    /// <summary>
    /// ���[�V�����^�C�v
    /// </summary>
    private EasyMotion2D.MotionType motionType;

    /// <summary>
    /// ���[�V����
    /// </summary>
    private MotionBase2D motion;

    /// <summary>
    /// ��]���[�V�����^�C�v
    /// </summary>
    private RotationBase2D.RotationType rotationType;

    /// <summary>
    /// ��]���[�V����
    /// </summary>
    private RotationBase2D rotation;

    /// <summary>
    /// ���[�V�����͌p�����邩�ǂ���
    /// </summary>
    private bool motionUpdate = true;

    /// <summary>
    /// ��]���[�V�����͌p�����邩�ǂ���
    /// </summary>
    private bool rotationUpdate = true;

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    protected override void OnInit() {
        base.OnInit();

        motion.InitMotion(transform, initDirection);
        rotation.motion = this;
    }

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnStart() {
        motion.StartMotion();
        rotation.StartRotation(transform);
        return true;
    }

    /// <summary>
    /// ���[�V�����̍X�V����
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnUpdate() {
        if ( motionUpdate ) {
            motionUpdate = motion.UpdateMotion();
            position = motion.position;
        }

        if ( rotationUpdate ) {
            rotationUpdate = rotation.UpdateRotation();
        }

        return motionUpdate;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        if ( motion == null ) {
            motion = EasyMotion2D.CreateInstance(EasyMotion2D.MotionType.Stop);
        }
        if ( rotation == null ) {
            rotation = RotationBase2D.CreateInstance(RotationBase2D.RotationType.None);
        }

        var motionType = EasyMotion2D.GetSerializedType(motion.GetType());
        var rotationType = RotationBase2D.GetSerializedType(rotation.GetType());

        result = result
            .Concat(BitConverter.GetBytes((int)motionType))
            .Concat(motion.Serialize())
            .Concat(BitConverter.GetBytes((int)rotationType))
            .Concat(rotation.Serialize()).ToArray();

        return result;
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        motionType = (EasyMotion2D.MotionType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        motion = EasyMotion2D.GetDeserializedMotion(motionType, bytes, offset, out offset);

        rotationType = (RotationBase2D.RotationType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        rotation = RotationBase2D.GetDeserializedRotation(rotationType, bytes, offset, out offset);

        return offset;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    public override float currentDirection {
        get {
            return motion.currentDirection;
        }
    }

    /// <summary>
    /// �����x���w�肷��
    /// </summary>
    /// <param name="vel">�����x</param>
    public override void SetInitVelocity(Vector2 vel) {
        motion.SetInitVelocity(vel);
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();

        var curMotionType = EasyMotion2D.GetSerializedType(motion.GetType());
        var newMotionType = (EasyMotion2D.MotionType)UnityEditor.EditorGUILayout.EnumPopup("Motion Type", curMotionType);
        if ( newMotionType != curMotionType ) {
            motion = EasyMotion2D.CreateInstance(newMotionType);
        }

        motion.DrawGUI();

        var curRotationType = RotationBase2D.GetSerializedType(rotation.GetType());
        var newRotationType = (RotationBase2D.RotationType)UnityEditor.EditorGUILayout.EnumPopup("Rotation Type", curRotationType);
        if ( newRotationType != curRotationType ) {
            rotation = RotationBase2D.CreateInstance(newRotationType);
        }

        rotation.DrawGUI();
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        return motion.DrawGizmos(transform, from);
    }

    /// <summary>
    /// �����擾
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <returns>�ݒ肳�ꂽ����</returns>
    public override float GetSpeed(Vector2 from) {
        return motion.GetSpeed(from);
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public override void SetSpeed(Vector2 from, float speed) {
        motion.SetSpeed(from, speed);
    }
#endif
}
