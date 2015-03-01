///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   Direction2D.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.28                                                                       //
//  Desc    :   向き情報。                                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 向き情報。
/// </summary>
public class Direction2D {
    /// <summary>
    /// 向きの表現方法
    /// </summary>
    enum Type {
        None,                   // 絶対角度
        MotionRelative,         // モーションの向きとの相対角度
        TransformRelative,      // Transformの向きとの相対角度
    };

    /// <summary>
    /// 向きの表現方法
    /// </summary>
    private Type type = Type.None;

    /// <summary>
    /// 角度
    /// </summary>
    private float angle;

    /// <summary>
    /// 管理元のモーションオブジェクト
    /// </summary>
    private MotionBase2D motion;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="motion">管理元のモーション</param>
    public Direction2D(MotionBase2D motion) {
        this.motion = motion;
    }

    /// <summary>
    /// 向き取得
    /// </summary>
    public float direction {
        get {
            switch ( type ) {
            case Type.None:
                // 絶対角度
                return angle;

            case Type.MotionRelative:
                // モーションの向きとの相対角度
                var direction = motion.initDirection;

                if ( direction == MotionBase2D.NO_DIRECTION ) {
                    return angle + motion.transform.localEulerAngles.z;
                } else {
                    return angle + direction;
                }

            case Type.TransformRelative:
                // Transformの向きとの相対角度
                return angle + motion.transform.localEulerAngles.z;

            default:
                // 異常値
                return 0;
            }
        }
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public byte[] Serialize() {
        return 
            BitConverter.GetBytes((int)type)
            .Concat(BitConverter.GetBytes(angle)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public int Deserialize(byte[] bytes, int offset) {
        type = (Type)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        angle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public void DrawGUI() {
        UnityEditor.EditorGUILayout.LabelField("Direction");
        ++UnityEditor.EditorGUI.indentLevel;
        type = (Type)UnityEditor.EditorGUILayout.EnumPopup("Type", type);
        angle = UnityEditor.EditorGUILayout.FloatField("Angle", angle);
        --UnityEditor.EditorGUI.indentLevel;
    }
#endif
}
