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

        ShowRotateGUI(obj);

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
    /// <param name="property"></param>
    public static void OnInspectorGUI(SerializedProperty property) {
        var typeProp = property.FindPropertyRelative("type");
        var type = (SerializedMotion2D.MotionType)typeProp.enumValueIndex;

        // モーション共通のGUI表示
        EditorGUILayout.PropertyField(property.FindPropertyRelative("type"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("delay"));
        ShowRotateGUI(property);

        var objType = SerializedMotion2D.GetType(type);
        if ( !objType.IsSubclassOf(typeof(EternalMotion2D)) ) {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("duration"));
        }

        var fromCurrent = property.FindPropertyRelative("fromCurrent");
        EditorGUILayout.PropertyField(fromCurrent);


        if ( !objType.IsSubclassOf(typeof(EternalMotion2D)) ) {
            EditorGUI.BeginDisabledGroup(!fromCurrent.boolValue);
            var relative = property.FindPropertyRelative("relative");
            if ( !fromCurrent.boolValue ) {
                relative.boolValue = false;
            }
            EditorGUILayout.PropertyField(relative);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("from"));
            EditorGUI.EndDisabledGroup();
        }

        // モーション個別のGUI表示
        switch ( type ) {
        case SerializedMotion2D.MotionType.MoveTo:
            // MoveTo
            EditorGUILayout.PropertyField(property.FindPropertyRelative("to"));
            break;

        case SerializedMotion2D.MotionType.MoveArc:
            // MoveArc
            EditorGUILayout.PropertyField(property.FindPropertyRelative("fromAngle"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("rotateAngle"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("radius"));
            break;

        case SerializedMotion2D.MotionType.Liner:
            // LinerMotion
            EditorGUILayout.PropertyField(property.FindPropertyRelative("velocity"));
            break;
        }
    }

    /// <summary>
    /// 回転設定GUI
    /// </summary>
    /// <param name="obj"></param>
    public static void ShowRotateGUI(SerializedObject obj) {
        var rotateType = obj.FindProperty("rotateType");
        EditorGUILayout.PropertyField(rotateType);
        EditorGUILayout.PropertyField(obj.FindProperty("rotateOffset"));

        switch ( (SerializedMotion2D.RotateType)rotateType.enumValueIndex ) {
        case SerializedMotion2D.RotateType.Forward:
        case SerializedMotion2D.RotateType.To:
            var rotateImmediate = obj.FindProperty("rotateImmediate");
            EditorGUILayout.PropertyField(rotateImmediate);

            EditorGUI.BeginDisabledGroup(rotateImmediate.boolValue);
            EditorGUILayout.PropertyField(obj.FindProperty("rotateSpeed"));
            EditorGUI.EndDisabledGroup();

            break;

        }
    }

    /// <summary>
    /// 回転設定GUI
    /// </summary>
    /// <param name="property"></param>
    public static void ShowRotateGUI(SerializedProperty property) {
        var rotateType = property.FindPropertyRelative("rotateType");
        EditorGUILayout.PropertyField(rotateType);
        EditorGUILayout.PropertyField(property.FindPropertyRelative("rotateOffset"));

        switch ( (SerializedMotion2D.RotateType)rotateType.enumValueIndex ) {
        case SerializedMotion2D.RotateType.Forward:
        case SerializedMotion2D.RotateType.To:
            var rotateImmediate = property.FindPropertyRelative("rotateImmediate");
            EditorGUILayout.PropertyField(rotateImmediate);

            EditorGUI.BeginDisabledGroup(rotateImmediate.boolValue);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("rotateSpeed"));
            EditorGUI.EndDisabledGroup();

            break;

        }
    }

}
