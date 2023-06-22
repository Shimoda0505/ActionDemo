using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Player�̋����Ǘ��N���X
/// </summary>
public class PlayerController : MonoBehaviour
{

    [Header("�X�N���v�g�֘A")]
    protected PlayerAnimData_S _animData = default;//�A�j���[�V�����Ǘ��N���X
    protected PlayerManager _playerManager = default;//�v���[���[�Ǘ��N���X
    protected PlayerData _playerData = default;//�v���C���[�̌ʃf�[�^�N���X
    protected List<GameObject> _playerWeponObjs = new List<GameObject>();//����̔���Ǘ��N���X
    protected List<PlayerWeponCollider> _playerWeponColliders = new List<PlayerWeponCollider>();//����̔���Ǘ��N���X


    [Header("�R���|�[�l���g�֘A")]
    protected Transform _tr = default;//�v���C���[��Transform
    protected Rigidbody _rb = default;//�v���C���[��Rigidbody
    protected Animator _anim = default;//�v���C���[��Animator


    [Header("���͊֘A")]
    private Vector3 _moveForward = default;//�ړ�����


    [Header("�ړ��֘A")]
    protected bool _isMove = false;//�ړ�����
    private bool _isRun = false;//���蒆��
    protected bool _isAction = false;//��𒆂�
    protected bool _isAtack = false;//�U������
    protected bool _isCombo = false;//�R���{�ł��邩
    private bool _isAttackCoroutine = false;//�U���R���[�`���������Ă��邩


    public enum MotionEnum
    {
        [InspectorName("�ҋ@")] IDLE,
        [InspectorName("����")] WALK,
        [InspectorName("����")] RUN,
        [InspectorName("���")] ROLL,
        [InspectorName("�X�e�b�v")] STEP,
        [InspectorName("�U��")] ATTACK,
        [InspectorName("�K�[�h")] GUARD,
        [InspectorName("�X�L��")] SKILL,
        [InspectorName("�_���[�W")] DAMAGE,
        [InspectorName("�_�E��")] DOWN_WEAK,
        [InspectorName("�_�E��")] DOWN_STRONG,
        [InspectorName("���S")] DEATH,
        [InspectorName("�����E�[��")] EQUIP,
    }
    protected MotionEnum _motionEnum = default;



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //�p�u���b�N----------------------------------------------------------------------------------------
    /*[ �����ݒ�֘A ]*/
    /// <summary>
    /// �R���|�[�l���g�֘A�̏����ݒ�
    /// </summary>
    public void SetComponent(Transform tr, Rigidbody rb, Animator anim) { _tr = tr; _rb = rb; _anim = anim; }
    /// <summary>
    /// PlayerData�̏����ݒ�
    /// </summary>
    public void SetPlayerData(PlayerData playerData) { _playerData = playerData; }
    /// <summary>
    /// PlayerManager�̏����ݒ�
    /// </summary>
    public void SetPlayerManager(PlayerManager playerManager) { _playerManager = playerManager; }
    /// <summary>
    /// �ړ������̐ݒ�
    /// </summary>
    public void SetMoveForward(Vector3 moveForward) { _moveForward = moveForward; }
    /// <summary>
    /// PlayerWeponCollider�̏����ݒ�
    /// </summary>
    public void SetPlayerWeponCollider(PlayerWeponCollider playerWeponCollider,GameObject playerWepon) { _playerWeponColliders.Add(playerWeponCollider); _playerWeponObjs.Add(playerWepon); }
    /// <summary>
    /// �_���[�W��MotionEnum�̕ύX
    /// </summary>
    public void SetMotionEnum(MotionEnum motionEnum)
    {

        //�S�Ă�������
        ALLReset();

        _motionEnum = motionEnum;

        switch (_motionEnum)
        {

            case MotionEnum.DAMAGE:

                //�_���[�W�A�j���[�V������true
                _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.DAMAGE);
                //�A�j���[�V����Mode��ύX
                _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);

                StartCoroutine(EndDamage(_playerData.DamagePlayerData().damageInterval));
                break;


            case MotionEnum.DOWN_STRONG:


