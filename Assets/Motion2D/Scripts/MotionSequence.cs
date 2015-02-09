///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionSequence.cs                                                                //
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
    public SerializedMotion[] sequence;

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
        var newSequence = new List<SerializedMotion>(sequence);
        newSequence.Insert(index, new SerializedMotion());
        sequence = newSequence.ToArray();
    }

    /// <summary>
    /// 指定位置のモーションを削除する
    /// </summary>
    /// <param name="index"></param>
    public void Remove(int index) {
        var newSequence = new List<SerializedMotion>(sequence);
        newSequence.RemoveAt(index);
        sequence = newSequence.ToArray();
    }

    /// <summary>
    /// モーションシーケンスを実行する
    /// </summary>
    private IEnumerator Start() {
        foreach ( var motion in sequence ) {
            switch ( motion.type ) {
            case MotionType.Line:
                // 直線移動
                yield return StartCoroutine(Line(motion.from, motion.to, motion.delay, motion.duration));
                break;

            case MotionType.Curve:
                // 旋回移動
                yield return StartCoroutine(Curve(motion.from, motion.fromAngle, motion.rotateAngle, motion.radius, motion.delay, motion.duration));
                break;

            default:
                // 静止
                yield return new WaitForSeconds(motion.delay + motion.duration);
                break;
            }
        }
    }
}
