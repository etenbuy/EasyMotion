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
    };

    // 共通
    public MotionType type;
    public float delay;
    public float duration;
    public bool fromCurrent = true;
    public bool relative = false;
    public Vector2 from;

    // 直線移動用
    public Vector2 to;

    // 旋回移動用
    public float fromAngle;
    public float rotateAngle;
    public float radius;
}
