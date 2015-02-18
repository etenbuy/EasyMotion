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
    private static Dictionary<MotionType, Type> runtimeType = new Dictionary<MotionType, Type>() {
        { MotionType.Stop, typeof(MotionBase2D) },
        { MotionType.MoveTo, typeof(MoveTo2D) },
        { MotionType.MoveArc, typeof(MoveArc2D) },
    };

    /// <summary>
    /// モーションの種類
    /// </summary>
    public MotionType type = MotionType.Stop;

    /// <summary>
    /// シリアライズ済みモーションデータ
    /// </summary>
    public byte[] serializedMotion = null;

    /// <summary>
    /// 実行時のモーションオブジェクト(デシリアライズされたモーションデータ)
    /// </summary>
    [HideInInspector]
    public MotionBase2D motion = null;

    /// <summary>
    /// シリアライズされたモーションデータからインスタンスを生成する
    /// </summary>
    private void Awake() {
        // デシリアライズされたモーションデータを更新
        UpdateDeserializedMotion();
    }

    /// <summary>
    /// 初期化。
    /// </summary>
    private void Start() {
        // モーション実行開始
        motion.StartMotion(this);
    }

    /// <summary>
    /// デシリアライズされたモーションデータを更新する
    /// </summary>
    public void UpdateDeserializedMotion() {
        // モーションオブジェクト作成
        motion = GetDeserializedMotion(type, serializedMotion);
    }

    /// <summary>
    /// デシリアライズされたモーションオブジェクトを取得する
    /// </summary>
    /// <param name="type">モーション型</param>
    /// <returns>実行時モーションオブジェクト</returns>
    public static MotionBase2D GetDeserializedMotion(EasyMotion2D.MotionType type, byte[] bytes) {
        // モーションオブジェクト作成
        var motion = CreateInstance(type);
        // モーションデータのデシリアライズ
        motion.Deserialize(bytes);
        return motion;
    }

    /// <summary>
    /// 新規の実行時モーションオブジェクトを生成する
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static MotionBase2D CreateInstance(EasyMotion2D.MotionType type) {
        return Activator.CreateInstance(runtimeType[type]) as MotionBase2D;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 初回描画かどうか
    /// </summary>
    private bool isFirstDraw = true;

    /// <summary>
    /// モーションのGizmo描画
    /// </summary>
    private void OnDrawGizmos() {
        if ( isFirstDraw ) {
            if ( !Application.isPlaying ) {
                motion = GetDeserializedMotion(type, serializedMotion);
            }
            isFirstDraw = false;
        }

        motion.DrawGizmos(transform);
    }
#endif
}
