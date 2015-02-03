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

    [Serializable]
    public class SequenceBase {
        public MotionType type;
        public Vector2 from;
        public float delay;
        public float duration;
    };

    [Serializable]
    public class SequenceLine : SequenceBase {
        public Vector2 to;
    };

    [Serializable]
    public class SequenceCurve : SequenceBase {
        public float fromAngle;
        public float rotateAngle;
        public float radius;
    };

    [SerializeField]
    private SequenceBase[] sequence = null;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start() {
    }
}
