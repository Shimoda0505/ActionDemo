using UnityEngine;



/// <summary>
/// �Ŕ̊Ǘ��N���X
/// </summary>
public class GimmickSignboard : MonoBehaviour
{

    private GimmickSignboardController _gimmickSignboardController;//GimmickSignboard�Ǘ��N���X
    private int _thisNumber = default;//���̊Ŕ̔z��ԍ�
    private string _playerTag = "Player";//�v���C���[�^�O



    //�O���Q��----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// GimmickSignboard�Ǘ��N���X
    /// </summary>
    public GimmickSignboardController SignboardController { set { _gimmickSignboardController = value; } }
    /// <summary>
    /// �Ŕ̔z��ԍ�
    /// </summary>
    public int SignboardNumber { set { _thisNumber = value; } }


    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {

        //�ՓˑΏۂ��v���C���[�Ȃ�U��
        if (other.gameObject.tag == _playerTag) { _gimmickSignboardController.ViewText(true, _thisNumber); }
    }

    private void OnTriggerExit(Collider other)
    {

        //�ՓˑΏۂ��v���C���[�Ȃ�U��
        if (other.gameObject.tag == _playerTag) { _gimmickSignboardController.ViewText(false, _thisNumber); }
    }
}