                //�_�E���A�j���[�V������true
                _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.DOWN_STRONG);
                //�A�j���[�V����Mode��ύX
                _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);

                StartCoroutine(EndDamage(_playerData.DamagePlayerData().downStrongInterval));
                StartCoroutine(EndDown(_playerData.DamagePlayerData().downStrongInterval / 2));
                break;


            case MotionEnum.DOWN_WEAK:

                //�_�E���A�j���[�V������true
                _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.DOWN_WEAK);
                //�A�j���[�V����Mode��ύX
                _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);

                StartCoroutine(EndDamage(_playerData.DamagePlayerData().downWeakInterval));
                break;


            case MotionEnum.DEATH:

                //���S�A�j���[�V������true
                _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.DEATH);
                //�A�j���[�V����Mode��ύX
                _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
                break;


            default:

                print("MotionEnum��case�Ȃ�");
                break;
        }
    }

    /*[ �Q�Ɗ֘A ]*/
    /// <summary>
    /// �w�肵��MotionEnum����
    /// </summary>
    /// <returns></returns>
    public bool GetNowMotionEnum(MotionEnum motionEnum) { if (_motionEnum == motionEnum) { return true; } return false; }
    /// <summary>
    /// �ړ������̎擾
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMoveForward() { return _moveForward; }

    /*[ �ړ��֘A ]*/
    /// <summary>
    /// �v���C���[�̈ړ����͊Ǘ�
    /// </summary>
    public virtual void PlayerMoveInput(bool isMove)
    {

        try
        {

            //�ړ�������⊮
            _isMove = isMove;

            switch (_motionEnum)
            {
                case MotionEnum.IDLE:

                    //�������͎�
                    if (isMove)
                    {

                        //����enum�ɑJ��
                        _motionEnum = MotionEnum.WALK;
                        //�����A�j���[�V������true
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.WALK);
                    }
                    break;


                case MotionEnum.WALK:

                    //���������͎�
                    if (!isMove) { StartCoroutine("EndMove"); }
                    else { StopCoroutine("EndMove"); }
                    break;


                case MotionEnum.RUN:

                    //���������͎�
                    if (!isMove) { StartCoroutine("EndMove"); }
                    else { StopCoroutine("EndMove"); }
                    break;


                case MotionEnum.ROLL:

                    //�A�N�V�����I�����ɑҋ@�E�ړ�enum�ɑJ��
                    if (!_isAction) { EndAction(); }
                    break;


                case MotionEnum.STEP:

                    //�A�N�V�����I�����ɑҋ@�E�ړ�enum�ɑJ��
                    if (!_isAction) { EndAction(); }
                    break;


                case MotionEnum.ATTACK:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.GUARD:
                    //�ePlayerController���O�N���X������
                    break;


                case MotionEnum.SKILL:

                    //�A�N�V�����I�����ɑҋ@�E�ړ�enum�ɑJ��
                    if (!_isAction) { EndAction(); }
                    break;


                case MotionEnum.DAMAGE:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DOWN_WEAK:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DOWN_STRONG:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DEATH:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.EQUIP:
                    //�ePlayerController���O�N���X������
                    break;

                default:

                    print("MotionEnum��case�Ȃ�");
                    break;
            }
        }
        catch { print("PlayerInput"); }
    }
    /// <summary>
    /// �v���C���[�̃A�N�V�������͊Ǘ�
    /// </summary>
    public virtual void PlayerActionInput((bool down, bool up) isMoveAction, (bool down, bool up) isItem)
    {

        try
        {

            //isMoveAction�̓��͒����𔻕ʂ�,_isRun�̃t���O�ύX
            if (isMoveAction.down) { _isRun = true; }
            else if (isMoveAction.up) { _isRun = false; }

            switch (_motionEnum)
            {
                case MotionEnum.IDLE:

                    //������͎��A�X�e�b�venum�ɕύX
                    if (isMoveAction.down)
                    {

                        //�X�^�~�i�̏���
                        if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().stepStamina)) { return; }

                        //�A�N�V����Flag��true
                        _isAction = true;
                        //�X�e�b�venum�ɑJ��
                        _motionEnum = MotionEnum.STEP;
                        //�A�j���[�V����Mode��ύX
                        _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
                        //�X�e�b�v�A�j���[�V������true
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.STEP);
                    }
                    break;


                case MotionEnum.WALK:

                    //������͎��ɑ���enum�ɕύX
                    if (isMoveAction.down) { StartCoroutine("StartRun"); }
                    else if (isMoveAction.up)
                    {

                        //�X�^�~�i�̏���A����ʈȉ��̃X�^�~�i�Ȃ珈�����Ȃ�
                        if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().rollStamina)) { return; }

                        //�A�N�V����Flag��true
                        _isAction = true;
                        //����ҋ@�̔j��
                        StopCoroutine("StartRun");
                        //���enum�ɑJ��
                        _motionEnum = MotionEnum.ROLL;
                        //�A�j���[�V����Mode��ύX
                        _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
                        //����A�j���[�V������true
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.ROLL);
                    }
                    break;


                case MotionEnum.RUN:

                    //���薢���͎��ɏI���ҋ@��A����enum�ɕύX
                    if (isMoveAction.up) { StartCoroutine("EndRun"); }
                    //������͂̃_�u���N���b�N���A���enum�ɕύX
                    if (isMoveAction.down)
                    {

                        //�X�^�~�i�̏���A����ʈȉ��̃X�^�~�i�Ȃ珈�����Ȃ�
                        if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().rollStamina)) { return; }

                        //�A�N�V����Flag��true
                        _isAction = true;
                        //�I���ҋ@�̔j��
                        StopCoroutine("EndRun");
                        //���enum�ɑJ��
                        _motionEnum = MotionEnum.ROLL;
                        //�A�j���[�V����Mode��ύX
                        _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
                        //����A�j���[�V������true
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.ROLL);
                    }
                    break;


                case MotionEnum.ROLL:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.STEP:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.ATTACK:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.GUARD:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.SKILL:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DAMAGE:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DOWN_WEAK:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DOWN_STRONG:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DEATH:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.EQUIP:
                    //�ePlayerController���O�N���X������
                    break;


                default:

                    print("MotionEnum��case�Ȃ�");
                    break;
            }
        }
        catch { print("PlayerInput"); }
    }
    /// <summary>
    /// �v���C���[�̍U�����͊Ǘ�
    /// </summary>
    public virtual void PlayerAttackInput((bool down, bool up) isAttackWeak, (bool down, bool up) isAttackStrong,
                                          (bool down, bool up) isGuard, (bool down, bool up) isSkill, bool isAction)
    {

        try
        {

            switch (_motionEnum)
            {
                case MotionEnum.IDLE:

                    //�U�����͎��̃A�N�V����
                    AttackMotion(isAttackWeak, isAttackStrong, isGuard, isSkill, isAction);
                    break;


                case MotionEnum.WALK:

                    //�U�����͎��̃A�N�V����
                    AttackMotion(isAttackWeak, isAttackStrong, isGuard, isSkill, isAction);
                    break;


                case MotionEnum.RUN:

                    //�U�����͎��̃A�N�V����
                    AttackMotion(isAttackWeak, isAttackStrong, isGuard, isSkill, isAction);
                    break;


                case MotionEnum.ATTACK:

                    ////�U��Flag��false�Ȃ�I���ҋ@
                    if (!_isAtack && !_isAttackCoroutine) { StartCoroutine("EndAttack"); _isAttackCoroutine = true; }
                    //�U��Flag��true�Ȃ�I���ҋ@�̔j��
                    else if (_isAtack && _isAttackCoroutine) { StopCoroutine("EndAttack"); _isAttackCoroutine = false; }
                    break;


                case MotionEnum.ROLL:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.STEP:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.GUARD:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.SKILL:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DAMAGE:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DOWN_WEAK:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DOWN_STRONG:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.DEATH:
                    //�ePlayerController���O�N���X������
                    break;
                case MotionEnum.EQUIP:
                    //�ePlayerController���O�N���X������
                    break;


                default:

                    print("MotionEnum��case�Ȃ�");
                    break;
            }
        }
        catch { print("PlayerInput"); }
    }
    /// <summary>
    /// �v���C���[�̈ړ��Ǘ�
    /// </summary>
    public virtual void PlayerMove(Vector2 moveInput)
    {

        try
        {

            switch (_motionEnum)
            {

                case MotionEnum.IDLE:

                    //�X�^�~�i�̉�
                    _playerManager.StaminaControl(PlayerState.StaminaEnum.HEALING, _playerData.StuminaSpeedPlayerData().healingSpeedStamina);

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.WALK:

                    //�X�^�~�i�̉�
                    _playerManager.StaminaControl(PlayerState.StaminaEnum.HEALING, _playerData.StuminaSpeedPlayerData().healingSpeedStamina);

                    //�v���C���[�̈ړ��Ɖ�](�n��)
                    if(moveInput != Vector2.zero) GroundMove(_rb, _tr, moveInput, _playerData.MovePlayerData().walkSpeed);
                    break;


                case MotionEnum.RUN:

                    //�X�^�~�i�̏���
                    if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaSpeedPlayerData().runSpeedStamina))
                    {

                        //����enum�ɑJ��
                        _motionEnum = MotionEnum.WALK;
                        //�����A�j���[�V������true
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.WALK);
                        return;
                    }

                    //�v���C���[�̈ړ��Ɖ�](�n��)
                    if (moveInput != Vector2.zero) GroundMove(_rb, _tr, moveInput, _playerData.MovePlayerData().runSpeed);
                    break;


                case MotionEnum.ATTACK:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.ROLL:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.STEP:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.GUARD:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.SKILL:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.DAMAGE:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.DOWN_WEAK:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.DOWN_STRONG:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.DEATH:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;

                default:

                    //�ړ����Ȃ�
                    GroundMove(_rb, _tr, moveInput, 0);
                    print("MotionEnum��case�Ȃ�");
                    break;
            }
        }
        catch { print("PlayerMove"); }
    }

    /*[ �A�j���[�V�����֘A ]*/
    /// <summary>
    /// �A�j���[�V������Idle�ɏ�����
    /// </summary>
    public void ResetAnim() { _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.IDLE); }
    /// <summary>
    /// �A�N�V����Flag��false
    /// Animation��Events�Ŏg�p
    /// </summary>
    public void EndActionAnim() { _isAction = false; }
    /// <summary>
    /// �U�������̃t���O�𔽓]������
    /// Animation��Events�Ŏg�p
    /// </summary>
    public void NowAttackAnim() { _isAtack = false; }
    /// <summary>
    /// �R���{�Ȃ�
    /// </summary>
    public void EndComboAnim() { _isCombo = false; }
    /// <summary>
    /// �U������̓���ւ�
    /// </summary>
    public void AttackCollider(int isCollider)
    {

        //����̗L��,�U���͂̕ύX
        if (isCollider == 1) { for (int i = 0; i < _playerWeponColliders.Count; i++) { _playerWeponColliders[i].ChangeCollider(true, _playerData.AttackPlayerData()._attackStrong); } }
        //����̗L��,�U���͂̕ύX
        else { for (int i = 0; i < _playerWeponColliders.Count; i++) { _playerWeponColliders[i].ChangeCollider(false, 0); } }
    }


    //�v���C�x�[�g----------------------------------------------------------------------------------------
    /*[ �ړ��֘A ]*/
    /// <summary>
    /// �v���C���[�̈ړ��Ɖ�](�n��)
    /// </summary>
    private void GroundMove(Rigidbody playerRb, Transform playerTrans, Vector2 input, float speed)
    {

        //�ړ����x0�Ȃ�
        if (speed == 0 )
        {

            //�ړ����Ȃ�
            playerRb.velocity = Vector3.zero;
            //�⊮�����ړ�����������
            playerTrans.rotation = Quaternion.LookRotation(_moveForward);
            return;
        }

        //�J�����̐��ʕ���������
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //���͒l�ƃJ�����̌�������ړ��������v�Z
        Vector3 moveForward = cameraForward * input.y + Camera.main.transform.right * input.x;
        //�ړ������̕ۊ�
        _moveForward = moveForward;
        //�ړ��̔��f
        playerRb.velocity = moveForward * speed + new Vector3(0, playerRb.velocity.y, 0);
        //�i�s����������
        playerTrans.rotation = Quaternion.LookRotation(moveForward);
    }

    /*[ �U���֘A ]*/
    /// <summary>
    /// �U������
    /// </summary>
    private void AttackMotion((bool down, bool up) isAttackWeak, (bool down, bool up) isAttackStrong, (bool down, bool up) isGuard, (bool down, bool up) isSkill, bool isAction)
    {

        if (isSkill.down)
        {

            //�X�^�~�i�̏���A����ʈȉ��̃X�^�~�i�Ȃ珈�����Ȃ�
            if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().skillStamina)) { return; }

            //�A�N�V����Flag��true
            _isAction = true;
            //�X�L��enum�ɑJ��
            _motionEnum = MotionEnum.SKILL;
            //�A�j���[�V����Mode��ύX
            _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
            //�X�L���A�j���[�V������true
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.SKILL);
        }
        //��E�U�����͎��ɍU��enum�ɕύX
        else if (isAttackWeak.down)
        {

            //�X�^�~�i�̏���A����ʈȉ��̃X�^�~�i�Ȃ珈�����Ȃ�
            if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().attackWeakStamina)) { return; }

            //�U��Flag��true
            _isAtack = true;
            //�R���{�\
            _isCombo = true;
            //�U��enum�ɑJ��
            _motionEnum = MotionEnum.ATTACK;
            //�A�j���[�V����Mode��ύX
            _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
            //��U���A�j���[�V������true
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.ATTACK_WEAK);
        }
        //���E�U�����͎��ɍU��enum�ɕύX
        else if (isAttackStrong.down)
        {

            //�X�^�~�i�̏���A����ʈȉ��̃X�^�~�i�Ȃ珈�����Ȃ�
            if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().attackStrongStamina)) { return; }

            //�U��Flag��true
            _isAtack = true;
            //�R���{�\
            _isCombo = true;
            //�U��enum�ɑJ��
            _motionEnum = MotionEnum.ATTACK;
            //�A�j���[�V����Mode��ύX
            _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
            //���U���A�j���[�V������true
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.ATTACK_STRONG);
        }
    }
    /// <summary>
    /// �A�N�V�����̏I�����ɁA�ҋ@���ړ��ɑJ��
    /// </summary>
    protected void EndAction()
    {

        //�ړ����͂��Ă��āA���蒆�Ȃ瑖��enum
        if (_isRun) { _motionEnum = MotionEnum.RUN; _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.RUN); }
        //�ړ����͂��Ă���Ȃ�ړ�enum
        else if (_isMove) { _motionEnum = MotionEnum.WALK; _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.WALK); }
        //�ړ����͂��ĂȂ��Ȃ�ҋ@enum
        else { _motionEnum = MotionEnum.IDLE; _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.IDLE); }

        //�A�j���[�V����Mode��ύX
        _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.NOMAL);
    }
    /// <summary>
    /// �S�Ă�������
    /// </summary>
    private void ALLReset()
    {


        StopCoroutine("EndAttack");
        StopCoroutine("StartRun");
        StopCoroutine("EndRun");

        _isMove = false;//�ړ�����
        _isRun = false;//���蒆��
        _isAction = false;//��𒆂�
        _isAtack = false;//�U������
        _isCombo = false;//�R���{�ł��邩
        _isAttackCoroutine = false;//�U���R���[�`���������Ă��邩

        //����̗L��,�U���͂̕ύX
        for (int i = 0; i < _playerWeponColliders.Count; i++) { _playerWeponColliders[i].ChangeCollider(false, 0); }
    }


    //�R���[�`��---------------------------------------------------------------------------------------------
    public IEnumerator EndAttack()
    {

        //�ҋ@
        yield return new WaitForSeconds(_playerData.AttackPlayerData().attackInterval);

        //�R���{�t���O�I��
        _isCombo = false;

        //�A�N�V�����̏I�����ɁA�ҋ@���ړ��ɑJ��
        EndAction();
    }
    private IEnumerator StartRun()
    {

        //�ҋ@
        yield return new WaitForSeconds(_playerData.MovePlayerData().runInterval);

        //����enum�ɑJ��
        _motionEnum = MotionEnum.RUN;
        //����A�j���[�V������true
        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.RUN);
    }
    private IEnumerator EndRun()
    {

        //�ҋ@
        yield return new WaitForSeconds(_playerData.MovePlayerData().runInterval);

        //����enum�ɑJ��
        _motionEnum = MotionEnum.WALK;
        //�����A�j���[�V������true
        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.WALK);
    }
    private IEnumerator EndMove()
    {

        //�ҋ@
        yield return new WaitForSeconds(_playerData.MovePlayerData().moveInterval);

        //����������Ȃ�
        if(_motionEnum == MotionEnum.WALK || _motionEnum == MotionEnum.RUN)
        {

            //����enum�ɑJ��
            _motionEnum = MotionEnum.IDLE;
            //�����A�j���[�V������true
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.IDLE);
        }
    }

    private IEnumerator EndDamage(float interval)
    {

        //�ҋ@
        yield return new WaitForSeconds(interval);

        //�A�N�V�����̏I�����ɁA�ҋ@���ړ��ɑJ��
        EndAction();
    }
    private IEnumerator EndDown(float interval)
    {

        //�ҋ@
        yield return new WaitForSeconds(interval);

        //�_�E���A�j���[�V������true
        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.NULL);
    }
}
