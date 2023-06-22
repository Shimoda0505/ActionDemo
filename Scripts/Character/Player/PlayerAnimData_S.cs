using UnityEngine;
using System;


/// <summary>
/// Player�̃A�j���[�V�����Ǘ��\����
/// </summary>
public  struct PlayerAnimData_S
{

    /// <summary>
    /// �A�j���[�V�����p�����[�^�[�ꗗ
    /// </summary>
    public enum AnimBoolMode
    {
        [InspectorName("�Ȃ�")] NULL,
        [InspectorName("�ҋ@")] IDLE,
        [InspectorName("����")] WALK,
        [InspectorName("����")] RUN,
        [InspectorName("�X�L��")] SKILL,
        [InspectorName("���")] ROLL,
        [InspectorName("�X�e�b�v")] STEP,
        [InspectorName("��U��")] ATTACK_WEAK,
        [InspectorName("���U��")] ATTACK_STRONG,
        [InspectorName("�K�[�h")] GUARD,
        [InspectorName("�_���[�W")] DAMAGE,
        [InspectorName("���_�E��")] DOWN_WEAK,
        [InspectorName("��_�E��")] DOWN_STRONG,
        [InspectorName("���S")] DEATH,
    }

    public enum AnimComboMode
    {
        [InspectorName("��R���{")] COMBO_WEAK,
        [InspectorName("���R���{")] COMBO_STRONG,
    }


    public enum AnimatorUpdateModeEnum
    {
        [InspectorName("AnimatePhysics")] ANIMATE_PHYSICS,
        [InspectorName("Normal")] NOMAL,
    }


    //���\�b�h��---------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// BoolAnimation�̕ύX
    /// </summary>
    public void ChangeBoolAnimation(Animator anim, AnimBoolMode animBoolMode )
    {

        //AnimMode��enum��ނ��J�E���g
        int enumCount = Enum.GetValues(typeof(AnimBoolMode)).Length;

        //AnimMode�̎�ޕ�����
        for (int i = 0; i < enumCount; i++)
        {

            //i�Ԃ�AnimMode��string�^�ɕϊ�
            string animName = Enum.GetValues(typeof(AnimBoolMode)).GetValue(i).ToString();

            //animName�̃A�j���[�V�������A�N�e�B�u
            anim.SetBool(animName, false);
        }

        //animMode�̃A�j���[�V�������A�N�e�B�u
        anim.SetBool(animBoolMode.ToString(), true);
    }
    /// <summary>
    /// TriggerAnimation�̕ύX
    /// </summary>
    public void ChangeTriggerAnimation(Animator anim, AnimComboMode animCombo)
    {

        //AnimMode��enum��ނ��J�E���g
        int enumCount = Enum.GetValues(typeof(AnimBoolMode)).Length;

        //AnimMode�̎�ޕ�����
        for (int i = 0; i < enumCount; i++)
        {

            //i�Ԃ�AnimMode��string�^�ɕϊ�
            string animName = Enum.GetValues(typeof(AnimBoolMode)).GetValue(i).ToString();

            //animName�̃A�j���[�V�������A�N�e�B�u
            anim.SetBool(animName, false);
        }

        //animCombo�̃A�j���[�V�������A�N�e�B�u
        anim.SetTrigger(animCombo.ToString());
    }
    /// <summary>
    /// Animator��UpdateMode��ApplyMode�̕ύX
    /// </summary>
    public void ChangeAnimatorApplyMode(Animator anim, AnimatorUpdateModeEnum animatorUpdateModeEnum)
    {

        //Animator��UpdateMode�ɑΉ����Đ؂�ւ�
        if (animatorUpdateModeEnum == AnimatorUpdateModeEnum.NOMAL)
        {
            //Animator��UpdateMode��؂�ւ�
            //Normal = �^�C���X�P�[����̂̕�������
            anim.updateMode = AnimatorUpdateMode.Normal;
            //Animator��ApplyRootMotion��؂�ւ�
            //�L�����N�^�[�̈ʒu�Ɖ�]���X�N���v�g���琧�䂷�邩
            anim.applyRootMotion = false;

        }
        else if(animatorUpdateModeEnum == AnimatorUpdateModeEnum.ANIMATE_PHYSICS)
        {
            //AnimatePhysics = �A�j���[�V������̂̕�������
            anim.updateMode = AnimatorUpdateMode.AnimatePhysics;
            //Animator��ApplyRootMotion��؂�ւ�
            //�L�����N�^�[�̈ʒu�Ɖ�]���A�j���[�V�������̂��琧��
            anim.applyRootMotion = true;
        }
    }
}