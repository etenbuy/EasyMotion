///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   CurveMotion.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   旋回モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 旋回モーション
/// </summary>
public class CurveMotion : MotionBase2D {
    /// <summary>
    /// 現在位置を始点とするかどうか
    /// </summary>
    [SerializeField]
    private bool fromCurrent = false;

    /// <summary>
    /// 始点
    /// </summary>
    [SerializeField]
    private Vector2 from = Vector2.zero;

    /// <summary>
    /// 初角度
    /// </summary>
    [SerializeField]
    private float fromAngle = 0;

    /// <summary>
    /// 旋回角度
    /// </summary>
    [SerializeField]
    private float rotateAngle = 0;

    /// <summary>
    /// 旋回半径
    /// </summary>
    [SerializeField]
    private float radius = 0;

    /// <summary>
    /// 移動開始までの時間
    /// </summary>
    [SerializeField]
    private float delay = 0;

    /// <summary>
    /// 移動時間
    /// </summary>
    [SerializeField]
    private float duration = 0;

    /// <summary>
    /// 旋回移動コルーチンを実行する
    /// </summary>
    private void Start() {
        if ( fromCurrent ) {
            from = Position2D;
        }

        StartCoroutine(Curve(from, fromAngle, rotateAngle, radius, delay, duration));
    }

#if UNITY_EDITOR
    /// <summary>
    /// 軌跡の描画(Editor用)
    /// </summary>
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;

        // 円の頂点数
        const int POINT_NUM = 45;

        // 角度情報
        var fromAngleRad = fromAngle * Mathf.Deg2Rad;
        var isRight = rotateAngle < 0;
        if ( isRight ) {
            // 右旋回の場合
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

        // 始点計算
        var fromPos = (fromCurrent && !Application.isPlaying) ? (Vector2)transform.localPosition : from;

        // 軌跡の描画
        for ( int i = 0 ; i < POINT_NUM ; ++i ) {
            var angleUnit = rotateAngle < 0 ? -i : i;
            var curAngle = 2 * Mathf.PI * i / POINT_NUM + fromAngleRad;
            var nextAngle = 2 * Mathf.PI * (i + 1) / POINT_NUM + fromAngleRad;

            // 描画範囲外の角度なら何もしない
            if ( curAngle > toAngleRad ) {
                continue;
            }

            // 円弧の端のずれ補正
            if ( nextAngle > toAngleRad ) {
                nextAngle = toAngleRad;
            }

            // 円弧の描画
            Gizmos.DrawLine(
                fromPos + new Vector2(-fromSin + Mathf.Sin(curAngle), fromCos - Mathf.Cos(curAngle)) * radius,
                fromPos + new Vector2(-fromSin + Mathf.Sin(nextAngle), fromCos - Mathf.Cos(nextAngle)) * radius);
        }

        // 矢印の描画
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
