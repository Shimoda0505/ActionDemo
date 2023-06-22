using UnityEngine;


/// <summary>
/// �v���C���[�Ɋւ���f�[�^���i�[����ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "PlayerData_", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{

    [Header("�R���|�[�l���g�֘A")]
    [SerializeField, Tooltip("�A�j���[�^�[")] private RuntimeAnimatorController _anim; 


    [Header("�ړ��֘A")]
    [SerializeField, Tooltip("�������x"), Range(1f, 10f)] private float _walkSpeed;
    [SerializeField, Tooltip("���葬�x"), Range(1f, 10f)] private float _runSpeed;
    [SerializeField, Tooltip("����I���C���^�[�o��"), Range(0.1f, 1f)] private float _runInterval;
    [SerializeField, Tooltip("�ړ��I���C���^�[�o��"), Range(0.1f, 1f)] private float _moveInterval;


    [Header("�U���֘A")]
    [SerializeField, Tooltip("��U����"), Range(1, 1000)] private int _attackWeak;
    [SerializeField, Tooltip("���U����"), Range(1, 1000)] private int _attackStrong;
    [SerializeField, Tooltip("�U���A�j���[�V�����I����A���͂��󂯕t���鎞��"), Range(0.1f, 1f)] private float _attackInterval;


    [Header("����֘A")]
    [SerializeField, Tooltip("��������"), Range(0.1f, 5f)] private float _equipInterval;
    [SerializeField, Tooltip("�[������"), Range(0.1f, 5f)] private float _unequipInterval;


    [Header("�X�^�~�i�֘A")]
    [SerializeField, Tooltip("��U��"), Range(1, 1000)] private int _attackWeakStamina;
    [SerializeField, Tooltip("���U��"), Range(1, 1000)] private int _attackStrongStamina;
    [SerializeField, Tooltip("�X�L��"), Range(1, 1000)] private int _skillStamina;
    [SerializeField, Tooltip("���"), Range(1, 1000)] private int _rollStamina;
    [SerializeField, Tooltip("�X�e�b�v"), Range(1, 1000)] private int _stepStamina;
    [SerializeField, Tooltip("���葬�x"), Range(1, 10)] private int _runSpeedStamina;
    [SerializeField, Tooltip("�񕜑��x"), Range(1, 10)] private int _healingSpeedStamina;


    [Header("�_���[�W�֘A")]
    [SerializeField, Tooltip("�_���[�W��C���^�[�o������"), Range(0.1f, 10f)] private float _damageInterval;
    [SerializeField, Tooltip("�y�_�E����C���^�[�o������"), Range(0.1f, 10f)] private float _downWeakInterval;
    [SerializeField, Tooltip("��_�E����C���^�[�o������"), Range(0.1f, 10f)] private float _downStrongInterval;




    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �R���|�[�l���g�֘A
    /// </summary>
    public RuntimeAnimatorController ComponentPlayerData() { return _anim; }
    /// <summary>
    /// �ړ��֘A
    /// </summary>
    public (float walkSpeed,float runSpeed, float runInterval, float moveInterval) MovePlayerData() { return (_walkSpeed, _runSpeed, _runInterval, _moveInterval); }
    /// <summary>
    /// �U���֘A
    /// </summary>
    public (int _attackWeak, int _attackStrong, float attackInterval) AttackPlayerData() { return (_attackWeak, _attackStrong, _attackInterval); }
    /// <summary>
    /// ����֘A
    /// </summary>
    public (float equipInterval, float unequipInterval) EquipPlayerData() { return (_equipInterval, _unequipInterval); }
    /// <summary>
    /// �X�^�~�i(�Œ�)�֘A
    /// </summary>
    public (int attackStrongStamina,int attackWeakStamina,int skillStamina, int rollStamina,int stepStamina) StuminaPlayerData()
                                    { return (_attackStrongStamina, _attackWeakStamina, _skillStamina, _rollStamina, _stepStamina); }
    /// <summary>
    /// �X�^�~�i(����)�֘A
    /// </summary>
    /// <returns></returns>
    public (int runSpeedStamina,int healingSpeedStamina) StuminaSpeedPlayerData() { return (_runSpeedStamina, _healingSpeedStamina); }
    /// <summary>
    /// �_���[�W(����)�֘A
    /// </summary>
    /// <returns></returns>
    public (float damageInterval, float downWeakInterval, float downStrongInterval) DamagePlayerData() { return (_damageInterval, _downWeakInterval, _downStrongInterval); }
}