using UnityEngine;



/// <summary>
/// プレイヤー・敵・ギミックの基底クラス
/// </summary>
public class CharacterState : MonoBehaviour
{

    [Header("ステータス関連(CharacterState)")]
    [SerializeField, Tooltip("体力"),Range(1,10000)] protected int _maxHp;
    [SerializeField, Tooltip("ダウン値"), Range(1, 10000)] protected int _downDamage;
    protected int _nowHp = default;//現在のHp
    protected int _nowDownDamage = default;//現在のダウン値

    public enum MotionEnum
    {

        [InspectorName("通常")]NOMAL,
        [InspectorName("ダメージ")] DAMAGE,
        [InspectorName("ダウン")] DOWN_WEAK,
        [InspectorName("ダウン")] DOWN_STRONG,
        [InspectorName("死亡")]DEATH,
    }
    public MotionEnum _motionEnum;




    protected virtual void Start()
    {

        //ステータスの初期化
        ResetState();
    }



    //メソッド部----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public virtual void Damage(int attack)
    {

        //現在の状態enumを初期化
        _motionEnum = MotionEnum.NOMAL;

        //現在のHpを減算
        _nowHp -= attack;
        //現在のダウン値の減算
        _nowDownDamage -= attack;

        //現在のHpが0以下で死亡enum
        if (_nowHp <= 0) { _motionEnum = MotionEnum.DEATH; }
        else if(_downDamage <= attack) { _motionEnum = MotionEnum.DOWN_STRONG; _nowDownDamage = _downDamage; }
        //現在のダウン値が0以下でダウンenum
        else if (_nowDownDamage <= 0) { _motionEnum = MotionEnum.DOWN_WEAK; _nowDownDamage = _downDamage; }
        //上記以外ならダメージenum
        else { _motionEnum = MotionEnum.DAMAGE; }
    }
    /// <summary>
    /// ステータスの初期化
    /// </summary>
    protected virtual void ResetState()
    {

        //現在のHpを初期化
        _nowHp = _maxHp;
        //現在のダウン値を初期化
        _nowDownDamage = _downDamage;
        //MotionEnumを初期化
        _motionEnum = MotionEnum.NOMAL;
    }
}
