///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   TweenVelocity2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   ���x�ω����[�V�����B                                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ���x�ω����[�V�����B
/// </summary>
public class TweenVelocity2D : LimitedMotion2D {
    /// <summary>
    /// �ω��O�̑��x
    /// </summary>
    private Vector2 fromVelocity;

    /// <summary>
    /// �ω���̑��x
    /// </summary>
    private Vector2 toVelocity;

    /// <summary>
    /// ���ݎ���
    /// </summary>
    private float curTime;

    /// <summary>
    /// �������[�V�����̏���������
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedStart() {
    }

    /// <summary>
    /// �������[�V�����̍X�V����
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedUpdate(float progress) {
        curTime = duration * progress;
        position = initPosition + GetPosition(curTime);
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(fromVelocity.x))
            .Concat(BitConverter.GetBytes(fromVelocity.y))
            .Concat(BitConverter.GetBytes(toVelocity.x))
            .Concat(BitConverter.GetBytes(toVelocity.y)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        fromVelocity.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        fromVelocity.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        toVelocity.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        toVelocity.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    public override float currentDirection {
        get {
            Vector2 vel;
            if ( duration == 0 ) {
                vel = toVelocity;
            } else {
                vel = fromVelocity + (toVelocity - fromVelocity) * curTime / duration;
            }
            return Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        }
    }

    /// <summary>
    /// �����x���w�肷��
    /// </summary>
    /// <param name="vel">�����x</param>
    public override void SetInitVelocity(Vector2 vel) {
        fromVelocity = vel;
    }

    /// <summary>
    /// �ʒu���������擾����
    /// </summary>
    /// <param name="time">����</param>
    /// <returns>�ʒu</returns>
    private Vector2 GetPosition(float time) {
        if ( duration == 0 ) {
            return Vector2.zero;
        }

        Vector2 result;
        result = (toVelocity - fromVelocity) * time / (2 * duration) + fromVelocity;
        result *= time;
        return result;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        fromVelocity = UnityEditor.EditorGUILayout.Vector2Field("From Velocity", fromVelocity);
        toVelocity = UnityEditor.EditorGUILayout.Vector2Field("To Velocity", toVelocity);
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        const int POINT_NUM = 20;

        Vector2[] points = new Vector2[POINT_NUM + 1];
        for ( int i = 0 ; i < POINT_NUM ; ++i ) {
            points[i] = from + GetPosition((float)i / POINT_NUM * duration);
        }
        var to = points[POINT_NUM] = from + GetPosition(duration);
        DrawLine(points);
        DrawArrowCap(to, Mathf.Atan2(toVelocity.y, toVelocity.x) * Mathf.Rad2Deg);

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
            curSpeed = fromVelocity.magnitude;
        }
        return curSpeed;
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public override void SetSpeed(Vector2 from, float speed) {
        fromVelocity = fromVelocity.normalized * speed;
    }

    /// <summary>
    /// �I�[�ʒu�̌������擾����
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="fromAngle">�J�n�p�x</param>
    /// <returns>�I�[�ʒu�̌���</returns>
    public override float GetEndDirection(Vector2 from, float fromAngle) {
        if ( toVelocity == Vector2.zero ) {
            return initDirection;
        }

        return Mathf.Atan2(toVelocity.y, toVelocity.x) * Mathf.Rad2Deg;
    }
#endif
}
