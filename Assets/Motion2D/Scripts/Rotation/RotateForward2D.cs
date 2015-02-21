///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   RotateForward2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   前方に回転する動作。                                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 前方に回転する動作。
/// </summary>
public class RotateForward2D : RotationBase2D {
    /// <summary>
    /// 回転角速度
    /// </summary>
    private float speed;

    /// <summary>
    /// 向きのずらし角度
    /// </summary>
    private float angleOffset;

    /// <summary>
    /// 回転動作の初期化処理
    /// </summary>
    /// <returns>true:回転動作継続 / false:以降の回転動作を継続しない</returns>
    protected override bool OnStart() {
        return true;
    }

    /// <summary>
    /// 回転動作の更新処理
    /// </summary>
    /// <returns>true:回転動作継続 / false:以降の回転動作を継続しない</returns>
    protected override bool OnUpdate() {
        // 現在の向き取得
        var toAngle = motion.direction;

        if ( toAngle == MotionBase2D.NO_DIRECTION ) {
            // 向きが存在しなければ何もしない
            return true;
        }

        // ずらし角度加算
        toAngle += angleOffset;

        // 回転量計算
        float diffAngle = AdjustAngleRange(toAngle - angle, -180);
        var rotAngle = speed * Time.deltaTime;

        // 向き更新
        if ( rotAngle > Mathf.Abs(diffAngle) ) {
            // 目標角度を超えて回転する場合は目標角度に一致
            angle = toAngle;
        } else {
            // 目標角度を超えない場合はその方向に回転
            angle += diffAngle < 0 ? -rotAngle : rotAngle;
        }

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
            .Concat(BitConverter.GetBytes(angleOffset)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済み回転動作データ</param>
    /// <param name="offset">回転動作データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        angleOffset = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();

        speed = UnityEditor.EditorGUILayout.FloatField("Speed", speed);
        angleOffset = UnityEditor.EditorGUILayout.FloatField("Angle Offset", angleOffset);
    }
#endif
}
