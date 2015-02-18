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
    /// ����p�x
    /// </summary>
    private float rotateAngle = 0;

    /// <summary>
    /// ���񔼌a
    /// </summary>
    private float radius = 0;

    /// <summary>
    /// �������[�V�����̍X�V����
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedUpdate(float progress) {
        var rotateAngleRad = rotateAngle;
        var fromAngleRad = fromAngle;

        if ( rotateAngleRad < 0 ) {
            // �E����̏ꍇ
            fromAngleRad += 180;
        }

        // �ʓx�@�\�L�ɕϊ�
        rotateAngleRad *= Mathf.Deg2Rad;
        fromAngleRad *= Mathf.Deg2Rad;

        var fromSin = Mathf.Sin(fromAngleRad);
        var fromCos = Mathf.Cos(fromAngleRad);

        // ���݈ʒu�v�Z
        var curAngle = fromAngleRad + progress * rotateAngleRad;

        position = initPosition + new Vector2(
            -fromSin + Mathf.Sin(curAngle),
            fromCos - Mathf.Cos(curAngle)
        ) * radius;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(fromAngle))
            .Concat(BitConverter.GetBytes(rotateAngle))
            .Concat(BitConverter.GetBytes(radius)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y</returns>
    public override int Deserialize(byte[] bytes) {
        var offset = base.Deserialize(bytes);

        fromAngle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        rotateAngle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        radius = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        fromAngle = UnityEditor.EditorGUILayout.FloatField("From Angle", fromAngle);
        rotateAngle = UnityEditor.EditorGUILayout.FloatField("Rotate Angle", rotateAngle);
        radius = UnityEditor.EditorGUILayout.FloatField("Radius", radius);
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    protected override void DrawGizmos() {
        // �~�̒��_��
        const int POINT_NUM = 45;

        // �p�x���̏�����
        var fromAngleRad = fromAngle * Mathf.Deg2Rad;
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
            points.Add(initPosition + new Vector2(-fromSin + Mathf.Sin(angle), fromCos - Mathf.Cos(angle)) * radius);

            // �[�܂œ��B������break
            if ( isEnd ) {
                break;
            }
        }

        // ���̕`��
        DrawLine(points.ToArray());

        // ���̕`��
        if ( isRight ) {
            var toPos = initPosition + new Vector2(-fromSin + Mathf.Sin(fromAngleRad), fromCos - Mathf.Cos(fromAngleRad)) * radius;
            DrawArrowCap(toPos, fromAngleRad * Mathf.Rad2Deg + 180);
        } else {
            var toPos = initPosition + new Vector2(-fromSin + Mathf.Sin(toAngleRad), fromCos - Mathf.Cos(toAngleRad)) * radius;
            DrawArrowCap(toPos, toAngleRad * Mathf.Rad2Deg);
        }
    }
#endif
}
