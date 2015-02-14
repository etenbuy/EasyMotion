///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   CurveMotion.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   ù‰ñƒ‚[ƒVƒ‡ƒ“B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// ù‰ñƒ‚[ƒVƒ‡ƒ“
/// </summary>
public class CurveMotion : MotionBase2D {
    /// <summary>
    /// ‰Šp“x
    /// </summary>
    [SerializeField]
    private float fromAngle = 0;

    /// <summary>
    /// ù‰ñŠp“x
    /// </summary>
    [SerializeField]
    private float rotateAngle = 0;

    /// <summary>
    /// ù‰ñ”¼Œa
    /// </summary>
    [SerializeField]
    private float radius = 0;

    /// <summary>
    /// ˆÚ“®ŠJn‚Ü‚Å‚ÌŠÔ
    /// </summary>
    [SerializeField]
    private float delay = 0;

    /// <summary>
    /// ˆÚ“®ŠÔ
    /// </summary>
    [SerializeField]
    private float duration = 0;

    /// <summary>
    /// ù‰ñˆÚ“®ƒRƒ‹[ƒ`ƒ“‚ğÀs‚·‚é
    /// </summary>
    private void Start() {
        if ( fromCurrent ) {
            from = Position2D;
        }

        StartCoroutine(Curve(from, fromAngle, rotateAngle, radius, delay, duration));
    }

#if UNITY_EDITOR
    /// <summary>
    /// ‹O“¹‚ğ¦‚·–îˆó‚ğ•`‰æ‚·‚é
    /// </summary>
    /// <param name="from"></param>
    /// <param name="fromAngle"></param>
    /// <param name="rotateAngle"></param>
    /// <param name="radius"></param>
    /// <param name="fromCurrent"></param>
    /// <returns></returns>
    public static Vector2 DrawArrow(Vector2 from, float fromAngle, float rotateAngle, float radius, bool fromCurrent) {
        Gizmos.color = Color.cyan;

        // ‰~‚Ì’¸“_”
        const int POINT_NUM = 45;

        // Šp“xî•ñ
        var fromAngleRad = fromAngle * Mathf.Deg2Rad;
        var isRight = rotateAngle < 0;
        if ( isRight ) {
            // ‰Eù‰ñ‚Ìê‡
            fromAngleRad -= Mathf.PI;
        }

        var fromSin = Mathf.Sin(fromAngleRad);
        var fromCos = Mathf.Cos(fromAngleRad);
        var rotateAngleRad = rotateAngle * Mathf.Deg2Rad;
        var toAngleRad = fromAngleRad + rotateAngleRad;

        if ( isRight ) {
            var tmp = toAngleRad;
            toAngleRad = fromAngleRad;
            fromAngleRad = tmp;
        }

        // ‹OÕ‚Ì•`‰æ
        for ( int i = 0 ; i < POINT_NUM ; ++i ) {
            var curAngle = 2 * Mathf.PI * i / POINT_NUM + fromAngleRad;
            var nextAngle = 2 * Mathf.PI * (i + 1) / POINT_NUM + fromAngleRad;

            // •`‰æ”ÍˆÍŠO‚ÌŠp“x‚È‚ç‰½‚à‚µ‚È‚¢
            if ( curAngle > toAngleRad ) {
                continue;
            }

            // ‰~ŒÊ‚Ì’[‚Ì‚¸‚ê•â³
            if ( nextAngle > toAngleRad ) {
                nextAngle = toAngleRad;
            }

            // ‰~ŒÊ‚Ì•`‰æ
            Gizmos.DrawLine(
                from + new Vector2(-fromSin + Mathf.Sin(curAngle), fromCos - Mathf.Cos(curAngle)) * radius,
                from + new Vector2(-fromSin + Mathf.Sin(nextAngle), fromCos - Mathf.Cos(nextAngle)) * radius);
        }

        // –îˆó‚Ì•`‰æ
        Vector2 toPos;
        if ( isRight ) {
            toPos = from + new Vector2(-fromSin + Mathf.Sin(fromAngleRad), fromCos - Mathf.Cos(fromAngleRad)) * radius;
            MotionGizmo.DrawArrowCap(toPos, fromAngleRad * Mathf.Rad2Deg + 180);
        } else {
            toPos = from + new Vector2(-fromSin + Mathf.Sin(toAngleRad), fromCos - Mathf.Cos(toAngleRad)) * radius;
            MotionGizmo.DrawArrowCap(toPos, toAngleRad * Mathf.Rad2Deg);
        }

        return toPos;
    }

    /// <summary>
    /// ‹OÕ‚Ì•`‰æ(Editor—p)
    /// </summary>
    private void OnDrawGizmos() {
        DrawArrow(InitPosition2D, fromAngle, rotateAngle, radius, fromCurrent);
    }
#endif
}
