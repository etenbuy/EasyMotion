///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SquareDecFunc.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   上に凸の二次関数。                                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 上に凸の二次関数。
/// </summary>
public class SquareDecFunc : ProgFuncBase {
    /// <summary>
    /// 進捗度合を取得する
    /// </summary>
    /// <param name="progress">入力値</param>
    /// <returns>出力値</returns>
    public override float GetProgress(float progress) {
        var result = progress - 1;
        result = -result * result + 1;
        return result;
    }
}
