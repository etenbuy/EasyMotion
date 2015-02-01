///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotion.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   直線モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 直線モーション
/// </summary>
public class LinerMotion : MotionBase2D {
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
    /// 終点
    /// </summary>
    [SerializeField]
    private Vector2 to = Vector2.zero;

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
    /// 直線移動コルーチンを実行する
    /// </summary>
    private void Start() {
        if ( fromCurrent ) {
            from = Position2D;
        }

        StartCoroutine(Line(from, to, delay, duration));
    }
}
