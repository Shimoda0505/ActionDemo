using UnityEngine;



/// <summary>
/// プレイヤーのアニメーション関連管理クラス
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{

    [Header("参照関連")]
    [SerializeField, Tooltip("音管理クラス")] private AudioManager _audioManager = default;
    private PlayerController _playerController;//参照先のPlayerController



    //外部参照----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 参照先のPlayerController
    /// </summary>
    public PlayerController NewPlayerController { set { _playerController = value; } }



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /* [ AnimationのEvents ] */
    /// <summary>
    /// アクションFlagのfalse
    /// AnimationのEventsで使用
    /// </summary>
    public void EndActionAnim() { _playerController.EndActionAnim(); }
    /// <summary>
    /// 攻撃中かのフラグを反転させる
    /// AnimationのEventsで使用
    /// </summary>
    public void ChangeNowAttack() { _playerController.NowAttackAnim(); }
    /// <summary>
    /// コンボなし
    /// </summary>
    public void EndComboAnim() { _playerController.EndComboAnim(); }
    /// <summary>
    /// 移動中のse
    /// </summary>
    public void StartMove(int number)
    {

        //AudioManagerがnullなら処理しない
        if (!_audioManager) { return; }

        //歩く足音
        if (number == 0) { _audioManager.StartAudio(3, 0); }
        //走る足音
        else if (number == 1) { _audioManager.StartAudio(3, 1); }
    }
    /// <summary>
    /// 攻撃判定の入れ替え
    /// </summary>
    public void AttackCollider(int isCollider) { _playerController.AttackCollider(isCollider); }
}
