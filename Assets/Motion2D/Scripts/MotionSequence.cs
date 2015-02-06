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

    /// <summary>
    /// �V���A���C�Y�ς݃��[�V����
    /// </summary>
    [Serializable]
    public class SerializedMotion {
        // ����
        public MotionType type;
        public float delay;
        public float duration;
        public Vector2 from;

        // �����ړ��p
        public Vector2 to;

        // ����ړ��p
        public float fromAngle;
        public float rotateAngle;
        public float radius;
    }

    /// <summary>
    /// ���[�V�����̈�A�̗���
    /// </summary>
    [SerializeField]
    private SerializedMotion[] sequence;

    /// <summary>
    /// ������
    /// </summary>
    private void Start() {
    }
}
