using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// PlayerController�̊Ǘ��N���X
/// Player�Ɋւ���قƂ�ǂ��Ǘ�
/// </summary>
public class PlayerManager : PlayerState
{

    [Header("�v���C���[�f�[�^�֘A")]
    [SerializeField, Tooltip("�v���[���[�I�u�W�F�N�g")] private GameObject _playerObj;
    [SerializeField, Tooltip("�v���[���[�̃A�j���[�V�����Ǘ��N���X")] private PlayerAnimationController _playerAnimationController;
    private PlayerController _playerController;//�g�p����PlayerController
    private int _playerNumber = 0;//�v���C���[�ԍ�
    private int _playerLastNumber = 0;//�Ō�̃v���C���[�ԍ��̕⊮


    [Header("�v���C���[�̃p�b�P�[�W�֘A")]
    public List<PlayerPackage> _playerPackages = new List<PlayerPackage>();
    [Serializable, Tooltip("�v���C���[�̃p�b�P�[�W")]
    public class PlayerPackage
    {
        [SerializeField, Tooltip("���O")] private string _name;
        [field: SerializeField, Tooltip("����Z�b�g����Ă��邩")] private bool _isWeponSet;
        [field: SerializeField, Tooltip("�v���C���[�̌ʃf�[�^")] private PlayerData _playerData;
        [field: SerializeField, Tooltip("�v���C���[�̌ʃN���X")] private PlayerController _playerController;
        [field: SerializeField, Tooltip("����f�[�^")] private GameObject[] _weapon;


        /// <summary>
        /// ����Z�b�g����Ă��邩
        /// </summary>
        public bool IsWeponSet { get { return _isWeponSet; } }
        /// <summary>
        /// �v���C���[�̌ʃf�[�^
        /// </summary>
        public PlayerData NowPlayerData { get { return _playerData; } }
        /// <summary>
        /// �v���C���[�̌ʃN���X
        /// </summary>
        public PlayerController NowPlayerController { get { return _playerController; } }
        /// <summary>
        /// ����f�[�^
        /// </summary>
        public GameObject[] Weapon { get { return _weapon; } }
    }


    [Header("�R���|�[�l���g�֘A")]
    private Transform _tr;//�v���C���[��Transform
    private Rigidbody _rb;//�v���C���[��Rigidbody
    private Animator _anim;//�v���C���[��Animator




    //������------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {

