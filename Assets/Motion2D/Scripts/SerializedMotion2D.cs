///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SerializedMotion.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   シリアライズ済みモーション情報。                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// シリアライズ済みモーション情報。
/// </summary>
[Serializable]
public class SerializedMotion2D {
    /// <summary>
    /// モーションタイプ
    /// </summary>
    public enum MotionType {
        Stop,
        MoveTo,
        MoveArc,
        Liner,
    };

    /// <summary>
    /// モーションタイプと実行時型の紐付け情報
    /// </summary>
    private static Dictionary<MotionType, Type> typeAssoc = new Dictionary<MotionType, Type>() {
        { MotionType.Stop, typeof(MotionBase2D) },
        { MotionType.MoveTo, typeof(MoveTo2D) },
        { MotionType.MoveArc, typeof(MoveArc2D) },
        { MotionType.Liner, typeof(LinerMotion2D) },
    };

    /// <summary>
    /// 実行時型を取得する
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetType(MotionType type) {
        return typeAssoc[type];
    }

    // 共通
    public MotionType type;
    public float delay;
    public bool fromCurrent = true;
    public bool relative = false;
    public Vector2 from;

    // LimitedMotion共通
    public float duration;

    // MoveTo
    public Vector2 to;

    // MoveArc
    public float fromAngle;
    public float rotateAngle;
    public float radius;

    // LinerMotion
    public Vector2 velocity;
}
