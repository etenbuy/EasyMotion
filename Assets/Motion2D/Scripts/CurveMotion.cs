///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   CurveMotion.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   ���񃂁[�V�����B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// ���񃂁[�V����
/// </summary>
public class CurveMotion : MotionBase2D {
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
    /// �ړ��J�n�܂ł̎���
    /// </summary>
    [SerializeField]
    private float delay = 0;

    /// <summary>
    /// �ړ�����
    /// </summary>
    [SerializeField]
    private float duration = 0;

    /// <summary>
    /// ����ړ��R���[�`�������s����
    /// </summary>
    private void Start() {
        if ( fromCurrent ) {
            from = Position2D;
        }

        StartCoroutine(Curve(from, fromAngle, rotateAngle, radius, delay, duration));
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
    /// <returns></returns>
    public static Vector2 DrawArrow(Vector2 from, float fromAngle, float rotateAngle, float radius, bool fromCurrent) {
        Gizmos.color = Color.cyan;

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

        // �O�Ղ̕`��
        for ( int i = 0 ; i < POINT_NUM ; ++i ) {
            var curAngle = 2 * Mathf.PI * i / POINT_NUM + fromAngleRad;
            var nextAngle = 2 * Mathf.PI * (i + 1) / POINT_NUM + fromAngleRad;

            // �`��͈͊O�̊p�x�Ȃ牽�����Ȃ�
            if ( curAngle > toAngleRad ) {
                continue;
            }

            // �~�ʂ̒[�̂���␳
            if ( nextAngle > toAngleRad ) {
                nextAngle = toAngleRad;
            }

            // �~�ʂ̕`��
            Gizmos.DrawLine(
                from + new Vector2(-fromSin + Mathf.Sin(curAngle), fromCos - Mathf.Cos(curAngle)) * radius,
                from + new Vector2(-fromSin + Mathf.Sin(nextAngle), fromCos - Mathf.Cos(nextAngle)) * radius);
        }

        // ���̕`��
        Vector2 toPos;
        if ( isRight ) {
            toPos = from + new Vector2(-fromSin + Mathf.Sin(fromAngleRad), fromCos - Mathf.Cos(fromAngleRad)) * radius;
            MotionGizmo.DrawArrowCap(toPos, fromAngleRad * Mathf.Rad2Deg + 180);
        } else {
            toPos = from + new Vector2(-fromSin + Mathf.Sin(toAngleRad), fromCos - Mathf.Cos(toAngleRad)) * radius;
            MotionGizmo.DrawArrowCap(toPos, toAngleRad * Mathf.Rad2Deg);
        }

        return toPos;
    }

    /// <summary>
    /// �O�Ղ̕`��(Editor�p)
    /// </summary>
    private void OnDrawGizmos() {
        DrawArrow(InitPosition2D, fromAngle, rotateAngle, radius, fromCurrent);
    }
#endif
}
