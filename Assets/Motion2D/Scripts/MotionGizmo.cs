///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionGizmo.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   Motion�֘A��Gizmo�\�����܂Ƃ߂��N���X�B                                          //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Motion�֘A��Gizmo�\�����܂Ƃ߂��N���X�B
/// </summary>
public class MotionGizmo {
    public static Color EditorColor = Color.cyan;
    public static Color MovingColor = Color.magenta;
    public static Color DisableColor = Color.gray;

    /// <summary>
    /// ���̕`��
    /// </summary>
    /// <param name="points"></param>
    /// <param name="color"></param>
    /// <param name="showStartCap"></param>
    /// <param name="showEndCap"></param>
    public static void DrawArrow(Vector2[] points, Color color, bool showStartCap = false, bool showEndCap = true) {
        if ( points.Length < 2 ) {
            // �������݂��Ȃ���Ή������Ȃ�
            return;
        }

        // ���̖�̊p�x�v�Z
        float startCapAngle = 0;
        float endCapAngle = 0;

        if ( showStartCap ) {
            var dir = points[0] - points[1];
            startCapAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        if ( showEndCap ) {
            var dir = points[points.Length - 1] - points[points.Length - 2];
            endCapAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        // ���̖�̕`��
        DrawArrow(points, color, showStartCap, showEndCap, startCapAngle, endCapAngle);
    }

    /// <summary>
    /// ���̕`��
    /// </summary>
    /// <param name="points"></param>
    /// <param name="color"></param>
    /// <param name="showStartCap"></param>
    /// <param name="showEndCap"></param>
    /// <param name="startCapAngle"></param>
    /// <param name="endCapAngle"></param>
    public static void DrawArrow(Vector2[] points, Color color, bool showStartCap, bool showEndCap, float startCapAngle, float endCapAngle) {
        if ( points.Length < 2 ) {
            // �������݂��Ȃ���Ή������Ȃ�
            return;
        }

        // �n�_�̐ݒ�
        var from = points[0];

        // ���̐F�ݒ�
        Gizmos.color = color;

        // ���̕`��
        foreach ( var point in points ) {
            Gizmos.DrawLine(from, point);
            from = point;
        }

        // ���̖�̕`��
        if ( showStartCap ) {
            DrawArrowCap(points[0], startCapAngle, color);
        }
        if ( showEndCap ) {
            DrawArrowCap(from, endCapAngle, color);
        }
    }

    /// <summary>
    /// ���̖�̕�����`�悷��
    /// </summary>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    /// <param name="color"></param>
    public static void DrawArrowCap(Vector2 from, float angle, Color color) {
        var scale = CameraScale;

        // ���b�V���ݒ�
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[3] {
            new Vector3( 0,-10) * scale,
            new Vector3( 0, 10) * scale,
            new Vector3(20,  0) * scale,
        };

        mesh.triangles = new int[] {
            0, 1, 2
        };

        mesh.normals = new Vector3[] {
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
        };

        mesh.colors = new Color[] {
            color,
            color,
            color,
        };

        // ���b�V���`��
        Graphics.DrawMeshNow(mesh, from, Quaternion.Euler(0, 0, angle));

        Object.DestroyImmediate(mesh);
    }

    public static float CameraScale {
        get {
            var sceneCamera = SceneView.lastActiveSceneView.camera;
            return sceneCamera.orthographicSize / sceneCamera.pixelHeight;
        }
    }
}

#endif