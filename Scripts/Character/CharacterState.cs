using UnityEngine;



/// <summary>
/// �v���C���[�E�G�E�M�~�b�N�̊��N���X
/// </summary>
public class CharacterState : MonoBehaviour
{

    [Header("�X�e�[�^�X�֘A(CharacterState)")]
    [SerializeField, Tooltip("�̗�"),Range(1,10000)] protected int _maxHp;
    [SerializeField, Tooltip("�_�E���l"), Range(1, 10000)] protected int _downDamage;
    protected int _nowHp = default;//���݂�Hp
    protected int _nowDownDamage = default;//���݂̃_�E���l

    public enum MotionEnum
    {

        [InspectorName("�ʏ�")]NOMAL,
        [InspectorName("�_���[�W")] DAMAGE,
        [InspectorName("�_�E��")] DOWN_WEAK,
        [InspectorName("�_�E��")] DOWN_STRONG,
        [InspectorName("���S")]DEATH,
    }
    public MotionEnum _motionEnum;




    protected virtual void Start()
    {

        //�X�e�[�^�X�̏�����
        ResetState();
    }



    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public virtual void Damage(int attack)
    {

        //���݂̏��enum��������
        _motionEnum = MotionEnum.NOMAL;

        //���݂�Hp�����Z
        _nowHp -= attack;
        //���݂̃_�E���l�̌��Z
        _nowDownDamage -= attack;

        //���݂�Hp��0�ȉ��Ŏ��Senum
        if (_nowHp <= 0) { _motionEnum = MotionEnum.DEATH; }
        else if(_downDamage <= attack) { _motionEnum = MotionEnum.DOWN_STRONG; _nowDownDamage = _downDamage; }
        //���݂̃_�E���l��0�ȉ��Ń_�E��enum
        else if (_nowDownDamage <= 0) { _motionEnum = MotionEnum.DOWN_WEAK; _nowDownDamage = _downDamage; }
        //��L�ȊO�Ȃ�_���[�Wenum
        else { _motionEnum = MotionEnum.DAMAGE; }
    }
    /// <summary>
    /// �X�e�[�^�X�̏�����
    /// </summary>
    protected virtual void ResetState()
    {

        //���݂�Hp��������
        _nowHp = _maxHp;
        //���݂̃_�E���l��������
        _nowDownDamage = _downDamage;
        //MotionEnum��������
        _motionEnum = MotionEnum.NOMAL;
    }
}
