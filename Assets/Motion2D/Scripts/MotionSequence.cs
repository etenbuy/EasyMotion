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
        public bool fromCurrent = true;
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

#if UNITY_EDITOR
    /// <summary>
    /// �����ʒu
    /// </summary>
    private Vector2 initPosition;
#endif

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
    /// ���[�V�����V�[�P���X�����s����
    /// </summary>
    private IEnumerator Start() {
#if UNITY_EDITOR
        initPosition = Position2D;
#endif

        foreach ( var motion in sequence ) {
            var from = motion.fromCurrent ? Position2D : motion.from;

            switch ( motion.type ) {
            case MotionType.Line:
                // �����ړ�
                yield return StartCoroutine(Line(from, motion.to, motion.delay, motion.duration));
                break;

            case MotionType.Curve:
                // ����ړ�
                yield return StartCoroutine(Curve(from, motion.fromAngle, motion.rotateAngle, motion.radius, motion.delay, motion.duration));
                break;

            default:
                // �Î~
                yield return new WaitForSeconds(motion.delay + motion.duration);
                break;
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// �O�Ղ̕`��(Editor�p)
    /// </summary>
    private void OnDrawGizmos() {
        Vector2 prevTo = Vector2.zero;
        bool isFirst = true;

        foreach ( var motion in sequence ) {
            if ( isFirst ) {
                isFirst = false;
                var initPos = Application.isPlaying ? initPosition : Position2D;
                prevTo = motion.fromCurrent ? initPos : motion.from;
            }

            var from = motion.fromCurrent ? prevTo : motion.from;

            switch ( motion.type ) {
            case MotionType.Line:
                // �����ړ�
                prevTo = DrawLineArrow(from, motion.to, false);
                break;

            case MotionType.Curve:
                // ����ړ�
                prevTo = DrawArcArrow(from, motion.fromAngle, motion.rotateAngle, motion.radius, false);
                break;

            default:
                // �Î~
                break;
            }
        }
    }
#endif
}
