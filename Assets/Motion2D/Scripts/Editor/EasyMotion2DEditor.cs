///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EasyMotion2DEditor.cs                                                            //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   EasyMotion2Dクラスのエディタ拡張。                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using System.Collections;

/// <summary>
/// EasyMotion2Dクラスのエディタ拡張。
/// </summary>
//[CustomEditor(typeof(EasyMotion2D))]
public class EasyMotion2DEditor : Editor {
    private int currentType;

    /// <summary>
    /// 初期化。
    /// </summary>
    public void Awake() {
        currentType = serializedObject.FindProperty("type").enumValueIndex;
    }

    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // モーション型選択描画
        var type = serializedObject.FindProperty("type");
        var newType = type.enumValueIndex;
        EditorGUILayout.PropertyField(type);

        //var script = target as EasyMotion2D;

        //if ( newType != currentType ) {
        //    // モーション型が変更された
        //    script.motion = new MoveTo2D();
            
        //    currentType = newType;
        //}

        //var motion = script.motion;

        //// モーションの各GUI描画
        //MotionBase2DEditor.DrawGUI(motion);

        //if ( motion is LimitedMotion2D ) {
        //    LimitedMotion2DEditor.DrawGUI(motion as LimitedMotion2D);
        //}

        serializedObject.ApplyModifiedProperties();
    }
}
