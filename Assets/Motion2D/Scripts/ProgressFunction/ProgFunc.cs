///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   ProgFunc.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   進捗度合関数の基底クラス。                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 進捗度合関数の基底クラス。
/// </summary>
public class ProgFunc {
    /// <summary>
    /// 進捗度合を取得する
    /// </summary>
    /// <param name="progress">入力値</param>
    /// <returns>出力値</returns>
    public virtual float GetProgress(float progress) {
        return progress;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public virtual byte[] Serialize() {
        return new byte[0];
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public virtual int Deserialize(byte[] bytes, int offset) {
        return offset;
    }
}
