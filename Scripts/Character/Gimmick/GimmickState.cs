using UnityEngine;

public class GimmickState : CharacterState
{

    [Header("�X�e�[�^�X�֘A(EnemyState)")]
    [SerializeField, Tooltip("�U����")] protected int _attack;
    protected const string _playerTag = "Player";//�v���C���[�̃^�O
    private const string _playerManagerTag = "PlayerManager";//PlayerManager�̃^�O
    protected PlayerManager _playerManager = default;//CharacterState�N���X



    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected override void Start()
    {

        //���
        base.Start();

        //PlayerManager�N���X���擾
        _playerManager = GameObject.FindWithTag(_playerManagerTag).GetComponent<PlayerManager>();
    }

}
