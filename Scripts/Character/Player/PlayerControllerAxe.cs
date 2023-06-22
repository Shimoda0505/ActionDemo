
/// <summary>
/// Player(Hand)の挙動管理クラス
/// </summary>
public class PlayerControllerAxe : PlayerController
{



    //メソッド部----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// プレイヤーの入力管理
    /// </summary>
    public override void PlayerAttackInput((bool down, bool up) isAttackWeak, (bool down, bool up) isAttackStrong, 
                                            (bool down, bool up) isGuard, (bool down, bool up) isSkill, bool isAction)
    {

        print("Axe使用中");

        //基底処理
        base.PlayerAttackInput(isAttackWeak, isAttackStrong, isGuard, isSkill, isAction);

        switch (_motionEnum)
        {

            case MotionEnum.IDLE:

                //ガード処理
                GuardAction(isGuard.down);
                break;


            case MotionEnum.WALK:

                //ガード処理
                GuardAction(isGuard.down);
                break;


            case MotionEnum.RUN:

                //ガード処理
                GuardAction(isGuard.down);
                break;


            case MotionEnum.ATTACK:

                //攻撃中は処理しない
                if (_isAtack || !_isCombo) { return; }

                //弱攻撃入力時に弱アニメーション
                if (isAttackWeak.down) 
                {

                    //スタミナの消費、消費量以下のスタミナなら処理しない
                    if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().attackStrongStamina)) { return; }

                    //攻撃Flagをtrue
                    _isAtack = true;
                    //弱コンボアニメーションをtrue
                    _animData.ChangeTriggerAnimation(_anim, PlayerAnimData_S.AnimComboMode.COMBO_WEAK);
                }
                //強攻撃入力時に強アニメーション
                else if (isAttackStrong.down) 
                {

                    //スタミナの消費、消費量以下のスタミナなら処理しない
                    if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().attackWeakStamina)) { return; }

                    //攻撃Flagをtrue
                    _isAtack = true;
                    //強コンボアニメーションをtrue
                    _animData.ChangeTriggerAnimation(_anim, PlayerAnimData_S.AnimComboMode.COMBO_STRONG);
                }

                //ガード処理
                GuardAction(isGuard.down);
                break;


            case MotionEnum.ROLL:
                break;


            case MotionEnum.STEP:
                break;


            case MotionEnum.GUARD:

                //ガード修了時
                //アクションの終了時に、待機か移動に遷移
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

                print("MotionEnumのcaseなし");
                break;
        }
    }

    private void GuardAction(bool isGuard)
    {

        //ガード入力時に攻撃enumに変更
        if (isGuard)
        {

            //ガードenumに遷移
            _motionEnum = MotionEnum.GUARD;
            //アニメーションModeを変更
            _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
            //ガードアニメーションをtrue
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.GUARD);
        }
    }
}
