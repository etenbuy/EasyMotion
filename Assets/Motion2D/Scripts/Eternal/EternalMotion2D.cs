///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EternalMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   無限モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 無限モーションの基底クラス
/// </summary>
public class EternalMotion2D : MotionBase2D {
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

#if UNITY_EDITOR
    /// <summary>
    /// Gizmo表示色
    /// </summary>
    protected Color GizmoColor {
        get {
            if ( !Application.isPlaying ) {
                return MotionGizmo.EditorColor;
            } else {
                return MotionGizmo.MovingColor;
            }
        }
    }
#endif
}
