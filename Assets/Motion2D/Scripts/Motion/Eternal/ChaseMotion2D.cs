///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   ChaseMotion2D.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.23                                                                       //
//  Desc    :   目標物に追尾する動作。                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 目標物に追尾する動作。
/// </summary>
public class ChaseMotion2D : EternalMotion2D {
    /// <summary>
    /// 初期の向き
    /// </summary>
    private float fromAngle;

    /// <summary>
    /// 前進する速さ
    /// </summary>
    private float speed;

    /// <summary>
    /// 旋回速度
    /// </summary>
    private float rotateSpeed;

    /// <summary>
    /// 目標物オブジェクトの種類
    /// </summary>
    private TargetBase2D.TargetType targetType;

    /// <summary>
    /// 追尾対象の目標物
    /// </summary>
    private TargetBase2D target;

    /// <summary>
    /// 現在の向き
    /// </summary>
    private float curAngle;

    /// <summary>
    /// 永久モーションの初期化処理
    /// </summary>
    protected override void OnEternalStart() {
        curAngle = fromAngle;
    }

    /// <summary>
    /// 永久モーションの更新処理
    /// </summary>
    /// <param name="time">経過時間</param>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnEternalUpdate(float time) {
        // 目標物のTransform取得
        var targetTrans = target.transform;
        if ( targetTrans == null ) {
            // 目標物が存在しない
            return true;
        }

        // 目標物への向き計算
        Vector2 targetDir = targetTrans.localPosition - transform.localPosition;
        var toAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        // 回転量計算
        float diffAngle = RotationBase2D.AdjustAngleRange(toAngle - curAngle, -180);
        var rotAngle = speed * Time.deltaTime;

        // 向き更新
        if ( rotAngle > Mathf.Abs(diffAngle) ) {
            // 目標角度を超えて回転する場合は目標角度に一致
            curAngle = toAngle;
        } else {
            // 目標角度を超えない場合はその方向に回転
            curAngle += diffAngle < 0 ? -rotAngle : rotAngle;
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
            .Concat(BitConverter.GetBytes(fromAngle))
            .Concat(BitConverter.GetBytes(speed))
            .Concat(BitConverter.GetBytes(rotateSpeed))
            .Concat(BitConverter.GetBytes((int)targetType))
            .Concat(target.Serialize()).ToArray();
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
        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        rotateSpeed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        targetType = (TargetBase2D.TargetType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        target = TargetBase2D.GetDeserialized(targetType, bytes, offset, out offset);

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
}
