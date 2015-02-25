///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   ChaseMotion2D.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.23                                                                       //
//  Desc    :   目標物に追尾する動作。                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 目標物に追尾する動作。
/// </summary>
public class ChaseMotion2D : EternalMotion2D {
    /// <summary>
    /// 初期の向き
    /// </summary>
    private float fromAngle;

    /// <summary>
    /// 初期の向きは現在の向きの相対値とするかどうか
    /// </summary>
    private bool relativeAngle;

    /// <summary>
    /// 前進する速さ
    /// </summary>
    private float speed;

    /// <summary>
    /// 旋回成分の時間関数の種類
    /// </summary>
    private TimeFuncBase.FuncType rotateTimeFuncType = TimeFuncBase.FuncType.None;

    /// <summary>
    /// 旋回成分の時間関数
    /// </summary>
    private TimeFuncBase rotateTimeFunc;

    /// <summary>
    /// 旋回速度
    /// </summary>
    private float rotateSpeed;

    /// <summary>
    /// 目標物オブジェクトの種類
    /// </summary>
    private TargetBase2D.TargetType targetType;

    /// <summary>
    /// 追尾対象の目標物
    /// </summary>
    private TargetBase2D target;

    /// <summary>
    /// 目標物を向いたらモーションを終了するかどうか
    /// </summary>
    private bool lookEnd;

    /// <summary>
    /// 現在の向き
    /// </summary>
    private float curAngle;

    /// <summary>
    /// 1フレーム前の旋回時刻
    /// </summary>
    private float prevRotateTime;

    /// <summary>
    /// 目標物に向いたときに呼ばれるイベント
    /// </summary>
    public MotionEvent onTarget;

    /// <summary>
    /// 永久モーションの初期化処理
    /// </summary>
    protected override void OnEternalStart() {
        curAngle = relativeAngle ? fromAngle + transform.localEulerAngles.z : fromAngle;
    }

    /// <summary>
    /// 永久モーションの更新処理(派生クラスで実装する)
    /// </summary>
    /// <param name="time">モーション開始からの経過時間</param>
    /// <param name="deltaTime">前回フレームからの経過時間</param>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnEternalUpdate(float time, float deltaTime) {
        var result = true;

        // 目標物のTransform取得
        var targetTrans = target.transform;
        if ( targetTrans != null ) {
            // 目標物への向き計算
            var targetPos = targetTrans.position;
            if ( transform.parent != null ) {
                targetPos = transform.parent.InverseTransformPoint(targetPos);
            }
            var targetDir = (Vector2)targetPos - position;
            var toAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

            // 回転量計算
            float diffAngle = RotationBase2D.AdjustAngleRange(toAngle - curAngle, -180);
            var rotateTime = rotateTimeFunc.GetTime(realTime);
            var rotAngle = rotateSpeed * (rotateTime - prevRotateTime);
            prevRotateTime = rotateTime;

            // 向き更新
            if ( rotAngle > Mathf.Abs(diffAngle) ) {
                // 目標角度を超えて回転する場合は目標角度に一致
                curAngle = toAngle;

                if ( onTarget != null ) {
                    // 目標物に向いたイベント実行
                    onTarget();
                }

                if ( lookEnd ) {
                    // モーション終了
                    result = false;
                }

            } else {
                // 目標角度を超えない場合はその方向に回転
                curAngle += diffAngle < 0 ? -rotAngle : rotAngle;
            }
        }

        // 位置更新
        var curAngleRad = curAngle * Mathf.Deg2Rad;
        position += new Vector2(Mathf.Cos(curAngleRad), Mathf.Sin(curAngleRad)) * speed * deltaTime;

        return result;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        if ( rotateTimeFunc == null ) {
            rotateTimeFunc = TimeFuncBase.CreateInstance(rotateTimeFuncType);
        }

        if ( target == null ) {
            target = TargetBase2D.CreateInstance(targetType);
        }

        return result
            .Concat(BitConverter.GetBytes(fromAngle))
            .Concat(BitConverter.GetBytes(relativeAngle))
            .Concat(BitConverter.GetBytes(speed))
            .Concat(BitConverter.GetBytes((int)rotateTimeFuncType))
            .Concat(rotateTimeFunc.Serialize())
            .Concat(BitConverter.GetBytes(rotateSpeed))
            .Concat(BitConverter.GetBytes((int)targetType))
            .Concat(target.Serialize())
            .Concat(BitConverter.GetBytes(lookEnd)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        fromAngle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        relativeAngle = BitConverter.ToBoolean(bytes, offset);
        offset += sizeof(bool);
        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        rotateTimeFuncType = (TimeFuncBase.FuncType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        rotateTimeFunc = TimeFuncBase.GetDeserialized(rotateTimeFuncType, bytes, offset, out offset);

        rotateSpeed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        targetType = (TargetBase2D.TargetType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        target = TargetBase2D.GetDeserialized(targetType, bytes, offset, out offset);

        lookEnd = BitConverter.ToBoolean(bytes, offset);
        offset += sizeof(bool);

        return offset;
    }

    /// <summary>
    /// 現在の向き
    /// </summary>
    public override float currentDirection {
        get {
            return curAngle;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        fromAngle = UnityEditor.EditorGUILayout.FloatField("From Angle", fromAngle);
        relativeAngle = UnityEditor.EditorGUILayout.Toggle("Relative Angle", relativeAngle);
        speed = UnityEditor.EditorGUILayout.FloatField("Speed", speed);

        var prevTimeFuncType = rotateTimeFuncType;
        rotateTimeFuncType = (TimeFuncBase.FuncType)UnityEditor.EditorGUILayout.EnumPopup("Rotate Function", rotateTimeFuncType);
        if ( rotateTimeFuncType != prevTimeFuncType || rotateTimeFunc == null ) {
            // 型が変更された
            rotateTimeFunc = TimeFuncBase.CreateInstance(rotateTimeFuncType);
        }
        rotateTimeFunc.DrawGUI();

        rotateSpeed = UnityEditor.EditorGUILayout.FloatField("Rotate Speed", rotateSpeed);

        var prevType = targetType;
        targetType = (TargetBase2D.TargetType)UnityEditor.EditorGUILayout.EnumPopup("Target Type", targetType);
        if ( targetType != prevType || target == null ) {
            // 型が変更された
            target = TargetBase2D.CreateInstance(targetType);
        }
        target.DrawGUI();

        lookEnd = UnityEditor.EditorGUILayout.Toggle("Look End", lookEnd);
    }

    /// <summary>
    /// 速さ取得
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <returns>設定された速さ</returns>
    public override float GetSpeed(Vector2 from) {
        return speed;
    }

    /// <summary>
    /// 速さ設定
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <param name="speed">速さ</param>
    public override void SetSpeed(Vector2 from, float speed) {
        this.speed = speed;
    }
#endif
}
