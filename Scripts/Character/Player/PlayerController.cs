using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Playerの挙動管理クラス
/// </summary>
public class PlayerController : MonoBehaviour
{

    [Header("スクリプト関連")]
    protected PlayerAnimData_S _animData = default;//アニメーション管理クラス
    protected PlayerManager _playerManager = default;//プレーヤー管理クラス
    protected PlayerData _playerData = default;//プレイヤーの個別データクラス
    protected List<GameObject> _playerWeponObjs = new List<GameObject>();//武器の判定管理クラス
    protected List<PlayerWeponCollider> _playerWeponColliders = new List<PlayerWeponCollider>();//武器の判定管理クラス


    [Header("コンポーネント関連")]
    protected Transform _tr = default;//プレイヤーのTransform
    protected Rigidbody _rb = default;//プレイヤーのRigidbody
    protected Animator _anim = default;//プレイヤーのAnimator


    [Header("入力関連")]
    private Vector3 _moveForward = default;//移動方向


    [Header("移動関連")]
    protected bool _isMove = false;//移動中か
    private bool _isRun = false;//走り中か
    protected bool _isAction = false;//回避中か
    protected bool _isAtack = false;//攻撃中か
    protected bool _isCombo = false;//コンボできるか
    private bool _isAttackCoroutine = false;//攻撃コルーチンが動いているか


    public enum MotionEnum
    {
        [InspectorName("待機")] IDLE,
        [InspectorName("歩き")] WALK,
        [InspectorName("走り")] RUN,
        [InspectorName("回避")] ROLL,
        [InspectorName("ステップ")] STEP,
        [InspectorName("攻撃")] ATTACK,
        [InspectorName("ガード")] GUARD,
        [InspectorName("スキル")] SKILL,
        [InspectorName("ダメージ")] DAMAGE,
        [InspectorName("ダウン")] DOWN_WEAK,
        [InspectorName("ダウン")] DOWN_STRONG,
        [InspectorName("死亡")] DEATH,
        [InspectorName("抜刀・納刀")] EQUIP,
    }
    protected MotionEnum _motionEnum = default;



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //パブリック----------------------------------------------------------------------------------------
    /*[ 初期設定関連 ]*/
    /// <summary>
    /// コンポーネント関連の初期設定
    /// </summary>
    public void SetComponent(Transform tr, Rigidbody rb, Animator anim) { _tr = tr; _rb = rb; _anim = anim; }
    /// <summary>
    /// PlayerDataの初期設定
    /// </summary>
    public void SetPlayerData(PlayerData playerData) { _playerData = playerData; }
    /// <summary>
    /// PlayerManagerの初期設定
    /// </summary>
    public void SetPlayerManager(PlayerManager playerManager) { _playerManager = playerManager; }
    /// <summary>
    /// 移動方向の設定
    /// </summary>
    public void SetMoveForward(Vector3 moveForward) { _moveForward = moveForward; }
    /// <summary>
    /// PlayerWeponColliderの初期設定
    /// </summary>
    public void SetPlayerWeponCollider(PlayerWeponCollider playerWeponCollider,GameObject playerWepon) { _playerWeponColliders.Add(playerWeponCollider); _playerWeponObjs.Add(playerWepon); }
    /// <summary>
    /// ダメージにMotionEnumの変更
    /// </summary>
    public void SetMotionEnum(MotionEnum motionEnum)
    {

        //全てを初期化
        ALLReset();

        _motionEnum = motionEnum;

        switch (_motionEnum)
        {

            case MotionEnum.DAMAGE:

                //ダメージアニメーションをtrue
                _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.DAMAGE);
                //アニメーションModeを変更
                _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);

                StartCoroutine(EndDamage(_playerData.DamagePlayerData().damageInterval));
                break;


            case MotionEnum.DOWN_STRONG:


