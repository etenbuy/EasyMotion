///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionSequence.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   �A���������[�V�����B                                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
    public SerializedMotion[] sequence;

    /// <summary>
    /// ���[�V���������ւ���
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    public void Replace(int index1, int index2) {
        var tmp = sequence[index1];
        sequence[index1] = sequence[index2];
        sequence[index2] = tmp;
    }

    /// <summary>
    /// �V�K���[�V�������w��ʒu�ɑ}������
    /// </summary>
    /// <param name="index"></param>
    public void InsertNew(int index) {
        var newSequence = new List<SerializedMotion>(sequence);
        newSequence.Insert(index, new SerializedMotion());
        sequence = newSequence.ToArray();
    }

    /// <summary>
    /// �w��ʒu�̃��[�V�������폜����
    /// </summary>
    /// <param name="index"></param>
    public void Remove(int index) {
        var newSequence = new List<SerializedMotion>(sequence);
        newSequence.RemoveAt(index);
        sequence = newSequence.ToArray();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Start() {
    }
}
