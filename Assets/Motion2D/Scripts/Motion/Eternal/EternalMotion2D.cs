///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EternalMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   �i�v���[�V�����B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �i�v���[�V�����B
/// </summary>
public class EternalMotion2D : MotionBase2D {
    /// <summary>
    /// ���Ԋ֐��̎��
    /// </summary>
    private TimeFuncBase.FuncType timeFuncType = TimeFuncBase.FuncType.None;

    /// <summary>
    /// ���Ԋ֐�
    /// </summary>
    private TimeFuncBase timeFunc;

    /// <summary>
    /// �J�n����
    /// </summary>
    private float startTime;

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnStart() {
        startTime = Time.time;
        OnEternalStart();
        return true;
    }

    /// <summary>
    /// ���[�V�����̍X�V����
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnUpdate() {
        // �i�v���[�V�����X�V
        return OnEternalUpdate(Time.time - startTime);
    }

    /// <summary>
    /// �i�v���[�V�����̏���������(�h���N���X�Ŏ�������)
    /// </summary>
    protected virtual void OnEternalStart() {
    }

    /// <summary>
    /// �i�v���[�V�����̍X�V����(�h���N���X�Ŏ�������)
    /// </summary>
    /// <param name="time">�o�ߎ���</param>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected virtual bool OnEternalUpdate(float time) {
        return false;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        if ( timeFunc == null ) {
            timeFunc = TimeFuncBase.CreateInstance(timeFuncType);
        }

        return result
            .Concat(BitConverter.GetBytes((int)timeFuncType))
            .Concat(timeFunc.Serialize()).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        timeFuncType = (TimeFuncBase.FuncType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        timeFunc = TimeFuncBase.GetDeserialized(timeFuncType, bytes, offset, out offset);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        var prevType = timeFuncType;
        timeFuncType = (TimeFuncBase.FuncType)UnityEditor.EditorGUILayout.EnumPopup("Function", timeFuncType);

        if ( timeFuncType != prevType || timeFunc == null ) {
            // �^���ύX���ꂽ
            timeFunc = TimeFuncBase.CreateInstance(timeFuncType);
        }

        timeFunc.DrawGUI();
    }
#endif
}