        try
        {

            //�R���|�[�l���g�擾
            if (_playerObj)
            {

                //Transform
                _tr = _playerObj.transform;
                //Rigidbody
                _rb = _playerObj.GetComponent<Rigidbody>();
                //Animator
                _anim = _playerObj.GetComponent<Animator>();
            }
            else { print("PlayerObj��null"); }


            //�S�Ă�PlayerController�ɏ����ݒ�
            for (int i = 0; i < _playerPackages.Count; i++)
            {

                //�ϐ��̎擾
                PlayerPackage playerPackage = _playerPackages[i];

                //�ݒ�
                //�R���|�[�l���g�֘A�̐ݒ�
                playerPackage.NowPlayerController.SetComponent(_tr, _rb, _anim);
                //PlayerData�̏����ݒ�
                playerPackage.NowPlayerController.SetPlayerData(playerPackage.NowPlayerData);
                //PlayerData�̏����ݒ�
                playerPackage.NowPlayerController.SetPlayerManager(this);
                //����̏����ݒ�
                for (int j = 0; j < playerPackage.Weapon.Length; j++)
                {

                    //����̔���I��
                    playerPackage.Weapon[j].GetComponent<PlayerWeponCollider>().ChangeCollider(false, 0);
                    //������\��
                    playerPackage.Weapon[j].SetActive(false);
                    //����̃R���C�_�[��⊮
                    playerPackage.NowPlayerController.SetPlayerWeponCollider(playerPackage.Weapon[j].GetComponent<PlayerWeponCollider>(), playerPackage.Weapon[j]);
                }
            }

            //������PlayerController
            _playerController = _playerPackages[_playerNumber].NowPlayerController;
            //PlayerAnimationEvents�N���X��PLayerController�̕ύX
            _playerAnimationController.NewPlayerController = _playerController;
            //������Animator
            _anim.runtimeAnimatorController = _playerPackages[_playerNumber].NowPlayerData.ComponentPlayerData();
            //�A�j���[�V�����̏�����
            _playerController.ResetAnim();
            //�����\��
            for (int j = 0; j < _playerPackages[_playerNumber].Weapon.Length; j++) { _playerPackages[_playerNumber].Weapon[j].SetActive(true); }
        }
        catch { print("PlayerManager�ŃG���["); }
    }



    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �v���C���[�ύX
    /// </summary>
    public void ChangePlayer(bool isDpadRightDown)
    {

        try
        {

            //�A�C�h�����ȊO�̎��������Ȃ�
            if (!_playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.IDLE)) { return; }

            if (isDpadRightDown)
            {

                //����̔�\��
                for (int j = 0; j < _playerPackages[_playerNumber].Weapon.Length; j++) { _playerPackages[_playerNumber].Weapon[j].SetActive(false); }

                //�Ō�̃v���C���[�ԍ��̕⊮
                _playerLastNumber = _playerNumber;

                //�g�p����v���C���[�̔ԍ�
                for (int i = _playerNumber + 1; i < _playerPackages.Count; i++)
                {

                    //����Z�b�g����Ă���΁A�v���C���[�ԍ��̕⊮
                    if (_playerPackages[i].IsWeponSet) { _playerNumber = i; break; }
                }
                //�ԍ����O��Ɠ����Ȃ�0�Ԃ��g�p
                if (_playerLastNumber == _playerNumber) { _playerNumber = 0; }

                //����̕\��
                for (int j = 0; j < _playerPackages[_playerNumber].Weapon.Length; j++) { _playerPackages[_playerNumber].Weapon[j].SetActive(true); }

                //PlayerController�̕ύX
                _playerController = _playerPackages[_playerNumber].NowPlayerController;
                //�ړ������̎󂯓n��
                _playerController.SetMoveForward(_playerPackages[_playerLastNumber].NowPlayerController.GetMoveForward());
                //PlayerAnimationEvents�N���X��PLayerController�̕ύX
                _playerAnimationController.NewPlayerController = _playerController;
                //Animator�̕ύX
                _anim.runtimeAnimatorController = _playerPackages[_playerNumber].NowPlayerData.ComponentPlayerData();
                //�A�j���[�V�����̏�����
                _playerController.ResetAnim();
            }
        }
        catch { print("PlayerManager�ŃG���["); }
    }
    /// <summary>
    /// ���݂�PlayerController
    /// </summary>
    public PlayerController PlayerController() { return _playerController; }
    /// <summary>
    /// �_���[�W����
    /// </summary>
    public override void Damage(int attack)
    {

        //��𒆂̎��������Ȃ�
        if (_playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.ROLL) ||
            _playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.DAMAGE) ||
            _playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.DOWN_STRONG) ||
            _playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.DOWN_WEAK) ||
            _playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.DEATH)) { return; }


        //���
        base.Damage(attack);

        //���Senum
        if (_motionEnum == MotionEnum.DEATH) { _playerController.SetMotionEnum(global::PlayerController.MotionEnum.DEATH); }
        //��_�E��enum
        else if (_motionEnum == MotionEnum.DOWN_STRONG) { _playerController.SetMotionEnum(global::PlayerController.MotionEnum.DOWN_STRONG); }
        //�y�_�E��enum
        else if (_motionEnum == MotionEnum.DOWN_WEAK) { _playerController.SetMotionEnum(global::PlayerController.MotionEnum.DOWN_WEAK); }
        //�_���[�Wenum
        else if(_motionEnum == MotionEnum.DAMAGE) { _playerController.SetMotionEnum(global::PlayerController.MotionEnum.DAMAGE); }

        //�ʏ�enum�ɑJ��
        _motionEnum = MotionEnum.NOMAL;
    }
}
