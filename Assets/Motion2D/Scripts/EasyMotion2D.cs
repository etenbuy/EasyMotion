///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EasyMotion2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   2D上のモーション管理。                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 2D上のモーション管理。
/// </summary>
public class EasyMotion2D : MonoBehaviour {
    /// <summary>
    /// モーションの種類定義
    /// </summary>
    private enum MotionType {
        Stop,
        MoveTo,
        MoveArc,
        Liner,
    };

    /// <summary>
    /// モーションの種類
    /// </summary>
    [SerializeField]
    private MotionType type = MotionType.Stop;

    /// <summary>
    /// 実行対象のモーション
    /// </summary>
    [SerializeField]
    private MotionBase2D motion;

    /// <summary>
    /// 初期化。
    /// </summary>
    private void Start() {
        motion.StartMotion(this);
    }
}
