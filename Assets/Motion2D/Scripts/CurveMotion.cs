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
    /// ���݈ʒu���n�_�Ƃ��邩�ǂ���
    /// </summary>
    [SerializeField]
    private bool fromCurrent = false;

    /// <summary>
    /// �n�_
    /// </summary>
    [SerializeField]
    private Vector2 from = Vector2.zero;

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
    /// �O�Ղ̕`��(Editor�p)
    /// </summary>
    private void OnDrawGizmos() {
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

        // �n�_�v�Z
        var fromPos = (fromCurrent && !Application.isPlaying) ? (Vector2)transform.localPosition : from;

        // �O�Ղ̕`��
        for ( int i = 0 ; i < POINT_NUM ; ++i ) {
            var angleUnit = rotateAngle < 0 ? -i : i;
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
                fromPos + new Vector2(-fromSin + Mathf.Sin(curAngle), fromCos - Mathf.Cos(curAngle)) * radius,
                fromPos + new Vector2(-fromSin + Mathf.Sin(nextAngle), fromCos - Mathf.Cos(nextAngle)) * radius);
        }

        // ���̕`��
        if ( isRight ) {
            var toPos = fromPos + new Vector2(-fromSin + Mathf.Sin(fromAngleRad), fromCos - Mathf.Cos(fromAngleRad)) * radius;
            DrawArrowCap(toPos, fromAngleRad * Mathf.Rad2Deg + 180);
        }else{
            var toPos = fromPos + new Vector2(-fromSin + Mathf.Sin(toAngleRad), fromCos - Mathf.Cos(toAngleRad)) * radius;
            DrawArrowCap(toPos, toAngleRad * Mathf.Rad2Deg);
        }
    }
#endif
}
