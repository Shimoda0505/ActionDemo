using UnityEngine;

public class GimmickState : CharacterState
{

    [Header("ステータス関連(EnemyState)")]
    [SerializeField, Tooltip("攻撃力")] protected int _attack;
    protected const string _playerTag = "Player";//プレイヤーのタグ
    private const string _playerManagerTag = "PlayerManager";//PlayerManagerのタグ
    protected PlayerManager _playerManager = default;//CharacterStateクラス



    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected override void Start()
    {

        //基底
        base.Start();

        //PlayerManagerクラスを取得
        _playerManager = GameObject.FindWithTag(_playerManagerTag).GetComponent<PlayerManager>();
    }

}
