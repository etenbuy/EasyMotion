///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LimitedMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   時限モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 時限モーション。
/// </summary>
public class LimitedMotion2D : MotionBase2D {
    /// <summary>
    /// 移動開始までの時間
    /// </summary>
    [SerializeField]
    protected float delay = 0;

    /// <summary>
    /// 移動時間
    /// </summary>
    [SerializeField]
    protected float duration = 0;

    /// <summary>
    /// モーションを開始する
    /// </summary>
    /// <param name="motion"></param>
    protected Coroutine StartMotion(IEnumerator motion) {
        return StartCoroutine(ExecuteMotion(motion));
    }

    /// <summary>
    /// モーション実行
    /// </summary>
    /// <param name="motion"></param>
    /// <returns></returns>
    private IEnumerator ExecuteMotion(IEnumerator motion) {
        // 開始まで待機
        yield return new WaitForSeconds(delay);
        // モーション実行
        yield return StartCoroutine(motion);
    }
}
