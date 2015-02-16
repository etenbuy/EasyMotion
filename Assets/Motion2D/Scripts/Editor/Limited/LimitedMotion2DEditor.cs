///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LimitedMotion2DEditor.cs                                                         //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   LimitedMotion2Dクラスのエディタ拡張。                                            //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using System.Collections;

/// <summary>
/// LimitedMotion2Dクラスのエディタ拡張。
/// </summary>
public class LimitedMotion2DEditor : Editor {
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    /// <param name="motionBase">モーションオブジェクト</param>
    public static void DrawGUI(MotionBase2D motionBase) {
        var motion = motionBase as LimitedMotion2D;
        MotionBase2DEditor.DrawGUI(motion);
        motion.duration = EditorGUILayout.FloatField("Duration", motion.duration);
    }
}
