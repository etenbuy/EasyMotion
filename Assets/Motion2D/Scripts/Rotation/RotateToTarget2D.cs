///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   RotateToTarget2D.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   目標物に回転する動作。                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 目標物に回転する動作。
/// </summary>
public class RotateToTarget2D : RotationBase2D {
    /// <summary>
    /// 目標物情報
    /// </summary>
    private TargetName2D target;

    /// <summary>
    /// 回転する速さ
    /// </summary>
    private float rotateSpeed;

    /// <summary>
    /// 向きのずらし角度
    /// </summary>
    private float angleOffset;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public RotateToTarget2D() {
        target = new TargetName2D();
    }

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
        // 目標物のTransform取得
        var targetTrans = target.transform;

        if ( targetTrans != null ) {
            // 目標物への向き計算
            var targetPos = targetTrans.position;
            if ( transform.parent != null ) {
                targetPos = transform.parent.InverseTransformPoint(targetPos);
            }
            var targetDir = (Vector2)targetPos - motion.position;
            var toAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            toAngle += angleOffset;

            // 回転量計算
            float diffAngle = RotationBase2D.AdjustAngleRange(toAngle - angle, -180);
            var rotAngle = rotateSpeed * Time.deltaTime;

            // 向き更新
            if ( rotAngle > Mathf.Abs(diffAngle) ) {
                // 目標角度を超えて回転する場合は目標角度に一致
                angle = toAngle;
            } else {
                // 目標角度を超えない場合はその方向に回転
                angle += diffAngle < 0 ? -rotAngle : rotAngle;
            }
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
            .Concat(target.Serialize())
            .Concat(BitConverter.GetBytes(rotateSpeed))
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

        offset = target.Deserialize(bytes, offset);
        rotateSpeed = BitConverter.ToSingle(bytes, offset);
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

        target.DrawGUI();
        rotateSpeed = UnityEditor.EditorGUILayout.FloatField("Rotate Speed", rotateSpeed);
        angleOffset = UnityEditor.EditorGUILayout.FloatField("Angle Offset", angleOffset);
    }
#endif
}
