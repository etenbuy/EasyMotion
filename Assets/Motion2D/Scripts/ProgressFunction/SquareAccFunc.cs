///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SquareAccFunc.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   ���ɓʂ̓񎟊֐��B                                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// ���ɓʂ̓񎟊֐��B
/// </summary>
public class SquareAccFunc : ProgFuncBase {
    /// <summary>
    /// �i���x�����擾����
    /// </summary>
    /// <param name="progress">���͒l</param>
    /// <returns>�o�͒l</returns>
    public override float GetProgress(float progress) {
        return progress * progress;
    }
}
