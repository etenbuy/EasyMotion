///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SquareAccFunc.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   下に凸の二次関数。                                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 下に凸の二次関数。
/// </summary>
public class SquareAccFunc : ProgFuncBase {
    /// <summary>
    /// 進捗度合を取得する
    /// </summary>
    /// <param name="progress">入力値</param>
    /// <returns>出力値</returns>
    public override float GetProgress(float progress) {
        return progress * progress;
    }
}
