///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionSequence2D.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   ���[�V�����̘A������B                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// ���[�V�����̘A������B
/// </summary>
public class MotionSequence2D : MotionBase2D {
    /// <summary>
    /// ����Ώۂ̃��[�V����
    /// </summary>
    private MotionBase2D[] motions;

    /// <summary>
    /// ���ݎ��s���̃��[�V����
    /// </summary>
    private int current;

    /// <summary>
    /// ���݂̌���
    /// </summary>
    private float curAngle;

    /// <summary>
    /// ���[�V�����؂�ւ��C�x���g
    /// </summary>
    /// <param name="motion"></param>
    public delegate void OnChange(int motion);

    /// <summary>
    /// ���[�V�����؂�ւ��C�x���g
    /// </summary>
    public OnChange onChange;

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    protected override void OnInit() {
        base.OnInit();

        foreach ( var motion in motions ) {
            motion.InitMotion(transform);
        }
    }

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnStart() {
        if ( motions.Length == 0 ) {
            return false;
        }

        current = 0;
        motions[0].StartMotion();
        curAngle = motions[0].direction;

        return true;
    }

    /// <summary>
    /// ���[�V�����̍X�V����
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnUpdate() {
        if ( current >= motions.Length ) {
            // ���s���郂�[�V���������݂��Ȃ���ΏI��
            return false;
        }

        // ���[�V�����̏�ԍX�V
        var nextUpdate = motions[current].UpdateMotion(false);
        position = motions[current].position;

        // �����̍X�V
        curAngle = motions[current].direction;

        if ( !nextUpdate ) {
            // ���̃��[�V�����ɑJ��
            if ( ++current >= motions.Length ) {
                // ���s���郂�[�V���������݂��Ȃ���ΏI��
                return false;
            }

            if ( onChange != null ) {
                // ���[�V�����ύX�C�x���g���s
                onChange(current);
            }

            // ���̃��[�V����������
            transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
            motions[current].InitMotion(transform, motions[current - 1].direction);
            motions[current].StartMotion();

            // �����̍X�V
            curAngle = motions[current].direction;
        }

        return true;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        if ( motions == null ) {
            return result.Concat(BitConverter.GetBytes((int)0)).ToArray();
        }

        // ���[�V������
        result = result.Concat(BitConverter.GetBytes(motions.Length)).ToArray();

        // ���[�V�����f�[�^
        for ( int i = 0 ; i < motions.Length ; ++i ) {
            var type = EasyMotion2D.GetSerializedType(motions[i].GetType());
            result = result
                .Concat(BitConverter.GetBytes((int)type))
                .Concat(motions[i].Serialize()).ToArray();
        }

        return result;
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        // ���[�V������
        var motionNum = BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);

        // ���[�V�����f�[�^
        motions = new MotionBase2D[motionNum];
        for ( int i = 0 ; i < motionNum ; ++i ) {
            var type = (EasyMotion2D.MotionType)BitConverter.ToInt32(bytes, offset);
            offset += sizeof(int);
            motions[i] = EasyMotion2D.GetDeserializedMotion(type, bytes, offset, out offset);
        }

        return offset;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    public override float currentDirection {
        get {
            return curAngle;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();

        if ( motions == null ) {
            return;
        }

        for ( int i = 0 ; i < motions.Length ; ++i ) {
            // �{�^���\��
            GUILayout.BeginHorizontal();

            if ( GUILayout.Button("Up", GUILayout.Width(60)) ) {
                OnUp(i);
            }
            if ( GUILayout.Button("Down", GUILayout.Width(60)) ) {
                OnDown(i);
            }
            if ( GUILayout.Button("Insert New", GUILayout.Width(80)) ) {
                OnInsertNew(i);
            }
            if ( GUILayout.Button("Remove", GUILayout.Width(80)) ) {
                OnRemove(i--);
                GUILayout.EndHorizontal();
                continue;
            }

            GUILayout.EndHorizontal();

            // �e���[�V������GUI�`��
            ++UnityEditor.EditorGUI.indentLevel;

            var currentType = EasyMotion2D.GetSerializedType(motions[i].GetType());
            var newType = (EasyMotion2D.MotionType)UnityEditor.EditorGUILayout.EnumPopup("Motion Type", currentType);
            if ( newType == EasyMotion2D.MotionType.Sequence ) {
                newType = currentType;
            }

            if ( newType != currentType ) {
                motions[i] = EasyMotion2D.CreateInstance(newType);
            }
            motions[i].DrawGUI();

            --UnityEditor.EditorGUI.indentLevel;
        }

        // �����ւ̐V�K�ǉ��{�^���\��
        if ( GUILayout.Button("Add New", GUILayout.Width(80)) ) {
            OnInsertNew(motions.Length);
        }
    }

    /// <summary>
    /// ��ړ��{�^�����N���b�N���ꂽ
    /// </summary>
    /// <param name="index">���[�V�����̃C���f�b�N�X</param>
    private void OnUp(int index) {
        if ( index == 0 ) {
            return;
        }

        Replace(index, index - 1);
    }

    /// <summary>
    /// ���ړ��{�^�����N���b�N���ꂽ
    /// </summary>
    /// <param name="index">���[�V�����̃C���f�b�N�X</param>
    private void OnDown(int index) {
        if ( index >= motions.Length - 1 ) {
            return;
        }

        Replace(index, index + 1);
    }

    /// <summary>
    /// �}���{�^�����N���b�N���ꂽ
    /// </summary>
    /// <param name="index">���[�V�����̃C���f�b�N�X</param>
    private void OnInsertNew(int index) {
        var newMotions = new List<MotionBase2D>(motions);
        newMotions.Insert(index, EasyMotion2D.CreateInstance(EasyMotion2D.MotionType.Stop));
        motions = newMotions.ToArray();
    }

    /// <summary>
    /// �폜�{�^�����N���b�N���ꂽ
    /// </summary>
    /// <param name="index">���[�V�����̃C���f�b�N�X</param>
    private void OnRemove(int index) {
        var newMotions = new List<MotionBase2D>(motions);
        newMotions.RemoveAt(index);
        motions = newMotions.ToArray();
    }

    /// <summary>
    /// ���[�V���������ւ���
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    private void Replace(int index1, int index2) {
        var tmp = motions[index1];
        motions[index1] = motions[index2];
        motions[index2] = tmp;
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        if ( motions == null ) {
            return initPosition;
        }

        // �e���[�V������Gizmo��`��
        foreach ( var motion in motions ) {
            // Gizmo�̕`��
            from = motion.DrawGizmos(transform, from);
        }

        return from;
    }

    /// <summary>
    /// �����擾
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <returns>�ݒ肳�ꂽ����</returns>
    public override float GetSpeed(Vector2 from) {
        if ( motions.Length == 0 ) {
            return 0;
        }

        float avgSpeed = 0;

        drawGizmos = false;
        foreach ( var motion in motions ) {
            var to = motion.DrawGizmos(from);
            avgSpeed += motion.GetSpeed(from);
            from = to;
        }
        drawGizmos = true;

        return avgSpeed / motions.Length;
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public override void SetSpeed(Vector2 from, float speed) {
        drawGizmos = false;
        foreach ( var motion in motions ) {
            var to = motion.DrawGizmos(from);
            motion.SetSpeed(from, speed);
            from = to;
        }
        drawGizmos = true;
    }
#endif
}
