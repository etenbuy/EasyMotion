///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   RotationBase2D.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   2D回転動作の基底クラス。                                                         //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 2D回転動作の基底クラス。
/// </summary>
[Serializable]
public class RotationBase2D {
    /// <summary>
    /// 回転を開始するまでの時間
    /// </summary>
    public float delay;

    /// <summary>
    /// 現在の角度
    /// </summary>
    public float angle;

    /// <summary>
    /// モーション
    /// </summary>
    public MotionBase2D motion;

    /// <summary>
    /// 回転動作の種類
    /// </summary>
    public enum RotationType {
        None,
        Rotate,
        Forward,
    };

    /// <summary>
    /// 回転動作の種類
    /// </summary>
    public RotationType type = RotationType.None;

    /// <summary>
    /// 実行時の回転動作状態定義
    /// </summary>
    private enum State {
        Disable,
        Waiting,
        Running,
    }

    /// <summary>
    /// 実行時の回転動作状態
    /// </summary>
    private State state = State.Disable;

    /// <summary>
    /// GameObjectのTransform
    /// </summary>
    protected Transform transform;

    /// <summary>
    /// 開始時刻
    /// </summary>
    protected float startTime { get; private set; }

    /// <summary>
    /// OnStart()は呼び出されたかどうか
    /// </summary>
    private bool onStartCalled = false;

    /// <summary>
    /// 回転動作を開始する
    /// </summary>
    /// <param name="objTrans">GameObjectのTransform</param>
    public void StartRotation(Transform objTrans) {
        state = delay > 0 ? State.Waiting : State.Running;
        transform = objTrans.transform;
        startTime = Time.time;
        angle = transform.localEulerAngles.z;
    }

    /// <summary>
    /// 回転動作状態を更新する
    /// </summary>
    public bool UpdateRotation() {
        switch ( state ) {
        case State.Waiting:
            // 開始までの一定時間待機
            if ( Time.time - startTime >= delay ) {
                // 回転動作実行に遷移
                state = State.Running;
            }
            return true;

        case State.Running:
            // 回転動作実行
            if ( !onStartCalled ) {
                onStartCalled = true;
                if ( !OnStart() ) {
                    return false;
                }
            }

            // 更新動作
            var nextUpdate = OnUpdate();

            // 向き更新
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y,
                angle
            );

            if ( !nextUpdate ) {
                // 回転動作終了なら無効状態に遷移
                state = State.Disable;
            }

            return nextUpdate;
        }

        return false;
    }

    /// <summary>
    /// 回転動作の初期化処理(派生クラスで実装する)
    /// </summary>
    /// <returns>true:回転動作継続 / false:以降の回転動作を継続しない</returns>
    protected virtual bool OnStart() {
        return false;
    }

    /// <summary>
    /// 回転動作の更新処理(派生クラスで実装する)
    /// </summary>
    /// <returns>true:回転動作継続 / false:以降の回転動作を継続しない</returns>
    protected virtual bool OnUpdate() {
        return false;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public virtual byte[] Serialize() {
        var result = BitConverter.GetBytes(delay);
        return result;
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済み回転動作データ</param>
    /// <param name="offset">回転動作データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public virtual int Deserialize(byte[] bytes, int offset) {
        delay = BitConverter.ToSingle(bytes, offset);
        return offset + sizeof(float);
    }
}
