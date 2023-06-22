
/// <summary>
/// Player(Hand)�̋����Ǘ��N���X
/// </summary>
public class PlayerControllerAxe : PlayerController
{



    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �v���C���[�̓��͊Ǘ�
    /// </summary>
    public override void PlayerAttackInput((bool down, bool up) isAttackWeak, (bool down, bool up) isAttackStrong, 
                                            (bool down, bool up) isGuard, (bool down, bool up) isSkill, bool isAction)
    {

        print("Axe�g�p��");

        //��ꏈ��
        base.PlayerAttackInput(isAttackWeak, isAttackStrong, isGuard, isSkill, isAction);

        switch (_motionEnum)
        {

            case MotionEnum.IDLE:

                //�K�[�h����
                GuardAction(isGuard.down);
                break;


            case MotionEnum.WALK:

                //�K�[�h����
                GuardAction(isGuard.down);
                break;


            case MotionEnum.RUN:

                //�K�[�h����
                GuardAction(isGuard.down);
                break;


            case MotionEnum.ATTACK:

                //�U�����͏������Ȃ�
                if (_isAtack || !_isCombo) { return; }

                //��U�����͎��Ɏ�A�j���[�V����
                if (isAttackWeak.down) 
                {

                    //�X�^�~�i�̏���A����ʈȉ��̃X�^�~�i�Ȃ珈�����Ȃ�
                    if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().attackStrongStamina)) { return; }

                    //�U��Flag��true
                    _isAtack = true;
                    //��R���{�A�j���[�V������true
                    _animData.ChangeTriggerAnimation(_anim, PlayerAnimData_S.AnimComboMode.COMBO_WEAK);
                }
                //���U�����͎��ɋ��A�j���[�V����
                else if (isAttackStrong.down) 
                {

                    //�X�^�~�i�̏���A����ʈȉ��̃X�^�~�i�Ȃ珈�����Ȃ�
                    if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().attackWeakStamina)) { return; }

                    //�U��Flag��true
                    _isAtack = true;
                    //���R���{�A�j���[�V������true
                    _animData.ChangeTriggerAnimation(_anim, PlayerAnimData_S.AnimComboMode.COMBO_STRONG);
                }

                //�K�[�h����
                GuardAction(isGuard.down);
                break;


            case MotionEnum.ROLL:
                break;


            case MotionEnum.STEP:
                break;


            case MotionEnum.GUARD:

                //�K�[�h�C����
                //�A�N�V�����̏I�����ɁA�ҋ@���ړ��ɑJ��
                if (isGuard.up) { EndAction(); }
                break;


            case MotionEnum.SKILL:
                break;
            case MotionEnum.DAMAGE:
                break;
            case MotionEnum.DOWN_WEAK:
                break;
            case MotionEnum.DOWN_STRONG:
                break;
            case MotionEnum.DEATH:
                break;


            default:

                print("MotionEnum��case�Ȃ�");
                break;
        }
    }

    private void GuardAction(bool isGuard)
    {

        //�K�[�h���͎��ɍU��enum�ɕύX
        if (isGuard)
        {

            //�K�[�henum�ɑJ��
            _motionEnum = MotionEnum.GUARD;
            //�A�j���[�V����Mode��ύX
            _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
            //�K�[�h�A�j���[�V������true
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.GUARD);
        }
    }
}
