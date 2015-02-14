///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionGizmo.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   Motion�֘A��Gizmo�\�����܂Ƃ߂��N���X�B                                          //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Motion�֘A��Gizmo�\�����܂Ƃ߂��N���X�B
/// </summary>
public class MotionGizmo {
    /// <summary>
    /// �\���F
    /// </summary>
    public static Color color = Color.cyan;

    /// <summary>
    /// ���̕`��
    /// </summary>
    /// <param name="points"></param>
    public static void DrawArrow(Vector2[] points) {
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
        var dir = from - points[points.Length - 2];
        var arrowAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        DrawArrowCap(from, arrowAngle);
    }

    /// <summary>
    /// ���̖�̕�����`�悷��
    /// </summary>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    public static void DrawArrowCap(Vector2 from, float angle) {
        var sceneCamera = SceneView.lastActiveSceneView.camera;
        var scale = sceneCamera.orthographicSize / sceneCamera.pixelHeight;

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

}
