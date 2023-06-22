using UnityEngine;



/// <summary>
/// �v���C���[�̃A�j���[�V�����֘A�Ǘ��N���X
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{

    [Header("�Q�Ɗ֘A")]
    [SerializeField, Tooltip("���Ǘ��N���X")] private AudioManager _audioManager = default;
    private PlayerController _playerController;//�Q�Ɛ��PlayerController



    //�O���Q��----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �Q�Ɛ��PlayerController
    /// </summary>
    public PlayerController NewPlayerController { set { _playerController = value; } }



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /* [ Animation��Events ] */
    /// <summary>
    /// �A�N�V����Flag��false
    /// Animation��Events�Ŏg�p
    /// </summary>
    public void EndActionAnim() { _playerController.EndActionAnim(); }
    /// <summary>
    /// �U�������̃t���O�𔽓]������
    /// Animation��Events�Ŏg�p
    /// </summary>
    public void ChangeNowAttack() { _playerController.NowAttackAnim(); }
    /// <summary>
    /// �R���{�Ȃ�
    /// </summary>
    public void EndComboAnim() { _playerController.EndComboAnim(); }
    /// <summary>
    /// �ړ�����se
    /// </summary>
    public void StartMove(int number)
    {

        //AudioManager��null�Ȃ珈�����Ȃ�
        if (!_audioManager) { return; }

        //��������
        if (number == 0) { _audioManager.StartAudio(3, 0); }
        //���鑫��
        else if (number == 1) { _audioManager.StartAudio(3, 1); }
    }
    /// <summary>
    /// �U������̓���ւ�
    /// </summary>
    public void AttackCollider(int isCollider) { _playerController.AttackCollider(isCollider); }
}
