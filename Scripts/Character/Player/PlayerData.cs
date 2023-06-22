using UnityEngine;


/// <summary>
/// プレイヤーに関するデータを格納するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "PlayerData_", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{

    [Header("コンポーネント関連")]
    [SerializeField, Tooltip("アニメーター")] private RuntimeAnimatorController _anim; 


    [Header("移動関連")]
    [SerializeField, Tooltip("歩き速度"), Range(1f, 10f)] private float _walkSpeed;
    [SerializeField, Tooltip("走り速度"), Range(1f, 10f)] private float _runSpeed;
    [SerializeField, Tooltip("走り終了インターバル"), Range(0.1f, 1f)] private float _runInterval;
    [SerializeField, Tooltip("移動終了インターバル"), Range(0.1f, 1f)] private float _moveInterval;


    [Header("攻撃関連")]
    [SerializeField, Tooltip("弱攻撃力"), Range(1, 1000)] private int _attackWeak;
    [SerializeField, Tooltip("強攻撃力"), Range(1, 1000)] private int _attackStrong;
    [SerializeField, Tooltip("攻撃アニメーション終了後、入力を受け付ける時間"), Range(0.1f, 1f)] private float _attackInterval;


    [Header("武器関連")]
    [SerializeField, Tooltip("抜刀時間"), Range(0.1f, 5f)] private float _equipInterval;
    [SerializeField, Tooltip("納刀時間"), Range(0.1f, 5f)] private float _unequipInterval;


    [Header("スタミナ関連")]
    [SerializeField, Tooltip("弱攻撃"), Range(1, 1000)] private int _attackWeakStamina;
    [SerializeField, Tooltip("強攻撃"), Range(1, 1000)] private int _attackStrongStamina;
    [SerializeField, Tooltip("スキル"), Range(1, 1000)] private int _skillStamina;
    [SerializeField, Tooltip("回避"), Range(1, 1000)] private int _rollStamina;
    [SerializeField, Tooltip("ステップ"), Range(1, 1000)] private int _stepStamina;
    [SerializeField, Tooltip("走り速度"), Range(1, 10)] private int _runSpeedStamina;
    [SerializeField, Tooltip("回復速度"), Range(1, 10)] private int _healingSpeedStamina;


    [Header("ダメージ関連")]
    [SerializeField, Tooltip("ダメージ後インターバル時間"), Range(0.1f, 10f)] private float _damageInterval;
    [SerializeField, Tooltip("軽ダウン後インターバル時間"), Range(0.1f, 10f)] private float _downWeakInterval;
    [SerializeField, Tooltip("大ダウン後インターバル時間"), Range(0.1f, 10f)] private float _downStrongInterval;




    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// コンポーネント関連
    /// </summary>
    public RuntimeAnimatorController ComponentPlayerData() { return _anim; }
    /// <summary>
    /// 移動関連
    /// </summary>
    public (float walkSpeed,float runSpeed, float runInterval, float moveInterval) MovePlayerData() { return (_walkSpeed, _runSpeed, _runInterval, _moveInterval); }
    /// <summary>
    /// 攻撃関連
    /// </summary>
    public (int _attackWeak, int _attackStrong, float attackInterval) AttackPlayerData() { return (_attackWeak, _attackStrong, _attackInterval); }
    /// <summary>
    /// 武器関連
    /// </summary>
    public (float equipInterval, float unequipInterval) EquipPlayerData() { return (_equipInterval, _unequipInterval); }
    /// <summary>
    /// スタミナ(固定)関連
    /// </summary>
    public (int attackStrongStamina,int attackWeakStamina,int skillStamina, int rollStamina,int stepStamina) StuminaPlayerData()
                                    { return (_attackStrongStamina, _attackWeakStamina, _skillStamina, _rollStamina, _stepStamina); }
    /// <summary>
    /// スタミナ(時間)関連
    /// </summary>
    /// <returns></returns>
    public (int runSpeedStamina,int healingSpeedStamina) StuminaSpeedPlayerData() { return (_runSpeedStamina, _healingSpeedStamina); }
    /// <summary>
    /// ダメージ(時間)関連
    /// </summary>
    /// <returns></returns>
    public (float damageInterval, float downWeakInterval, float downStrongInterval) DamagePlayerData() { return (_damageInterval, _downWeakInterval, _downStrongInterval); }
}