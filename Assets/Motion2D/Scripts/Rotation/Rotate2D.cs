///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   Rotate2D.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   等速回転動作。                                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 等速回転動作。
/// </summary>
public class Rotate2D : RotationBase2D {
    /// <summary>
    /// 回転角速度
    /// </summary>
    public float speed;

    /// <summary>
    /// 初期角度
    /// </summary>
    private float initAngle;

    /// <summary>
    /// 回転動作の初期化処理
    /// </summary>
    /// <returns>true:回転動作継続 / false:以降の回転動作を継続しない</returns>
    protected override bool OnStart() {
        initAngle = angle;
        return true;
    }

    /// <summary>
    /// 回転動作の更新処理
    /// </summary>
    /// <returns>true:回転動作継続 / false:以降の回転動作を継続しない</returns>
    protected override bool OnUpdate() {
        angle = initAngle + speed * (Time.time - startTime);
        return true;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(speed)).ToArray();
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

        return offset;
    }
}
