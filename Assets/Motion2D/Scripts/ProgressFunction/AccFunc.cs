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
public class AccFunc : ProgFuncBase {
    /// <summary>
    /// 加速期間(0～0.5)
    /// </summary>
    private float accSpan;

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
        if ( accSpan == 0 ) {
            return progress;
        } else if ( progress <= accSpan ) {
            progress = progress * progress / accSpan / 2;
        } else if ( progress <= 1 - accSpan ) {
            progress = progress - accSpan / 2;
        } else {
            progress = progress * (1 - progress / 2) + (accSpan * accSpan - 1) / 2;
            progress /= accSpan;
            progress += 1 - 1.5f * accSpan;
        }

        return progress * maxSpeed;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(accSpan)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        accSpan = BitConverter.ToSingle(bytes, offset);
        maxSpeed = 1 / (1 - accSpan);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        accSpan = UnityEditor.EditorGUILayout.Slider("Accelerate Span", accSpan, 0, 0.5f);
    }
#endif
}
