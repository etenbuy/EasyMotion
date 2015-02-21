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

/// <summary>
/// 2D��]����̊��N���X�B
/// </summary>
[Serializable]
public class RotationBase2D {
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
    public enum RotationType {
        None,
        Rotate,
        Forward,
    };

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
    protected float startTime { get; private set; }

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
}
