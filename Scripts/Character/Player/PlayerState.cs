using UnityEngine;


/// <summary>
/// Player�̊��N���X
/// </summary>
public class PlayerState : CharacterState
{


    [Header("�Q�Ɗ֘A")]
    [SerializeField, Tooltip("Canvas�Ǘ��N���X")] private CanvasManager _canvasManager;

    [Header("�X�e�[�^�X�֘A(PlayerState)")]
    [SerializeField, Tooltip("�X�^�~�i"), Range(1, 10000)] private int _maxStamina;
    private int _nowUseStamina = default;//�l�ύX�p�̃X�^�~�i
    private bool _isStumina = default;//�X�^�~�i�̎g�p�󋵕ԋp�l

    public enum StaminaEnum
    {
        [InspectorName("����")] USE,
        [InspectorName("��")] HEALING,
    }



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �X�^�~�i�̎g�p�Ɖ�
    /// </summary>
    public virtual bool StaminaControl(StaminaEnum staminaEnum, int stamina)
    {

        try
        {

            //����
            if (staminaEnum == StaminaEnum.USE)
            {

                //0�ȉ��Ȃ�A�l��0�ŌŒ肵��false�ԋp
                if (_nowUseStamina >= _maxStamina) { _nowUseStamina = _maxStamina; _isStumina = false; }
                //�g�p�ʂ����݂̃X�^�~�i�ȏ�Ȃ�false�ԋp
                else if (stamina > _maxStamina - _nowUseStamina) { _isStumina = false; }
                //�X�^�~�i�������true�ԋp
                else { _nowUseStamina += stamina; _isStumina = true; }
            }
            //��
            else if (staminaEnum == StaminaEnum.HEALING)
            {

                //�ő�l�ȏ�Ȃ�A�l���ő�l�ŌŒ肵��false�ԋp
                if (_nowUseStamina <= 0) { _nowUseStamina = 0; _isStumina = false; }
                //�X�^�~�i���񕜂���true�ԋp
                else { _nowUseStamina -= stamina; _isStumina = true; }
            }

            //�X�^�~�iUI�ɔ��f
            _canvasManager.StaminaSlider(_maxStamina, _maxStamina - _nowUseStamina);

            //�X�^�~�i�̎g�p�󋵕ԋp�l
            return _isStumina;

        }
        catch { print("PlayerState�ŃG���["); return false; }
    }


    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public override void Damage(int attack)
    {

        //���
        base.Damage(attack);

        try
        {

            //HpUI�ɔ��f
            _canvasManager.HpSlider(_maxHp, _nowHp);
            //���SUI
            if (_motionEnum == MotionEnum.DEATH) { _canvasManager.StartDiedUI(); }
        }
        catch { print("PlayerState�ŃG���["); }
    }
}


















