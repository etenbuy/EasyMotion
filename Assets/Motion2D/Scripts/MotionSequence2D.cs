///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionSequence2D.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   連続したモーション。                                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 連続したモーション
/// </summary>
public class MotionSequence2D : LimitedMotion2D {
    /// <summary>
    /// モーションの一連の流れ
    /// </summary>
    [SerializeField]
    public SerializedMotion2D[] sequence;

    /// <summary>
    /// モーションを入れ替える
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    public void Replace(int index1, int index2) {
        var tmp = sequence[index1];
        sequence[index1] = sequence[index2];
        sequence[index2] = tmp;
    }

    /// <summary>
    /// 新規モーションを指定位置に挿入する
    /// </summary>
    /// <param name="index"></param>
    public void InsertNew(int index) {
        var newSequence = new List<SerializedMotion2D>(sequence);
        newSequence.Insert(index, new SerializedMotion2D());
        sequence = newSequence.ToArray();
    }

    /// <summary>
    /// 指定位置のモーションを削除する
    /// </summary>
    /// <param name="index"></param>
    public void Remove(int index) {
        var newSequence = new List<SerializedMotion2D>(sequence);
        newSequence.RemoveAt(index);
        sequence = newSequence.ToArray();
    }

    /// <summary>
    /// モーションシーケンスを実行する
    /// </summary>
    private IEnumerator Start() {
        foreach ( var motion in sequence ) {
            var from = motion.fromCurrent ? Position2D : motion.from;

            switch ( motion.type ) {
            case SerializedMotion2D.MotionType.MoveTo:
                // 直線移動
                yield return StartMotion(MoveTo2D.Move(this, from, motion.relative ? motion.to + from : motion.to, motion.duration));
                break;

            case SerializedMotion2D.MotionType.MoveArc:
                // 旋回移動
                yield return StartMotion(MoveArc2D.Move(this, from, motion.fromAngle, motion.rotateAngle, motion.radius, motion.duration));
                break;

            default:
                // 静止
                yield return new WaitForSeconds(motion.delay + motion.duration);
                break;
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// 軌跡の描画(Editor用)
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
            case SerializedMotion2D.MotionType.MoveTo:
                // 直線移動
                prevTo = MoveTo2D.DrawArrow(from, motion.relative ? motion.to + from : motion.to);
                break;

            case SerializedMotion2D.MotionType.MoveArc:
                // 旋回移動
                prevTo = MoveArc2D.DrawArrow(from, motion.fromAngle, motion.rotateAngle, motion.radius, false);
                break;

            default:
                // 静止
                break;
            }
        }
    }
#endif
}
