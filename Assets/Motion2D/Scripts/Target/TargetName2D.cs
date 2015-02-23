///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   TargetName2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.23                                                                       //
//  Desc    :   名前指定の目標物情報クラス。                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 名前指定の目標物情報クラス。
/// </summary>
public class TargetName2D : TargetBase2D {
    /// <summary>
    /// 目標物の名前
    /// </summary>
    private string name = string.Empty;

    /// <summary>
    /// 目標物のタグ名
    /// </summary>
    private string tag = string.Empty;

    /// <summary>
    /// 目標物のTransform参照
    /// </summary>
    private Transform targetTrans;

    /// <summary>
    /// 目標物のTransform
    /// </summary>
    public override Transform transform {
        get {
            if ( targetTrans == null && !string.IsNullOrEmpty(name) ) {
                // 名前で目標物探索
                var obj = GameObject.Find(name);
                if ( obj != null ) {
                    targetTrans = obj.transform;
                }
            }

            if ( targetTrans == null && !string.IsNullOrEmpty(tag) ) {
                // タグ名で目標物探索
                var obj = GameObject.FindGameObjectWithTag(tag);
                if ( obj != null ) {
                    targetTrans = obj.transform;
                }
            }

            return targetTrans;
        }
    }

    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        var nameBytes = System.Text.Encoding.ASCII.GetBytes(name);
        var tagBytes = System.Text.Encoding.ASCII.GetBytes(tag);

        return result
            .Concat(BitConverter.GetBytes(nameBytes.Length))
            .Concat(nameBytes)
            .Concat(BitConverter.GetBytes(tagBytes.Length))
            .Concat(tagBytes).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        var length = BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        name = System.Text.Encoding.ASCII.GetString(bytes, offset, length);
        offset += length;

        length = BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        tag = System.Text.Encoding.ASCII.GetString(bytes, offset, length);
        offset += length;

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        name = UnityEditor.EditorGUILayout.TextField("Name", name);
        tag = UnityEditor.EditorGUILayout.TagField("Tag", tag);
    }
#endif
}
