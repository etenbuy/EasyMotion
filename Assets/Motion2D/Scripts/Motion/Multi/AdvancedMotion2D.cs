///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   AdvancedMotion2D.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.15                                                                       //
//  Desc    :   拡張モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 拡張モーション。
/// </summary>
public class AdvancedMotion2D : MotionBase2D {
    /// <summary>
    /// モーションタイプ
    /// </summary>
    private EasyMotion2D.MotionType motionType;

    /// <summary>
    /// モーション
    /// </summary>
    private MotionBase2D motion;

    /// <summary>
    /// 回転モーションタイプ
    /// </summary>
    private RotationBase2D.RotationType rotationType;

    /// <summary>
    /// 回転モーション
    /// </summary>
    private RotationBase2D rotation;

    /// <summary>
    /// モーションは継続するかどうか
    /// </summary>
    private bool motionUpdate = true;

    /// <summary>
    /// 回転モーションは継続するかどうか
    /// </summary>
    private bool rotationUpdate = true;

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    protected override void OnInit() {
        base.OnInit();

        motion.InitMotion(transform, initDirection);
        rotation.motion = this;
    }

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnStart() {
        motion.StartMotion();
        rotation.StartRotation(transform);
        return true;
    }

    /// <summary>
    /// モーションの更新処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnUpdate() {
        if ( motionUpdate ) {
            motionUpdate = motion.UpdateMotion();
            position = motion.position;
        }

        if ( rotationUpdate ) {
            rotationUpdate = rotation.UpdateRotation();
        }

        return motionUpdate;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        if ( motion == null ) {
            motion = EasyMotion2D.CreateInstance(EasyMotion2D.MotionType.Stop);
        }
        if ( rotation == null ) {
            rotation = RotationBase2D.CreateInstance(RotationBase2D.RotationType.None);
        }

        var motionType = EasyMotion2D.GetSerializedType(motion.GetType());
        var rotationType = RotationBase2D.GetSerializedType(rotation.GetType());

        result = result
            .Concat(BitConverter.GetBytes((int)motionType))
            .Concat(motion.Serialize())
            .Concat(BitConverter.GetBytes((int)rotationType))
            .Concat(rotation.Serialize()).ToArray();

        return result;
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        motionType = (EasyMotion2D.MotionType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        motion = EasyMotion2D.GetDeserializedMotion(motionType, bytes, offset, out offset);

        rotationType = (RotationBase2D.RotationType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        rotation = RotationBase2D.GetDeserializedRotation(rotationType, bytes, offset, out offset);

        return offset;
    }

    /// <summary>
    /// 現在の向き
    /// </summary>
    public override float currentDirection {
        get {
            return motion.currentDirection;
        }
    }

    /// <summary>
    /// 初速度を指定する
    /// </summary>
    /// <param name="vel">初速度</param>
    public override void SetInitVelocity(Vector2 vel) {
        motion.SetInitVelocity(vel);
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();

        var curMotionType = EasyMotion2D.GetSerializedType(motion.GetType());
        var newMotionType = (EasyMotion2D.MotionType)UnityEditor.EditorGUILayout.EnumPopup("Motion Type", curMotionType);
        if ( newMotionType != curMotionType ) {
            motion = EasyMotion2D.CreateInstance(newMotionType);
        }

        motion.DrawGUI();

        var curRotationType = RotationBase2D.GetSerializedType(rotation.GetType());
        var newRotationType = (RotationBase2D.RotationType)UnityEditor.EditorGUILayout.EnumPopup("Rotation Type", curRotationType);
        if ( newRotationType != curRotationType ) {
            rotation = RotationBase2D.CreateInstance(newRotationType);
        }

        rotation.DrawGUI();
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    /// <param name="from">現在位置</param>
    /// <returns>移動後の位置</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        return motion.DrawGizmos(transform, from);
    }

    /// <summary>
    /// 速さ取得
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <returns>設定された速さ</returns>
    public override float GetSpeed(Vector2 from) {
        return motion.GetSpeed(from);
    }

    /// <summary>
    /// 速さ設定
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <param name="speed">速さ</param>
    public override void SetSpeed(Vector2 from, float speed) {
        motion.SetSpeed(from, speed);
    }
#endif
}
