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

#if UNITY_EDITOR
    private bool isMoving = false;
#endif

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
#if UNITY_EDITOR
        isMoving = true;
#endif

        // 開始まで待機
        yield return new WaitForSeconds(delay);
        // モーション実行
        yield return StartCoroutine(motion);

#if UNITY_EDITOR
        isMoving = false;
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// Gizmo表示色
    /// </summary>
    protected Color GizmoColor {
        get {
            if ( !Application.isPlaying ) {
                return MotionGizmo.EditorColor;
            } else if ( isMoving ) {
                return MotionGizmo.MovingColor;
            } else {
                return MotionGizmo.DisableColor;
            }
        }
    }
#endif
}
