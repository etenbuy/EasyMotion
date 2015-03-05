///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveArc2D.cs                                                                     //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   ����O����`�����[�V�����B                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ����O����`�����[�V�����B
/// </summary>
public class MoveArc2D : LimitedMotion2D {
    /// <summary>
    /// �����̌���
    /// </summary>
    private Direction2D fromDirection;

    /// <summary>
    /// ����p�x
    /// </summary>
    private float rotateAngle = 0;

    /// <summary>
    /// ���񔼌a
    /// </summary>
    private float radius = 0;

    /// <summary>
    /// ���݂̌���
    /// </summary>
    private float curAngle;

    /// <summary>
    /// ���p�x(�ʓx�@�\�L)
    /// </summary>
    private float fromAngleRad;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public MoveArc2D() {
        fromDirection = new Direction2D(this);
    }

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    protected override void OnInit() {
        base.OnInit();

        // ���s���̏��p�x�̌���
        fromAngleRad = fromDirection.direction;
        if ( rotateAngle < 0 ) {
            // �E����̏ꍇ
            fromAngleRad += 180;
        }

        curAngle = fromAngleRad;

        // �ʓx�@�\�L�ɕϊ�
        fromAngleRad *= Mathf.Deg2Rad;
    }

    /// <summary>
    /// �������[�V�����̍X�V����
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedUpdate(float progress) {
        var rotateAngleRad = rotateAngle;

        // �ʓx�@�\�L�ɕϊ�
        rotateAngleRad *= Mathf.Deg2Rad;

        var fromSin = Mathf.Sin(fromAngleRad);
        var fromCos = Mathf.Cos(fromAngleRad);

        // ���݈ʒu�v�Z
        var curAngleRad = fromAngleRad + progress * rotateAngleRad;

        position = initPosition + new Vector2(
            -fromSin + Mathf.Sin(curAngleRad),
            fromCos - Mathf.Cos(curAngleRad)
        ) * radius;

        curAngle = curAngleRad * Mathf.Rad2Deg;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(fromDirection.Serialize())
            .Concat(BitConverter.GetBytes(rotateAngle))
            .Concat(BitConverter.GetBytes(radius)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        offset = fromDirection.Deserialize(bytes, offset);
        rotateAngle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        radius = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    public override float currentDirection {
        get {
            return rotateAngle < 0 ? curAngle + 180 : curAngle;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        fromDirection.DrawGUI();
        rotateAngle = UnityEditor.EditorGUILayout.FloatField("Rotate Angle", rotateAngle);
        radius = UnityEditor.EditorGUILayout.FloatField("Radius", radius);
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        // �~�̒��_��
        const int POINT_NUM = 45;

        // �p�x���̏�����
        var isRight = rotateAngle < 0;
        var fromAngleRad = this.fromAngleRad;
        if ( !Application.isPlaying ) {
            fromAngleRad = fromDirection.direction * Mathf.Deg2Rad;

            if ( isRight ) {
                // �E����̏ꍇ
                fromAngleRad -= Mathf.PI;
            }
        }

        var fromSin = Mathf.Sin(fromAngleRad);
        var fromCos = Mathf.Cos(fromAngleRad);
        var rotateAngleRad = rotateAngle * Mathf.Deg2Rad;
        var toAngleRad = fromAngleRad + rotateAngleRad;

        if ( isRight ) {
            var tmp = toAngleRad;
            toAngleRad = fromAngleRad;
            fromAngleRad = tmp;
        }

        // �O�Ճf�[�^�̍쐬
        var points = new System.Collections.Generic.List<Vector2>();
        for ( int i = 0 ; i < POINT_NUM + 1 ; ++i ) {
            // �~�^�����̊p�x�v�Z
            var angle = 2 * Mathf.PI * i / POINT_NUM + fromAngleRad;

            // �~�̒[�܂œ��B������A�[�̈ʒu�𒲐�����
            bool isEnd = false;
            if ( angle > toAngleRad ) {
                angle = toAngleRad;
                isEnd = true;
            }

            // ���_�f�[�^�ǉ�
            points.Add(from + new Vector2(-fromSin + Mathf.Sin(angle), fromCos - Mathf.Cos(angle)) * radius);

            // �[�܂œ��B������break
            if ( isEnd ) {
                break;
            }
        }

        // ���̕`��
        DrawLine(points.ToArray());

        // ���̕`��
        Vector2 to;
        if ( isRight ) {
            to = from + new Vector2(-fromSin + Mathf.Sin(fromAngleRad), fromCos - Mathf.Cos(fromAngleRad)) * radius;
            DrawArrowCap(to, fromAngleRad * Mathf.Rad2Deg + 180);
        } else {
            to = from + new Vector2(-fromSin + Mathf.Sin(toAngleRad), fromCos - Mathf.Cos(toAngleRad)) * radius;
            DrawArrowCap(to, toAngleRad * Mathf.Rad2Deg);
        }

        return to;
    }

    /// <summary>
    /// �����擾
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <returns>�ݒ肳�ꂽ����</returns>
    public override float GetSpeed(Vector2 from) {
        var length = radius * rotateAngle * Mathf.Deg2Rad;
        var curSpeed = 0f;
        if ( duration != 0 ) {
            curSpeed = length / duration;
        }
        return curSpeed;
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public override void SetSpeed(Vector2 from, float speed) {
        if ( speed == 0 ) {
            duration = 0;
        } else {
            var length = radius * rotateAngle * Mathf.Deg2Rad;
            duration = length / speed;
        }
    }

    /// <summary>
    /// �I�[�ʒu�̌������擾����
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="fromAngle">�J�n�p�x</param>
    /// <returns>�I�[�ʒu�̌���</returns>
    public override float GetEndDirection(Vector2 from, float fromAngle) {
        if ( fromAngle == NO_DIRECTION ) {
            return NO_DIRECTION;
        }
        return fromAngle + rotateAngle;
    }
#endif
}
