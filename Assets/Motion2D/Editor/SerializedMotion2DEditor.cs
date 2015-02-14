///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SerializedMotion2DEditor.cs                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   シリアライズ済みモーションエディタ。                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections;

/// <summary>
/// シリアライズ済みモーションエディタ。
/// </summary>
public class SerializedMotion2DEditor {
    /// <summary>
    /// SerializedMotionのインスペクタ上のレイアウト
    /// </summary>
    /// <param name="obj"></param>
    public static void OnInspectorGUI(SerializedObject obj, SerializedMotion2D.MotionType type) {
        // モーション共通のGUI表示
        EditorGUILayout.PropertyField(obj.FindProperty("delay"));

        if ( SerializedMotion2D.GetType(type).IsSubclassOf(typeof(LimitedMotion2D)) ) {
            EditorGUILayout.PropertyField(obj.FindProperty("duration"));
        }

        var fromCurrent = obj.FindProperty("fromCurrent");
        EditorGUILayout.PropertyField(fromCurrent);

        EditorGUI.BeginDisabledGroup(!fromCurrent.boolValue);
        var relative = obj.FindProperty("relative");
        if ( !fromCurrent.boolValue ) {
            relative.boolValue = false;
        }
        EditorGUILayout.PropertyField(relative);
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(obj.FindProperty("from"));
        EditorGUI.EndDisabledGroup();

        // モーション個別のGUI表示
        switch ( type ) {
        case SerializedMotion2D.MotionType.MoveTo:
            // MoveTo
            EditorGUILayout.PropertyField(obj.FindProperty("to"));
            break;

        case SerializedMotion2D.MotionType.MoveArc:
            // MoveArc
            EditorGUILayout.PropertyField(obj.FindProperty("fromAngle"));
            EditorGUILayout.PropertyField(obj.FindProperty("rotateAngle"));
            EditorGUILayout.PropertyField(obj.FindProperty("radius"));
            break;

        case SerializedMotion2D.MotionType.Liner:
            // LinerMotion
            EditorGUILayout.PropertyField(obj.FindProperty("velocity"));
            break;
        }
    }

    /// <summary>
    /// SerializedMotionのインスペクタ上のレイアウト
    /// </summary>
    /// <param name="propery"></param>
    public static void OnInspectorGUI(SerializedProperty propery) {
        var typeProp = propery.FindPropertyRelative("type");
        var type = (SerializedMotion2D.MotionType)typeProp.enumValueIndex;

        // モーション共通のGUI表示
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("type"));
        EditorGUILayout.PropertyField(propery.FindPropertyRelative("delay"));

        var objType = SerializedMotion2D.GetType(type);
        if ( !objType.IsSubclassOf(typeof(EternalMotion2D)) ) {
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("duration"));
        }

        var fromCurrent = propery.FindPropertyRelative("fromCurrent");
        EditorGUILayout.PropertyField(fromCurrent);


        if ( !objType.IsSubclassOf(typeof(EternalMotion2D)) ) {
            EditorGUI.BeginDisabledGroup(!fromCurrent.boolValue);
            var relative = propery.FindPropertyRelative("relative");
            if ( !fromCurrent.boolValue ) {
                relative.boolValue = false;
            }
            EditorGUILayout.PropertyField(relative);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("from"));
            EditorGUI.EndDisabledGroup();
        }

        // モーション個別のGUI表示
        switch ( type ) {
        case SerializedMotion2D.MotionType.MoveTo:
            // MoveTo
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("to"));
            break;

        case SerializedMotion2D.MotionType.MoveArc:
            // MoveArc
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("fromAngle"));
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("rotateAngle"));
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("radius"));
            break;

        case SerializedMotion2D.MotionType.Liner:
            // LinerMotion
            EditorGUILayout.PropertyField(propery.FindPropertyRelative("velocity"));
            break;
        }
    }
}
