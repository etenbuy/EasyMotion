///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveTo2DEditor.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   MoveTo2Dクラスのエディタ拡張。                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using System.Collections;

/// <summary>
/// MoveTo2Dクラスのエディタ拡張。
/// </summary>
public class MoveTo2DEditor : Editor {
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    /// <param name="editor">呼び出し元のエディタスクリプト</param>
    /// <param name="motionBase">モーションオブジェクト</param>
    public static void DrawGUI(MotionBase2D motionBase) {
        var motion = motionBase as MoveTo2D;
        LimitedMotion2DEditor.DrawGUI(motion);
        motion.to = EditorGUILayout.Vector2Field("To", motion.to);
    }
}
