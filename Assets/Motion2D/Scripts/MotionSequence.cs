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
    /// ���[�V�����V�[�P���X�����s����
    /// </summary>
    private IEnumerator Start() {
        foreach ( var motion in sequence ) {
            var from = motion.fromCurrent ? Position2D : motion.from;

            switch ( motion.type ) {
            case SerializedMotion.MotionType.Line:
                // �����ړ�
                yield return StartCoroutine(LinerMotion.Move(this, from, motion.to, motion.delay, motion.duration));
                break;

            case SerializedMotion.MotionType.Curve:
                // ����ړ�
                yield return StartCoroutine(CurveMotion.Move(this, from, motion.fromAngle, motion.rotateAngle, motion.radius, motion.delay, motion.duration));
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
                prevTo = motion.fromCurrent ? GetInitPosition2D(motion.fromCurrent) : motion.from;
            }

            var from = motion.fromCurrent ? prevTo : motion.from;

            switch ( motion.type ) {
            case SerializedMotion.MotionType.Line:
                // �����ړ�
                prevTo = LinerMotion.DrawArrow(from, motion.to);
                break;

            case SerializedMotion.MotionType.Curve:
                // ����ړ�
                prevTo = CurveMotion.DrawArrow(from, motion.fromAngle, motion.rotateAngle, motion.radius, false);
                break;

            default:
                // �Î~
                break;
            }
        }
    }
#endif
}
