using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;



public class GimmickSignboardController : MonoBehaviour
{

    [Header("�Q�Ɗ֘A")]
    [SerializeField, Tooltip("�e�L�X�g�w�i")] private GameObject _textBackImage;
    [SerializeField, Tooltip("�e�L�X�g�\���L�����o�X")] private Text _text;


    [Header("�Ŕ̃p�b�P�[�W�֘A")]
    [SerializeField] private List<SignboardPackage> _signboardPackage = new List<SignboardPackage>();
    [Serializable, Tooltip("�Ŕ̃p�b�P�[�W")]
    public class SignboardPackage
    {
        [field: SerializeField, Tooltip("�Ŕ̃I�u�W�F�N�g")] private GameObject _signboardObj;
        [field: SerializeField, Tooltip("�\������e�L�X�g"), TextArea(1, 2)] private string _viewText;


        /// <summary>
        /// �Ŕ̃I�u�W�F�N�g
        /// </summary>
        public GameObject SignboardObj { get { return _signboardObj; } }
        /// <summary>
        /// �\������e�L�X�g
        /// </summary>
        public string ViewText { get { return _viewText; } }
    }
    private SignboardPackage _signboard = default;//�g�p����SignboardPackage



    private void Start()
    {

        //�e�L�X�g�w�i�̔�\��
        _textBackImage.SetActive(false);

        for (int i = 0; i < _signboardPackage.Count; i++)
        {

            //�Ŕ̊Ǘ��N���X���擾
            GimmickSignboard gimmickSignboard = _signboardPackage[i].SignboardObj.GetComponent<GimmickSignboard>();

            //���̃N���X
            gimmickSignboard.SignboardController = this.gameObject.GetComponent<GimmickSignboardController>();
            //�e�X�̔��ԍ�
            gimmickSignboard.SignboardNumber = i;
        }
    }



    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �e�L�X�g�̕\��
    /// </summary>
    public void ViewText(bool isView,int number)
    {

        //�g�p����SignboardPackage���X�V
        _signboard = _signboardPackage[number];

        //�e�L�X�g�̕\���E�e�L�X�g�̐ݒ�
        if (isView) { _textBackImage.SetActive(true); _text.text = _signboard.ViewText; }
        //�e�L�X�g�̔�\��
        else { _textBackImage.SetActive(false); }
    }
}
