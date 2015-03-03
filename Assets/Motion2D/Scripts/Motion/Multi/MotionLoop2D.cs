///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionLoop2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.03                                                                       //
//  Desc    :   �����̃��[�V�����̌J��Ԃ��B                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// �����̃��[�V�����̌J��Ԃ��B
/// </summary>
public class MotionLoop2D : MotionSequence2D {
    /// <summary>
    /// ���[�V�����̍X�V����
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnUpdate() {
        // ���[�V�����̏�ԍX�V
        var nextUpdate = motions[current].UpdateMotion(false);
        position = motions[current].position;

        // �����̍X�V
        curAngle = motions[current].direction;

        if ( !nextUpdate ) {
            // ���̃��[�V�����ɑJ��
            var prev = current;
            if ( ++current >= motions.Length ) {
                // �����̃��[�V���������s������擪�ɖ߂�
                current = 0;
            }

            if ( onChange != null ) {
                // ���[�V�����ύX�C�x���g���s
                onChange(current);
            }

            // ���̃��[�V����������
            transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
            motions[current].InitMotion(transform, motions[prev].currentDirection);
            motions[current].StartMotion();

            // �����̍X�V
            curAngle = motions[current].direction;
        }

        return true;
    }
}
