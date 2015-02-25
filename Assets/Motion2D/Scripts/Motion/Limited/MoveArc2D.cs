///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveArc2D.cs                                                                     //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   旋回軌道を描くモーション。                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 旋回軌道を描くモーション。
/// </summary>
public class MoveArc2D : LimitedMotion2D {
    /// <summary>
    /// 初角度
    /// </summary>
    private float fromAngle = 0;

    /// <summary>
    /// 初期の向きは現在の向きの相対値とするかどうか
    /// </summary>
    private bool relativeAngle;

    /// <summary>
    /// 旋回角度
    /// </summary>
    private float rotateAngle = 0;

    /// <summary>
    /// 旋回半径
    /// </summary>
    private float radius = 0;

    /// <summary>
    /// 現在の向き
    /// </summary>
    private float curAngle = MotionBase2D.NO_DIRECTION;

    /// <summary>
    /// 初角度(弧度法表記)
    /// </summary>
    private float fromAngleRad;

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnStart() {
        if ( !base.OnStart() ) {
            return false;
        }

        // 実行時の初角度の決定
        fromAngleRad = fromAngle;
        if ( rotateAngle < 0 ) {
            // 右旋回の場合
            fromAngleRad += 180;
        }

        if ( relativeAngle ) {
            // 相対角度指定の場合は現在角度を加算
            fromAngleRad += transform.localEulerAngles.z;
        }

        // 弧度法表記に変換
        fromAngleRad *= Mathf.Deg2Rad;

        return true;
    }

    /// <summary>
    /// 時限モーションの更新処理
    /// </summary>
    /// <param name="progress">進捗率</param>
    protected override void OnLimitedUpdate(float progress) {
        var rotateAngleRad = rotateAngle;

        // 弧度法表記に変換
        rotateAngleRad *= Mathf.Deg2Rad;

        var fromSin = Mathf.Sin(fromAngleRad);
        var fromCos = Mathf.Cos(fromAngleRad);

        // 現在位置計算
        var curAngleRad = fromAngleRad + progress * rotateAngleRad;

        position = initPosition + new Vector2(
            -fromSin + Mathf.Sin(curAngleRad),
            fromCos - Mathf.Cos(curAngleRad)
        ) * radius;

        curAngle = curAngleRad * Mathf.Rad2Deg;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(fromAngle))
            .Concat(BitConverter.GetBytes(relativeAngle))
            .Concat(BitConverter.GetBytes(rotateAngle))
            .Concat(BitConverter.GetBytes(radius)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        fromAngle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        relativeAngle = BitConverter.ToBoolean(bytes, offset);
        offset += sizeof(bool);
        rotateAngle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        radius = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

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

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        fromAngle = UnityEditor.EditorGUILayout.FloatField("From Angle", fromAngle);
        relativeAngle = UnityEditor.EditorGUILayout.Toggle("Relative Angle", relativeAngle);
        rotateAngle = UnityEditor.EditorGUILayout.FloatField("Rotate Angle", rotateAngle);
        radius = UnityEditor.EditorGUILayout.FloatField("Radius", radius);
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    /// <param name="from">現在位置</param>
    /// <returns>移動後の位置</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        // 円の頂点数
        const int POINT_NUM = 45;

        // 角度情報の初期化
        if ( !Application.isPlaying ) {
            if ( relativeAngle ) {
                fromAngleRad = (fromAngle + transform.localEulerAngles.z);
            } else {
                fromAngleRad = fromAngle;
            }
            fromAngleRad *= Mathf.Deg2Rad;
        }

        var isRight = rotateAngle < 0;
        if ( isRight ) {
            // 右旋回の場合
            fromAngleRad -= Mathf.PI;
        }

        var fromSin = Mathf.Sin(fromAngleRad);
        var fromCos = Mathf.Cos(fromAngleRad);
        var rotateAngleRad = rotateAngle * Mathf.Deg2Rad;
        var toAngleRad = fromAngleRad + rotateAngleRad;

        if ( isRight ) {
            var tmp = toAngleRad;
            toAngleRad = fromAngleRad;
            fromAngleRad = tmp;
        }

        // 軌跡データの作成
        var points = new System.Collections.Generic.List<Vector2>();
        for ( int i = 0 ; i < POINT_NUM + 1 ; ++i ) {
            // 円運動時の角度計算
            var angle = 2 * Mathf.PI * i / POINT_NUM + fromAngleRad;

            // 円の端まで到達したら、端の位置を調整する
            bool isEnd = false;
            if ( angle > toAngleRad ) {
                angle = toAngleRad;
                isEnd = true;
            }

            // 頂点データ追加
            points.Add(from + new Vector2(-fromSin + Mathf.Sin(angle), fromCos - Mathf.Cos(angle)) * radius);

            // 端まで到達したらbreak
            if ( isEnd ) {
                break;
            }
        }

        // 線の描画
        DrawLine(points.ToArray());

        // 矢印の描画
        Vector2 to;
        if ( isRight ) {
            to = from + new Vector2(-fromSin + Mathf.Sin(fromAngleRad), fromCos - Mathf.Cos(fromAngleRad)) * radius;
            DrawArrowCap(to, fromAngleRad * Mathf.Rad2Deg + 180);
        } else {
            to = from + new Vector2(-fromSin + Mathf.Sin(toAngleRad), fromCos - Mathf.Cos(toAngleRad)) * radius;
            DrawArrowCap(to, toAngleRad * Mathf.Rad2Deg);
        }

        return to;
    }

    /// <summary>
    /// 速さ取得
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <returns>設定された速さ</returns>
    public override float GetSpeed(Vector2 from) {
        var length = radius * rotateAngle * Mathf.Deg2Rad;
        var curSpeed = 0f;
        if ( duration != 0 ) {
            curSpeed = length / duration;
        }
        return curSpeed;
    }

    /// <summary>
    /// 速さ設定
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <param name="speed">速さ</param>
    public override void SetSpeed(Vector2 from, float speed) {
        if ( speed == 0 ) {
            duration = 0;
        } else {
            var length = radius * rotateAngle * Mathf.Deg2Rad;
            duration = length / speed;
        }
    }
#endif
}
