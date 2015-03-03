///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionLoop2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.03                                                                       //
//  Desc    :   複数のモーションの繰り返し。                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 複数のモーションの繰り返し。
/// </summary>
public class MotionLoop2D : MotionSequence2D {
    /// <summary>
    /// モーションの更新処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnUpdate() {
        // モーションの状態更新
        var nextUpdate = motions[current].UpdateMotion(false);
        position = motions[current].position;

        // 向きの更新
        curAngle = motions[current].direction;

        if ( !nextUpdate ) {
            // 次のモーションに遷移
            var prev = current;
            if ( ++current >= motions.Length ) {
                // 末尾のモーションを実行したら先頭に戻る
                current = 0;
            }

            if ( onChange != null ) {
                // モーション変更イベント実行
                onChange(current);
            }

            // 次のモーション初期化
            transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
            motions[current].InitMotion(transform, motions[prev].currentDirection);
            motions[current].StartMotion();

            // 向きの更新
            curAngle = motions[current].direction;
        }

        return true;
    }
}
