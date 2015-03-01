///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   Direction2D.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.28                                                                       //
//  Desc    :   �������B                                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �������B
/// </summary>
public class Direction2D {
    /// <summary>
    /// �����̕\�����@
    /// </summary>
    enum Type {
        None,                   // ��Ίp�x
        MotionRelative,         // ���[�V�����̌����Ƃ̑��Ίp�x
        TransformRelative,      // Transform�̌����Ƃ̑��Ίp�x
    };

    /// <summary>
    /// �����̕\�����@
    /// </summary>
    private Type type = Type.None;

    /// <summary>
    /// �p�x
    /// </summary>
    private float angle;

    /// <summary>
    /// �Ǘ����̃��[�V�����I�u�W�F�N�g
    /// </summary>
    private MotionBase2D motion;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="motion">�Ǘ����̃��[�V����</param>
    public Direction2D(MotionBase2D motion) {
        this.motion = motion;
    }

    /// <summary>
    /// �����擾
    /// </summary>
    public float direction {
        get {
            switch ( type ) {
            case Type.None:
                // ��Ίp�x
                return angle;

            case Type.MotionRelative:
                // ���[�V�����̌����Ƃ̑��Ίp�x
                var direction = motion.initDirection;

                if ( direction == MotionBase2D.NO_DIRECTION ) {
                    return angle + motion.transform.localEulerAngles.z;
                } else {
                    return angle + direction;
                }

            case Type.TransformRelative:
                // Transform�̌����Ƃ̑��Ίp�x
                return angle + motion.transform.localEulerAngles.z;

            default:
                // �ُ�l
                return 0;
            }
        }
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public byte[] Serialize() {
        return 
            BitConverter.GetBytes((int)type)
            .Concat(BitConverter.GetBytes(angle)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public int Deserialize(byte[] bytes, int offset) {
        type = (Type)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        angle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public void DrawGUI() {
        UnityEditor.EditorGUILayout.LabelField("Direction");
        ++UnityEditor.EditorGUI.indentLevel;
        type = (Type)UnityEditor.EditorGUILayout.EnumPopup("Type", type);
        angle = UnityEditor.EditorGUILayout.FloatField("Angle", angle);
        --UnityEditor.EditorGUI.indentLevel;
    }
#endif
}
