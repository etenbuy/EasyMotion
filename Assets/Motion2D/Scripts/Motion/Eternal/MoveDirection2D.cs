///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveDirection2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.25                                                                       //
//  Desc    :   指定された向きに直進するモーション。                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 指定された向きに直進するモーション。
/// </summary>
public class MoveDirection2D : EternalMotion2D {
    /// <summary>
    /// 直進する速さ
    /// </summary>
    private float speed;

    /// <summary>
    /// 直進する向き
    /// </summary>
    private Direction2D moveDirection;

    /// <summary>
    /// 移動速度
    /// </summary>
    private Vector2 velocity;

    /// <summary>
    /// 現在の向き
    /// </summary>
    private float curAngle = NO_DIRECTION;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MoveDirection2D() {
        moveDirection = new Direction2D(this);
    }

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    protected override void OnInit() {
        base.OnInit();
        // 内部変数の初期化
        UpdateParam();
    }

    /// <summary>
    /// 永久モーションの初期化処理
    /// </summary>
    protected override void OnEternalStart() {
        // 内部変数の初期化
        UpdateParam();
    }

    /// <summary>
    /// 永久モーションの更新処理(派生クラスで実装する)
    /// </summary>
    /// <param name="time">モーション開始からの経過時間</param>
    /// <param name="deltaTime">前回フレームからの経過時間</param>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnEternalUpdate(float time, float deltaTime) {
        position = initPosition + velocity * time;
        return true;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(speed))
            .Concat(moveDirection.Serialize()).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        offset = moveDirection.Deserialize(bytes, offset);

        return offset;
    }

    /// <summary>
    /// 現在の向き
    /// </summary>
    public override float currentDirection {
        get {
            return curAngle;
        }
    }

    /// <summary>
    /// 初速度を指定する
    /// </summary>
    /// <param name="vel">初速度</param>
    public override void SetInitVelocity(Vector2 vel) {
        velocity = vel;
        moveDirection.type = Direction2D.Type.None;
        moveDirection.angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        UpdateParam();
    }

    /// <summary>
    /// 内部的に使用する進行方向・速度を更新する
    /// </summary>
    private void UpdateParam() {
        // 進行方向の計算
        curAngle = moveDirection.direction;

        // 移動速度の計算
        var curAngleRad = curAngle * Mathf.Deg2Rad;
        velocity = new Vector2(Mathf.Cos(curAngleRad), Mathf.Sin(curAngleRad)) * speed;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        speed = UnityEditor.EditorGUILayout.FloatField("Speed", speed);
        moveDirection.DrawGUI();
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    /// <param name="from">現在位置</param>
    /// <returns>移動後の位置</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        if ( !Application.isPlaying ) {
            UpdateParam();
        }

        var to = from + velocity;
        DrawLine(from, to);
        DrawArrowCap(to, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);

        return from + velocity * float.MaxValue;
    }

    /// <summary>
    /// 速さ取得
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <returns>設定された速さ</returns>
    public override float GetSpeed(Vector2 from) {
        return velocity.magnitude;
    }

    /// <summary>
    /// 速さ設定
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <param name="speed">速さ</param>
    public override void SetSpeed(Vector2 from, float speed) {
        velocity = velocity.normalized * speed;
    }

    /// <summary>
    /// 終端位置の向きを取得する
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <param name="fromAngle">開始角度</param>
    /// <returns>終端位置の向き</returns>
    public override float GetEndDirection(Vector2 from, float fromAngle) {
        return Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
    }
#endif
}
