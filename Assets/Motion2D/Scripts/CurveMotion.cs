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
}
