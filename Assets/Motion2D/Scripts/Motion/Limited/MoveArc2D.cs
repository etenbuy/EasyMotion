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
    /// ���p�x
    /// </summary>
    private float fromAngle = 0;

    /// <summary>
    /// �����̌����͌��݂̌����̑��Βl�Ƃ��邩�ǂ���
    /// </summary>
    private bool relativeAngle;

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
    private float curAngle = MotionBase2D.NO_DIRECTION;

    /// <summary>
    /// ���p�x(�ʓx�@�\�L)
    /// </summary>
    private float fromAngleRad;

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnStart() {
        if ( !base.OnStart() ) {
            return false;
        }

        // ���s���̏��p�x�̌���
        fromAngleRad = fromAngle;
        if ( rotateAngle < 0 ) {
            // �E����̏ꍇ
            fromAngleRad += 180;
        }

        if ( relativeAngle ) {
            // ���Ίp�x�w��̏ꍇ�͌��݊p�x�����Z
            fromAngleRad += transform.localEulerAngles.z;
        }

        // �ʓx�@�\�L�ɕϊ�
        fromAngleRad *= Mathf.Deg2Rad;

        return true;
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
            .Concat(BitConverter.GetBytes(fromAngle))
            .Concat(BitConverter.GetBytes(relativeAngle))
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

        fromAngle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        relativeAngle = BitConverter.ToBoolean(bytes, offset);
        offset += sizeof(bool);
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
            return curAngle;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        fromAngle = UnityEditor.EditorGUILayout.FloatField("From Angle", fromAngle);
        relativeAngle = UnityEditor.EditorGUILayout.Toggle("Relative Angle", relativeAngle);
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
        if ( !Application.isPlaying ) {
            if ( relativeAngle ) {
                fromAngleRad = (fromAngle + transform.localEulerAngles.z);
            } else {
                fromAngleRad = fromAngle;
            }
            fromAngleRad *= Mathf.Deg2Rad;
        }

        var isRight = rotateAngle < 0;
        if ( isRight ) {
            // �E����̏ꍇ
            fromAngleRad -= Mathf.PI;
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
#endif
}