                //ダウンアニメーションをtrue
                _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.DOWN_STRONG);
                //アニメーションModeを変更
                _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);

                StartCoroutine(EndDamage(_playerData.DamagePlayerData().downStrongInterval));
                StartCoroutine(EndDown(_playerData.DamagePlayerData().downStrongInterval / 2));
                break;


            case MotionEnum.DOWN_WEAK:

                //ダウンアニメーションをtrue
                _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.DOWN_WEAK);
                //アニメーションModeを変更
                _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);

                StartCoroutine(EndDamage(_playerData.DamagePlayerData().downWeakInterval));
                break;


            case MotionEnum.DEATH:

                //死亡アニメーションをtrue
                _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.DEATH);
                //アニメーションModeを変更
                _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
                break;


            default:

                print("MotionEnumのcaseなし");
                break;
        }
    }

    /*[ 参照関連 ]*/
    /// <summary>
    /// 指定したMotionEnum中か
    /// </summary>
    /// <returns></returns>
    public bool GetNowMotionEnum(MotionEnum motionEnum) { if (_motionEnum == motionEnum) { return true; } return false; }
    /// <summary>
    /// 移動方向の取得
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMoveForward() { return _moveForward; }

    /*[ 移動関連 ]*/
    /// <summary>
    /// プレイヤーの移動入力管理
    /// </summary>
    public virtual void PlayerMoveInput(bool isMove)
    {

        try
        {

            //移動中かを補完
            _isMove = isMove;

            switch (_motionEnum)
            {
                case MotionEnum.IDLE:

                    //歩き入力時
                    if (isMove)
                    {

                        //歩きenumに遷移
                        _motionEnum = MotionEnum.WALK;
                        //歩きアニメーションをtrue
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.WALK);
                    }
                    break;


                case MotionEnum.WALK:

                    //歩き未入力時
                    if (!isMove) { StartCoroutine("EndMove"); }
                    else { StopCoroutine("EndMove"); }
                    break;


                case MotionEnum.RUN:

                    //歩き未入力時
                    if (!isMove) { StartCoroutine("EndMove"); }
                    else { StopCoroutine("EndMove"); }
                    break;


                case MotionEnum.ROLL:

                    //アクション終了時に待機・移動enumに遷移
                    if (!_isAction) { EndAction(); }
                    break;


                case MotionEnum.STEP:

                    //アクション終了時に待機・移動enumに遷移
                    if (!_isAction) { EndAction(); }
                    break;


                case MotionEnum.ATTACK:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.GUARD:
                    //各PlayerController名前クラスが処理
                    break;


                case MotionEnum.SKILL:

                    //アクション終了時に待機・移動enumに遷移
                    if (!_isAction) { EndAction(); }
                    break;


                case MotionEnum.DAMAGE:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DOWN_WEAK:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DOWN_STRONG:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DEATH:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.EQUIP:
                    //各PlayerController名前クラスが処理
                    break;

                default:

                    print("MotionEnumのcaseなし");
                    break;
            }
        }
        catch { print("PlayerInput"); }
    }
    /// <summary>
    /// プレイヤーのアクション入力管理
    /// </summary>
    public virtual void PlayerActionInput((bool down, bool up) isMoveAction, (bool down, bool up) isItem)
    {

        try
        {

            //isMoveActionの入力中かを判別し,_isRunのフラグ変更
            if (isMoveAction.down) { _isRun = true; }
            else if (isMoveAction.up) { _isRun = false; }

            switch (_motionEnum)
            {
                case MotionEnum.IDLE:

                    //走り入力時、ステップenumに変更
                    if (isMoveAction.down)
                    {

                        //スタミナの消費
                        if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().stepStamina)) { return; }

                        //アクションFlagをtrue
                        _isAction = true;
                        //ステップenumに遷移
                        _motionEnum = MotionEnum.STEP;
                        //アニメーションModeを変更
                        _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
                        //ステップアニメーションをtrue
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.STEP);
                    }
                    break;


                case MotionEnum.WALK:

                    //走り入力時に走りenumに変更
                    if (isMoveAction.down) { StartCoroutine("StartRun"); }
                    else if (isMoveAction.up)
                    {

                        //スタミナの消費、消費量以下のスタミナなら処理しない
                        if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().rollStamina)) { return; }

                        //アクションFlagをtrue
                        _isAction = true;
                        //走り待機の破棄
                        StopCoroutine("StartRun");
                        //回避enumに遷移
                        _motionEnum = MotionEnum.ROLL;
                        //アニメーションModeを変更
                        _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
                        //回避アニメーションをtrue
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.ROLL);
                    }
                    break;


                case MotionEnum.RUN:

                    //走り未入力時に終了待機後、歩きenumに変更
                    if (isMoveAction.up) { StartCoroutine("EndRun"); }
                    //走り入力のダブルクリック時、回避enumに変更
                    if (isMoveAction.down)
                    {

                        //スタミナの消費、消費量以下のスタミナなら処理しない
                        if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().rollStamina)) { return; }

                        //アクションFlagをtrue
                        _isAction = true;
                        //終了待機の破棄
                        StopCoroutine("EndRun");
                        //回避enumに遷移
                        _motionEnum = MotionEnum.ROLL;
                        //アニメーションModeを変更
                        _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
                        //回避アニメーションをtrue
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.ROLL);
                    }
                    break;


                case MotionEnum.ROLL:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.STEP:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.ATTACK:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.GUARD:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.SKILL:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DAMAGE:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DOWN_WEAK:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DOWN_STRONG:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DEATH:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.EQUIP:
                    //各PlayerController名前クラスが処理
                    break;


                default:

                    print("MotionEnumのcaseなし");
                    break;
            }
        }
        catch { print("PlayerInput"); }
    }
    /// <summary>
    /// プレイヤーの攻撃入力管理
    /// </summary>
    public virtual void PlayerAttackInput((bool down, bool up) isAttackWeak, (bool down, bool up) isAttackStrong,
                                          (bool down, bool up) isGuard, (bool down, bool up) isSkill, bool isAction)
    {

        try
        {

            switch (_motionEnum)
            {
                case MotionEnum.IDLE:

                    //攻撃入力時のアクション
                    AttackMotion(isAttackWeak, isAttackStrong, isGuard, isSkill, isAction);
                    break;


                case MotionEnum.WALK:

                    //攻撃入力時のアクション
                    AttackMotion(isAttackWeak, isAttackStrong, isGuard, isSkill, isAction);
                    break;


                case MotionEnum.RUN:

                    //攻撃入力時のアクション
                    AttackMotion(isAttackWeak, isAttackStrong, isGuard, isSkill, isAction);
                    break;


                case MotionEnum.ATTACK:

                    ////攻撃Flagがfalseなら終了待機
                    if (!_isAtack && !_isAttackCoroutine) { StartCoroutine("EndAttack"); _isAttackCoroutine = true; }
                    //攻撃Flagがtrueなら終了待機の破棄
                    else if (_isAtack && _isAttackCoroutine) { StopCoroutine("EndAttack"); _isAttackCoroutine = false; }
                    break;


                case MotionEnum.ROLL:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.STEP:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.GUARD:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.SKILL:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DAMAGE:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DOWN_WEAK:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DOWN_STRONG:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.DEATH:
                    //各PlayerController名前クラスが処理
                    break;
                case MotionEnum.EQUIP:
                    //各PlayerController名前クラスが処理
                    break;


                default:

                    print("MotionEnumのcaseなし");
                    break;
            }
        }
        catch { print("PlayerInput"); }
    }
    /// <summary>
    /// プレイヤーの移動管理
    /// </summary>
    public virtual void PlayerMove(Vector2 moveInput)
    {

        try
        {

            switch (_motionEnum)
            {

                case MotionEnum.IDLE:

                    //スタミナの回復
                    _playerManager.StaminaControl(PlayerState.StaminaEnum.HEALING, _playerData.StuminaSpeedPlayerData().healingSpeedStamina);

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.WALK:

                    //スタミナの回復
                    _playerManager.StaminaControl(PlayerState.StaminaEnum.HEALING, _playerData.StuminaSpeedPlayerData().healingSpeedStamina);

                    //プレイヤーの移動と回転(地面)
                    if(moveInput != Vector2.zero) GroundMove(_rb, _tr, moveInput, _playerData.MovePlayerData().walkSpeed);
                    break;


                case MotionEnum.RUN:

                    //スタミナの消費
                    if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaSpeedPlayerData().runSpeedStamina))
                    {

                        //歩きenumに遷移
                        _motionEnum = MotionEnum.WALK;
                        //歩きアニメーションをtrue
                        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.WALK);
                        return;
                    }

                    //プレイヤーの移動と回転(地面)
                    if (moveInput != Vector2.zero) GroundMove(_rb, _tr, moveInput, _playerData.MovePlayerData().runSpeed);
                    break;


                case MotionEnum.ATTACK:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.ROLL:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.STEP:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.GUARD:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.SKILL:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.DAMAGE:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.DOWN_WEAK:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.DOWN_STRONG:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;


                case MotionEnum.DEATH:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    break;

                default:

                    //移動しない
                    GroundMove(_rb, _tr, moveInput, 0);
                    print("MotionEnumのcaseなし");
                    break;
            }
        }
        catch { print("PlayerMove"); }
    }

    /*[ アニメーション関連 ]*/
    /// <summary>
    /// アニメーションをIdleに初期化
    /// </summary>
    public void ResetAnim() { _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.IDLE); }
    /// <summary>
    /// アクションFlagのfalse
    /// AnimationのEventsで使用
    /// </summary>
    public void EndActionAnim() { _isAction = false; }
    /// <summary>
    /// 攻撃中かのフラグを反転させる
    /// AnimationのEventsで使用
    /// </summary>
    public void NowAttackAnim() { _isAtack = false; }
    /// <summary>
    /// コンボなし
    /// </summary>
    public void EndComboAnim() { _isCombo = false; }
    /// <summary>
    /// 攻撃判定の入れ替え
    /// </summary>
    public void AttackCollider(int isCollider)
    {

        //判定の有無,攻撃力の変更
        if (isCollider == 1) { for (int i = 0; i < _playerWeponColliders.Count; i++) { _playerWeponColliders[i].ChangeCollider(true, _playerData.AttackPlayerData()._attackStrong); } }
        //判定の有無,攻撃力の変更
        else { for (int i = 0; i < _playerWeponColliders.Count; i++) { _playerWeponColliders[i].ChangeCollider(false, 0); } }
    }


    //プライベート----------------------------------------------------------------------------------------
    /*[ 移動関連 ]*/
    /// <summary>
    /// プレイヤーの移動と回転(地面)
    /// </summary>
    private void GroundMove(Rigidbody playerRb, Transform playerTrans, Vector2 input, float speed)
    {

        //移動速度0なら
        if (speed == 0 )
        {

            //移動しない
            playerRb.velocity = Vector3.zero;
            //補完した移動方向を向く
            playerTrans.rotation = Quaternion.LookRotation(_moveForward);
            return;
        }

        //カメラの正面方向を所得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //入力値とカメラの向きから移動方向を計算
        Vector3 moveForward = cameraForward * input.y + Camera.main.transform.right * input.x;
        //移動方向の保管
        _moveForward = moveForward;
        //移動の反映
        playerRb.velocity = moveForward * speed + new Vector3(0, playerRb.velocity.y, 0);
        //進行方向を向く
        playerTrans.rotation = Quaternion.LookRotation(moveForward);
    }

    /*[ 攻撃関連 ]*/
    /// <summary>
    /// 攻撃制御
    /// </summary>
    private void AttackMotion((bool down, bool up) isAttackWeak, (bool down, bool up) isAttackStrong, (bool down, bool up) isGuard, (bool down, bool up) isSkill, bool isAction)
    {

        if (isSkill.down)
        {

            //スタミナの消費、消費量以下のスタミナなら処理しない
            if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().skillStamina)) { return; }

            //アクションFlagをtrue
            _isAction = true;
            //スキルenumに遷移
            _motionEnum = MotionEnum.SKILL;
            //アニメーションModeを変更
            _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
            //スキルアニメーションをtrue
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.SKILL);
        }
        //弱右攻撃入力時に攻撃enumに変更
        else if (isAttackWeak.down)
        {

            //スタミナの消費、消費量以下のスタミナなら処理しない
            if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().attackWeakStamina)) { return; }

            //攻撃Flagをtrue
            _isAtack = true;
            //コンボ可能
            _isCombo = true;
            //攻撃enumに遷移
            _motionEnum = MotionEnum.ATTACK;
            //アニメーションModeを変更
            _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
            //弱攻撃アニメーションをtrue
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.ATTACK_WEAK);
        }
        //強右攻撃入力時に攻撃enumに変更
        else if (isAttackStrong.down)
        {

            //スタミナの消費、消費量以下のスタミナなら処理しない
            if (!_playerManager.StaminaControl(PlayerState.StaminaEnum.USE, _playerData.StuminaPlayerData().attackStrongStamina)) { return; }

            //攻撃Flagをtrue
            _isAtack = true;
            //コンボ可能
            _isCombo = true;
            //攻撃enumに遷移
            _motionEnum = MotionEnum.ATTACK;
            //アニメーションModeを変更
            _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.ANIMATE_PHYSICS);
            //強攻撃アニメーションをtrue
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.ATTACK_STRONG);
        }
    }
    /// <summary>
    /// アクションの終了時に、待機か移動に遷移
    /// </summary>
    protected void EndAction()
    {

        //移動入力していて、走り中なら走りenum
        if (_isRun) { _motionEnum = MotionEnum.RUN; _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.RUN); }
        //移動入力しているなら移動enum
        else if (_isMove) { _motionEnum = MotionEnum.WALK; _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.WALK); }
        //移動入力してないなら待機enum
        else { _motionEnum = MotionEnum.IDLE; _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.IDLE); }

        //アニメーションModeを変更
        _animData.ChangeAnimatorApplyMode(_anim, PlayerAnimData_S.AnimatorUpdateModeEnum.NOMAL);
    }
    /// <summary>
    /// 全てを初期化
    /// </summary>
    private void ALLReset()
    {


        StopCoroutine("EndAttack");
        StopCoroutine("StartRun");
        StopCoroutine("EndRun");

        _isMove = false;//移動中か
        _isRun = false;//走り中か
        _isAction = false;//回避中か
        _isAtack = false;//攻撃中か
        _isCombo = false;//コンボできるか
        _isAttackCoroutine = false;//攻撃コルーチンが動いているか

        //判定の有無,攻撃力の変更
        for (int i = 0; i < _playerWeponColliders.Count; i++) { _playerWeponColliders[i].ChangeCollider(false, 0); }
    }


    //コルーチン---------------------------------------------------------------------------------------------
    public IEnumerator EndAttack()
    {

        //待機
        yield return new WaitForSeconds(_playerData.AttackPlayerData().attackInterval);

        //コンボフラグ終了
        _isCombo = false;

        //アクションの終了時に、待機か移動に遷移
        EndAction();
    }
    private IEnumerator StartRun()
    {

        //待機
        yield return new WaitForSeconds(_playerData.MovePlayerData().runInterval);

        //走りenumに遷移
        _motionEnum = MotionEnum.RUN;
        //走りアニメーションをtrue
        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.RUN);
    }
    private IEnumerator EndRun()
    {

        //待機
        yield return new WaitForSeconds(_playerData.MovePlayerData().runInterval);

        //歩きenumに遷移
        _motionEnum = MotionEnum.WALK;
        //歩きアニメーションをtrue
        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.WALK);
    }
    private IEnumerator EndMove()
    {

        //待機
        yield return new WaitForSeconds(_playerData.MovePlayerData().moveInterval);

        //歩きか走りなら
        if(_motionEnum == MotionEnum.WALK || _motionEnum == MotionEnum.RUN)
        {

            //歩きenumに遷移
            _motionEnum = MotionEnum.IDLE;
            //歩きアニメーションをtrue
            _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.IDLE);
        }
    }

    private IEnumerator EndDamage(float interval)
    {

        //待機
        yield return new WaitForSeconds(interval);

        //アクションの終了時に、待機か移動に遷移
        EndAction();
    }
    private IEnumerator EndDown(float interval)
    {

        //待機
        yield return new WaitForSeconds(interval);

        //ダウンアニメーションをtrue
        _animData.ChangeBoolAnimation(_anim, PlayerAnimData_S.AnimBoolMode.NULL);
    }
}
