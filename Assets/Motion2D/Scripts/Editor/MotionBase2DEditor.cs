///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionBase2DEditor.cs                                                            //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   MotionBase2Dクラスのエディタ拡張。                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using System.Collections;

/// <summary>
/// MotionBase2Dクラスのエディタ拡張。
/// </summary>
public class MotionBase2DEditor : Editor {
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    /// <param name="motion">モーションオブジェクト</param>
    public static void DrawGUI(MotionBase2D motion) {
        EditorGUILayout.FloatField("Delay", motion.delay);
    }
}
