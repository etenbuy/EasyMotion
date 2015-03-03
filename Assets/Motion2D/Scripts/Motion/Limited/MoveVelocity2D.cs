///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveVelocity2D.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   ���x�w��̎������[�V�����B                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ���x�w��̎������[�V�����B
/// </summary>
public class MoveVelocity2D : LimitedMotion2D {
    /// <summary>
    /// �ړ����x
    /// </summary>
    private Vector2 velocity;

    /// <summary>
    /// ���݂̌���
    /// </summary>
    private float curAngle;

    /// <summary>
    /// �������[�V�����̏���������
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedStart() {
        curAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// �������[�V�����̍X�V����
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedUpdate(float progress) {
        position = initPosition + progress * duration * velocity;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(velocity.x))
            .Concat(BitConverter.GetBytes(velocity.y)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        velocity.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        velocity.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    public override float currentDirection {
        get {
            return curAngle;
        }
    }

    /// <summary>
    /// �����x���w�肷��
    /// </summary>
    /// <param name="vel">�����x</param>
    public override void SetInitVelocity(Vector2 vel) {
        velocity = vel;
        curAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        velocity = UnityEditor.EditorGUILayout.Vector2Field("Velocity", velocity);
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        var to = from + velocity * duration;
        DrawLine(from, to);
        DrawArrowCap(to, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);

        return to;
    }

    /// <summary>
    /// �����擾
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <returns>�ݒ肳�ꂽ����</returns>
    public override float GetSpeed(Vector2 from) {
        var curSpeed = 0f;
        if ( duration != 0 ) {
            curSpeed = velocity.magnitude;
        }
        return curSpeed;
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public override void SetSpeed(Vector2 from, float speed) {
        velocity = velocity.normalized * speed;
    }

    /// <summary>
    /// �I�[�ʒu�̌������擾����
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="fromAngle">�J�n�p�x</param>
    /// <returns>�I�[�ʒu�̌���</returns>
    public override float GetEndDirection(Vector2 from, float fromAngle) {
        if ( velocity == Vector2.zero ) {
            return initDirection;
        }

        return Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
    }
#endif
}
