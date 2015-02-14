///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionSequence2D.cs                                                              //
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
public class MotionSequence2D : LimitedMotion2D {
    /// <summary>
    /// ���[�V�����̈�A�̗���
    /// </summary>
    [SerializeField]
    public SerializedMotion2D[] sequence;

#if UNITY_EDITOR
    /// <summary>
    /// ���ݎ��s���̃��[�V����
    /// </summary>
    private SerializedMotion2D current = null;

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
        var newSequence = new List<SerializedMotion2D>(sequence);
        newSequence.Insert(index, new SerializedMotion2D());
        sequence = newSequence.ToArray();
    }

    /// <summary>
    /// �w��ʒu�̃��[�V�������폜����
    /// </summary>
    /// <param name="index"></param>
    public void Remove(int index) {
        var newSequence = new List<SerializedMotion2D>(sequence);
        newSequence.RemoveAt(index);
        sequence = newSequence.ToArray();
    }
#endif

    /// <summary>
    /// ���[�V�����V�[�P���X�����s����
    /// </summary>
    private IEnumerator Start() {
        foreach ( var motion in sequence ) {
            var from = motion.fromCurrent ? Position2D : motion.from;

#if UNITY_EDITOR
            current = motion;
#endif

            switch ( motion.type ) {
            case SerializedMotion2D.MotionType.MoveTo:
                yield return StartMotion(MoveTo2D.Move(this, from, motion.relative ? motion.to + from : motion.to, motion.duration));
                break;

            case SerializedMotion2D.MotionType.MoveArc:
                yield return StartMotion(MoveArc2D.Move(this, from, motion.fromAngle, motion.rotateAngle, motion.radius, motion.duration));
                break;

            case SerializedMotion2D.MotionType.Liner:
                yield return StartMotion(LinerMotion2D.Move(this, from, motion.velocity));
                break;

            default:
                yield return new WaitForSeconds(motion.delay + motion.duration);
                break;
            }
        }

#if UNITY_EDITOR
        current = null;
#endif
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

            Color color;
            if ( !Application.isPlaying ) {
                color = MotionGizmo.EditorColor;
            } else if ( motion == current ) {
                color = MotionGizmo.MovingColor;
            } else {
                color = MotionGizmo.DisableColor;
            }

            switch ( motion.type ) {
            case SerializedMotion2D.MotionType.MoveTo:
                prevTo = MoveTo2D.DrawArrow(from, motion.relative ? motion.to + from : motion.to, color);
                break;

            case SerializedMotion2D.MotionType.MoveArc:
                prevTo = MoveArc2D.DrawArrow(from, motion.fromAngle, motion.rotateAngle, motion.radius, false, color);
                break;

            case SerializedMotion2D.MotionType.Liner:
                prevTo = LinerMotion2D.DrawArrow(from, motion.velocity, color);
                break;

            default:
                break;
            }

            if ( SerializedMotion2D.GetType(motion.type).IsSubclassOf(typeof(EternalMotion2D)) ) {
                // �������[�V�����Ȃ炻��ȍ~�ɐi�߂Ȃ����ߏI��
                break;
            }
        }
    }
#endif
}
