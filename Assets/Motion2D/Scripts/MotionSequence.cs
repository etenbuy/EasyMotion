///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionSequence.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   �A���������[�V�����B                                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// �A���������[�V����
/// </summary>
public class MotionSequence : MotionBase2D {
    /// <summary>
    /// ���[�V�����^�C�v
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
    /// ������
    /// </summary>
    private void Start() {
    }
}
