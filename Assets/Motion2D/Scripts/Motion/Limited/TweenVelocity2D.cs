///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   TweenVelocity2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   速度変化モーション。                                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 速度変化モーション。
/// </summary>
public class TweenVelocity2D : LimitedMotion2D {
    /// <summary>
    /// 変化前の速度
    /// </summary>
    private Vector2 fromVelocity;

    /// <summary>
    /// 変化後の速度
    /// </summary>
    private Vector2 toVelocity;

    /// <summary>
    /// 現在時刻
    /// </summary>
    private float curTime;

    /// <summary>
    /// 時限モーションの初期化処理
    /// </summary>
    /// <param name="progress">進捗率</param>
    protected override void OnLimitedStart() {
    }

    /// <summary>
    /// 時限モーションの更新処理
    /// </summary>
    /// <param name="progress">進捗率</param>
    protected override void OnLimitedUpdate(float progress) {
        curTime = duration * progress;
        position = initPosition + GetPosition(curTime);
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(fromVelocity.x))
            .Concat(BitConverter.GetBytes(fromVelocity.y))
            .Concat(BitConverter.GetBytes(toVelocity.x))
            .Concat(BitConverter.GetBytes(toVelocity.y)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        fromVelocity.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        fromVelocity.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        toVelocity.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        toVelocity.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

    /// <summary>
    /// 現在の向き
    /// </summary>
    public override float currentDirection {
        get {
            Vector2 vel;
            if ( duration == 0 ) {
                vel = toVelocity;
            } else {
                vel = fromVelocity + (toVelocity - fromVelocity) * curTime / duration;
            }
            return Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        }
    }

    /// <summary>
    /// 初速度を指定する
    /// </summary>
    /// <param name="vel">初速度</param>
    public override void SetInitVelocity(Vector2 vel) {
        fromVelocity = vel;
    }

    /// <summary>
    /// 位置を時刻より取得する
    /// </summary>
    /// <param name="time">時刻</param>
    /// <returns>位置</returns>
    private Vector2 GetPosition(float time) {
        if ( duration == 0 ) {
            return Vector2.zero;
        }

        Vector2 result;
        result = (toVelocity - fromVelocity) * time / (2 * duration) + fromVelocity;
        result *= time;
        return result;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        fromVelocity = UnityEditor.EditorGUILayout.Vector2Field("From Velocity", fromVelocity);
        toVelocity = UnityEditor.EditorGUILayout.Vector2Field("To Velocity", toVelocity);
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    /// <param name="from">現在位置</param>
    /// <returns>移動後の位置</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        const int POINT_NUM = 20;

        Vector2[] points = new Vector2[POINT_NUM + 1];
        for ( int i = 0 ; i < POINT_NUM ; ++i ) {
            points[i] = from + GetPosition((float)i / POINT_NUM * duration);
        }
        var to = points[POINT_NUM] = from + GetPosition(duration);
        DrawLine(points);
        DrawArrowCap(to, Mathf.Atan2(toVelocity.y, toVelocity.x) * Mathf.Rad2Deg);

        return to;
    }

    /// <summary>
    /// 速さ取得
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <returns>設定された速さ</returns>
    public override float GetSpeed(Vector2 from) {
        var curSpeed = 0f;
        if ( duration != 0 ) {
            curSpeed = fromVelocity.magnitude;
        }
        return curSpeed;
    }

    /// <summary>
    /// 速さ設定
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <param name="speed">速さ</param>
    public override void SetSpeed(Vector2 from, float speed) {
        fromVelocity = fromVelocity.normalized * speed;
    }

    /// <summary>
    /// 終端位置の向きを取得する
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <param name="fromAngle">開始角度</param>
    /// <returns>終端位置の向き</returns>
    public override float GetEndDirection(Vector2 from, float fromAngle) {
        if ( toVelocity == Vector2.zero ) {
            return initDirection;
        }

        return Mathf.Atan2(toVelocity.y, toVelocity.x) * Mathf.Rad2Deg;
    }
#endif
}
