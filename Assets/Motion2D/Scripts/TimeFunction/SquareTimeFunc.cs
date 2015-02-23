///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SquareTimeFunc.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.24                                                                       //
//  Desc    :   二次時間関数。                                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 二次時間関数。
/// </summary>
public class SquareTimeFunc : TimeFuncBase {
    /// <summary>
    /// 係数
    /// </summary>
    private float coefficient = 1;

    /// <summary>
    /// 時間を取得する
    /// </summary>
    /// <param name="time">入力値</param>
    /// <returns>出力値</returns>
    public override float GetTime(float time) {
        return time * time * coefficient;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(coefficient)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        coefficient = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        coefficient = UnityEditor.EditorGUILayout.FloatField("Coefficient", coefficient);
    }
#endif
}
