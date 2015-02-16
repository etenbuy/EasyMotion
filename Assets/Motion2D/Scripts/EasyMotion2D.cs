///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EasyMotion2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   2D上のモーション管理。                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 2D上のモーション管理。
/// </summary>
public class EasyMotion2D : MonoBehaviour {
    /// <summary>
    /// モーションの種類定義
    /// </summary>
    public enum MotionType {
        Stop,
        MoveTo,
        MoveArc,
        Liner,
    };

    /// <summary>
    /// 実行時型の定義
    /// </summary>
    private Dictionary<MotionType, Type> runtimeType = new Dictionary<MotionType, Type>() {
        { MotionType.Stop, typeof(MotionBase2D) },
        { MotionType.MoveTo, typeof(MoveTo2D) },
    };

    /// <summary>
    /// モーションの種類
    /// </summary>
    [SerializeField]
    private MotionType type;

    /// <summary>
    /// シリアライズ済みモーションデータ
    /// </summary>
    [SerializeField]
    private byte[] serializedMotion;

    /// <summary>
    /// 実行時のモーションオブジェクト(デシリアライズされたモーションデータ)
    /// </summary>
    private MotionBase2D motion;

    /// <summary>
    /// シリアライズされたモーションデータからインスタンスを生成する
    /// </summary>
    private void Awake() {
        // モーションオブジェクト作成
        motion = Activator.CreateInstance(runtimeType[type]) as MotionBase2D;
        // モーションデータのデシリアライズ
        motion.Deserialize(serializedMotion);
    }

    /// <summary>
    /// 初期化。
    /// </summary>
    private void Start() {
        // モーション実行開始
        motion.StartMotion(this);
    }
}
