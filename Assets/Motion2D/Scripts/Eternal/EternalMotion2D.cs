///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EternalMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   永久モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 永久モーション。
/// </summary>
public class EternalMotion2D : MotionBase2D {
    /// <summary>
    /// 開始時刻
    /// </summary>
    private float startTime;

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnStart() {
        startTime = Time.time;
        return true;
    }

    /// <summary>
    /// モーションの更新処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnUpdate() {
        // 永久モーション更新
        return OnEternalUpdate(Time.time - startTime);
    }

    /// <summary>
    /// 永久モーションの更新処理(派生クラスで実装する)
    /// </summary>
    /// <param name="time">経過時間</param>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected virtual bool OnEternalUpdate(float time) {
        return false;
    }
}
