///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionSequence.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   連続したモーション。                                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 連続したモーション
/// </summary>
public class MotionSequence : MotionBase2D {
    /// <summary>
    /// モーションタイプ
    /// </summary>
    public enum MotionType {
        Base,
        Line,
        Curve,
    };

    /// <summary>
    /// シリアライズ済みモーション
    /// </summary>
    [Serializable]
    public class SerializedMotion {
        // 共通
        public MotionType type;
        public float delay;
        public float duration;
        public Vector2 from;

        // 直線移動用
        public Vector2 to;

        // 旋回移動用
        public float fromAngle;
        public float rotateAngle;
        public float radius;
    }

    /// <summary>
    /// モーションの一連の流れ
    /// </summary>
    [SerializeField]
    private SerializedMotion[] sequence;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start() {
    }
}
