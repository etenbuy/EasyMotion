///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveArc2D.cs                                                                     //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   ���񃂁[�V�����B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ���񃂁[�V����
/// </summary>
public class MoveArc2D : LimitedMotion2D {
    /// <summary>
    /// ���p�x
    /// </summary>
    [SerializeField]
    private float fromAngle = 0;

    /// <summary>
    /// ����p�x
    /// </summary>
    [SerializeField]
    private float rotateAngle = 0;

    /// <summary>
    /// ���񔼌a
    /// </summary>
    [SerializeField]
    private float radius = 0;

    /// <summary>
    /// ����ړ��R���[�`�������s����
    /// </summary>
    private void Start() {
        if ( fromCurrent ) {
            from = Position2D;
        }

        StartMotion(Move(this, from, fromAngle, rotateAngle, radius, duration));
    }

    /// <summary>
    /// ����ړ�
    /// </summary>
    /// <param name="motion">���[�V�����I�u�W�F�N�g</param>
    /// <param name="from">�n�_</param>
    /// <param name="fromAngle">���p�x</param>
    /// <param name="rotateAngle">����p�x</param>
    /// <param name="radius">���񔼌a</param>
    /// <param name="duration">���[�V��������</param>
    /// <returns></returns>
    public static IEnumerator Move(MotionBase2D motion, Vector2 from, float fromAngle, float rotateAngle, float radius, float duration) {
        var startTime = Time.time;
        var endTime = startTime + duration;

        if ( rotateAngle < 0 ) {
            // �E����̏ꍇ
            fromAngle += 180;
        }

        // ���񃂁[�V�������s
        rotateAngle *= Mathf.Deg2Rad;
        fromAngle *= Mathf.Deg2Rad;

        var fromSin = Mathf.Sin(fromAngle);
        var fromCos = Mathf.Cos(fromAngle);

        while ( Time.time < endTime ) {
            // ���݈ʒu�v�Z
            var progress = (Time.time - startTime) / duration;
            var curAngle = fromAngle + progress * rotateAngle;

            motion.Position2D = from + new Vector2(
                -fromSin + Mathf.Sin(curAngle),
                fromCos - Mathf.Cos(curAngle)
            ) * radius;

            yield return 0;
        }

        // �I��
        var toAngle = fromAngle + rotateAngle;
        motion.Position2D = from + new Vector2(
            -fromSin + Mathf.Sin(toAngle),
            fromCos - Mathf.Cos(toAngle)
        ) * radius;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �O������������`�悷��
    /// </summary>
    /// <param name="from"></param>
    /// <param name="fromAngle"></param>
    /// <param name="rotateAngle"></param>
    /// <param name="radius"></param>
    /// <param name="fromCurrent"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Vector2 DrawArrow(Vector2 from, float fromAngle, float rotateAngle, float radius, bool fromCurrent, Color color) {
        // �~�̒��_��
        const int POINT_NUM = 45;

        // �p�x���
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
        var points = new List<Vector2>();
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
        Vector2 toPos;
        if ( isRight ) {
            toPos = from + new Vector2(-fromSin + Mathf.Sin(fromAngleRad), fromCos - Mathf.Cos(fromAngleRad)) * radius;
            MotionGizmo.DrawArrowCap(toPos, fromAngleRad * Mathf.Rad2Deg + 180, color);
        } else {
            toPos = from + new Vector2(-fromSin + Mathf.Sin(toAngleRad), fromCos - Mathf.Cos(toAngleRad)) * radius;
            MotionGizmo.DrawArrowCap(toPos, toAngleRad * Mathf.Rad2Deg, color);
        }

        MotionGizmo.DrawArrow(points.ToArray(), color, false, false);

        return toPos;
    }

    /// <summary>
    /// �O�Ղ̕`��(Editor�p)
    /// </summary>
    private void OnDrawGizmos() {
        DrawArrow(InitPosition2D, fromAngle, rotateAngle, radius, fromCurrent, GizmoColor);
    }
#endif
}