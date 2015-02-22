///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   Acc2Func.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.22                                                                       //
//  Desc    :   加速関数(Acc2Funcの拡張版)。                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 加速関数(Acc2Funcの拡張版)。
/// </summary>
public class Acc2Func : ProgFuncBase {
    /// <summary>
    /// 加速期間1(0～0.5)
    /// </summary>
    private float accSpan1;

    /// <summary>
    /// 加速期間2(0～0.5)
    /// </summary>
    private float accSpan2;

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
        if ( accSpan1 == 0 && accSpan2 == 0 ) {
            return progress;
        } else if ( progress <= accSpan1 ) {
            if ( accSpan1 == 0 ) {
                return progress;
            }
            progress = progress * progress / accSpan1 / 2;
        } else if ( progress <= 1 - accSpan2 ) {
            progress = progress - accSpan1 / 2;
        } else {
            if ( accSpan2 == 0 ) {
                return progress;
            }
            progress = progress * (1 - progress / 2) + (accSpan2 * accSpan2 - 1) / 2;
            progress /= accSpan2;
            progress += 1 - 0.5f * accSpan1 - accSpan2;
        }

        return progress * maxSpeed;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(accSpan1))
            .Concat(BitConverter.GetBytes(accSpan2)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        accSpan1 = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        accSpan2 = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        maxSpeed = 2 / (2 - accSpan1 - accSpan2);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        accSpan1 = UnityEditor.EditorGUILayout.Slider("Accelerate Span1", accSpan1, 0, 0.5f);
        accSpan2 = UnityEditor.EditorGUILayout.Slider("Accelerate Span2", accSpan2, 0, 0.5f);
    }
#endif
}
