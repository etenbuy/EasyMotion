///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   RotationBase2D.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   2D��]����̊��N���X�B                                                         //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 2D��]����̊��N���X�B
/// </summary>
[Serializable]
public class RotationBase2D {
    /// <summary>
    /// ��]����̎��
    /// </summary>
    public enum RotationType {
        None,
        Rotate,
        Forward,
    };

    /// <summary>
    /// ���s���^�̒�`
    /// </summary>
    private static Dictionary<RotationType, Type> runtimeType = new Dictionary<RotationType, Type>() {
        { RotationType.None, typeof(RotationBase2D) },
        { RotationType.Rotate, typeof(Rotate2D) },
        { RotationType.Forward, typeof(RotateForward2D) },
    };

    /// <summary>
    /// ��]���J�n����܂ł̎���
    /// </summary>
    public float delay;

    /// <summary>
    /// ���݂̊p�x
    /// </summary>
    public float angle;

    /// <summary>
    /// ���[�V����
    /// </summary>
    public MotionBase2D motion;

    /// <summary>
    /// ��]����̎��
    /// </summary>
    public RotationType type = RotationType.None;

    /// <summary>
    /// ���s���̉�]�����Ԓ�`
    /// </summary>
    private enum State {
        Disable,
        Waiting,
        Running,
    }

    /// <summary>
    /// ���s���̉�]������
    /// </summary>
    private State state = State.Disable;

    /// <summary>
    /// GameObject��Transform
    /// </summary>
    protected Transform transform;

    /// <summary>
    /// �J�n����
    /// </summary>
    private float startTime;

    /// <summary>
    /// OnStart()�͌Ăяo���ꂽ���ǂ���
    /// </summary>
    private bool onStartCalled = false;

    /// <summary>
    /// ��]������J�n����
    /// </summary>
    /// <param name="objTrans">GameObject��Transform</param>
    public void StartRotation(Transform objTrans) {
        state = delay > 0 ? State.Waiting : State.Running;
        transform = objTrans.transform;
        startTime = Time.time;
        angle = transform.localEulerAngles.z;
    }

    /// <summary>
    /// ��]�����Ԃ��X�V����
    /// </summary>
    public bool UpdateRotation() {
        switch ( state ) {
        case State.Waiting:
            // �J�n�܂ł̈�莞�ԑҋ@
            if ( Time.time - startTime >= delay ) {
                // ��]������s�ɑJ��
                state = State.Running;
            }
            return true;

        case State.Running:
            // ��]������s
            if ( !onStartCalled ) {
                onStartCalled = true;
                if ( !OnStart() ) {
                    return false;
                }
            }

            // �X�V����
            var nextUpdate = OnUpdate();

            // �����X�V
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y,
                angle
            );

            if ( !nextUpdate ) {
                // ��]����I���Ȃ疳����ԂɑJ��
                state = State.Disable;
            }

            return nextUpdate;
        }

        return false;
    }

    /// <summary>
    /// ��]����̏���������(�h���N���X�Ŏ�������)
    /// </summary>
    /// <returns>true:��]����p�� / false:�ȍ~�̉�]������p�����Ȃ�</returns>
    protected virtual bool OnStart() {
        return false;
    }

    /// <summary>
    /// ��]����̍X�V����(�h���N���X�Ŏ�������)
    /// </summary>
    /// <returns>true:��]����p�� / false:�ȍ~�̉�]������p�����Ȃ�</returns>
    protected virtual bool OnUpdate() {
        return false;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public virtual byte[] Serialize() {
        var result = BitConverter.GetBytes(delay);
        return result;
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݉�]����f�[�^</param>
    /// <param name="offset">��]����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public virtual int Deserialize(byte[] bytes, int offset) {
        delay = BitConverter.ToSingle(bytes, offset);
        return offset + sizeof(float);
    }

    /// <summary>
    /// �V�K�̎��s����]����I�u�W�F�N�g�𐶐�����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static RotationBase2D CreateInstance(RotationBase2D.RotationType type) {
        return Activator.CreateInstance(runtimeType[type]) as RotationBase2D;
    }

    /// <summary>
    /// �f�V���A���C�Y���ꂽ��]����I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="type">��]����^</param>
    /// <param name="bytes">�V���A���C�Y�ς݉�]����f�[�^</param>
    /// <returns>���s����]����I�u�W�F�N�g</returns>
    public static RotationBase2D GetDeserializedRotation(RotationType type, byte[] bytes) {
        // ��]����I�u�W�F�N�g�쐬
        var rotation = CreateInstance(type);
        if ( bytes != null ) {
            // ��]����f�[�^�̃f�V���A���C�Y
            rotation.Deserialize(bytes, 0);
        }
        return rotation;
    }

    /// <summary>
    /// �p�x���w��͈͂ɕ␳����
    /// </summary>
    /// <param name="angle">�␳�Ώۂ̊p�x</param>
    /// <param name="minAngle">�p�x�͈͂̍ŏ��l</param>
    /// <returns>�␳���ꂽ�p�x</returns>
    public static float AdjustAngleRange(float angle, float minAngle) {
        var maxAngle = minAngle + 360f;

        if ( angle >= minAngle && angle < maxAngle ) {
            return angle;
        }

        angle -= minAngle;
        angle = angle % 360f;
        if ( angle < 0 ) {
            angle += 360f;
        }
        angle += minAngle;

        return angle;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public virtual void DrawGUI() {
        delay = UnityEditor.EditorGUILayout.FloatField("Delay", delay);
    }
#endif
}
