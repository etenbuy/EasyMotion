///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   AccFunc.cs                                                                       //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   加速関数。                                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 加速関数。
/// </summary>
public class AccFunc : ProgFunc {
    /// <summary>
    /// 加速時間(0～0.5)
    /// </summary>
    private float accTime;

    /// <summary>
    /// 最高速度(内部で自動計算される)
    /// </summary>
    private float maxSpeed;

    /// <summary>
    /// 進捗度合を取得する
    /// </summary>
    /// <param name="progress">入力値</param>
    /// <returns>出力値</returns>
    public override float GetProgress(float progress) {
        if ( progress <= accTime ) {
            progress = maxSpeed / accTime / 2 * progress * progress;
        } else if ( progress < 1 - accTime ) {
            progress = maxSpeed * progress + maxSpeed * accTime / 2;
        } else {
            progress = -progress / accTime * (0.5f * progress + 1) + 1 - 1.5f * accTime;
            progress *= maxSpeed;
        }

        return progress;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(accTime)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        accTime = BitConverter.ToSingle(bytes, offset);
        maxSpeed = 1 / (1 - accTime);
        offset += sizeof(float);

        return offset;
    }
}
