using UnityEngine;


/// <summary>
/// Playerの基底クラス
/// </summary>
public class PlayerState : CharacterState
{


    [Header("参照関連")]
    [SerializeField, Tooltip("Canvas管理クラス")] private CanvasManager _canvasManager;

    [Header("ステータス関連(PlayerState)")]
    [SerializeField, Tooltip("スタミナ"), Range(1, 10000)] private int _maxStamina;
    private int _nowUseStamina = default;//値変更用のスタミナ
    private bool _isStumina = default;//スタミナの使用状況返却値

    public enum StaminaEnum
    {
        [InspectorName("消費")] USE,
        [InspectorName("回復")] HEALING,
    }



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// スタミナの使用と回復
    /// </summary>
    public virtual bool StaminaControl(StaminaEnum staminaEnum, int stamina)
    {

        try
        {

            //消費
            if (staminaEnum == StaminaEnum.USE)
            {

                //0以下なら、値を0で固定してfalse返却
                if (_nowUseStamina >= _maxStamina) { _nowUseStamina = _maxStamina; _isStumina = false; }
                //使用量が現在のスタミナ以上ならfalse返却
                else if (stamina > _maxStamina - _nowUseStamina) { _isStumina = false; }
                //スタミナを消費してtrue返却
                else { _nowUseStamina += stamina; _isStumina = true; }
            }
            //回復
            else if (staminaEnum == StaminaEnum.HEALING)
            {

                //最大値以上なら、値を最大値で固定してfalse返却
                if (_nowUseStamina <= 0) { _nowUseStamina = 0; _isStumina = false; }
                //スタミナを回復してtrue返却
                else { _nowUseStamina -= stamina; _isStumina = true; }
            }

            //スタミナUIに反映
            _canvasManager.StaminaSlider(_maxStamina, _maxStamina - _nowUseStamina);

            //スタミナの使用状況返却値
            return _isStumina;

        }
        catch { print("PlayerStateでエラー"); return false; }
    }


    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public override void Damage(int attack)
    {

        //基底
        base.Damage(attack);

        try
        {

            //HpUIに反映
            _canvasManager.HpSlider(_maxHp, _nowHp);
            //死亡UI
            if (_motionEnum == MotionEnum.DEATH) { _canvasManager.StartDiedUI(); }
        }
        catch { print("PlayerStateでエラー"); }
    }
}


















