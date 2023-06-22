using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ���锠�̊Ǘ��N���X
/// </summary>
public class GimmickCrashBox : GimmickState
{

    [Header("�Q�Ɗ֘A")]
    [SerializeField, Tooltip("�I�[�f�B�I�Ǘ��N���X")] private AudioManager _audioManager = default;

    [Header("���̃R���|�[�l���g�֘A")]
    private Rigidbody _thisRb = default;//�{�̂�Rigidbody
    private Vector3 _thisPos = default;//�{�̂̏����ʒu
    private Quaternion _thisRotate = default;//�{�̂̏����p�x
    private GameObject _boxChild = default;//�q�I�u�W�F�N�g
    private Collider _boxCollider = default;//BoxCollider
    private MeshRenderer _meshRenderer = default;//MeshRenderer

    [Header("��ꂽ���̃R���|�[�l���g�֘A")]
    private List<GameObject> _crachBoxObj = new List<GameObject>();//��ꂽ�{�b�N�X�̃I�u�W�F�N�g
    private List<Collider> _crachBoxCollider = new List<Collider>();//��ꂽ�{�b�N�X�̃R���C�_�[
    private List<Rigidbody> _crachBoxRb = new List<Rigidbody>();//��ꂽ�{�b�N�X�̕���
    private List<Vector3> _crachBoxPos = new List<Vector3>();//��ꂽ�{�b�N�X�̏����ʒu
    private List<Quaternion> _crachBoxRotate = new List<Quaternion>();//��ꂽ�{�b�N�X�̏����p�x

    [Header("�X�e�[�^�X�֘A")]
    [SerializeField, Tooltip("���ɖ߂�܂ł̃C���^�[�o��"), Range(1f, 10f)] public int _reverseInterval;
    [SerializeField, Tooltip("�j�Ђ̕������Ȃ����܂ł̃C���^�[�o��"), Range(0.1f, 10f)] public float _falseRbInterval;
    private bool _isCrach = default;//���Ă���Œ�



    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected override void Start()
    {

        //���
        base.Start();
        try
        {

            //�R���|�[�l���g�擾
            _boxChild = gameObject.transform.GetChild(0).gameObject;//�q�I�u�W�F�N�g
            _boxCollider = gameObject.GetComponent<Collider>();//Collider
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();//MeshRenderer

            //�{��
            _thisRb = gameObject.GetComponent<Rigidbody>();//Rigidbody
            _thisPos = gameObject.transform.position;//�����ʒu
            _thisRotate = gameObject.transform.rotation;//�����p�x

            //��ꂽ�Ƃ��̔j�ЃI�u�W�F�N�g�̐����擾
            int childCount = _boxChild.transform.childCount;
            //��ꂽ�Ƃ��̔j�ЃI�u�W�F�N�g�����Ɏ擾����
            for (int j = 0; j < childCount; j++)
            {

                //�S�Ċi�[
                GameObject childObject = _boxChild.transform.GetChild(j).gameObject;//��ꂽ�Ƃ��̔j��
                _crachBoxObj.Add(childObject);//��ꂽ�Ƃ��̔j�Ђ̃I�u�W�F�N�g
                _crachBoxCollider.Add(childObject.GetComponent<Collider>());//��ꂽ�{�b�N�X�̃R���C�_�[
                _crachBoxRb.Add(childObject.GetComponent<Rigidbody>());//��ꂽ�{�b�N�X�̕���
                _crachBoxPos.Add(childObject.transform.position);//��ꂽ�Ƃ��̔j�Ђ̍��W
                _crachBoxRotate.Add(childObject.transform.rotation);//��ꂽ�Ƃ��̔j�Ђ̊p�x
            }
        }
        catch { print("GimmickCrashBoxController�ŃG���["); }
    }


    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    public override void Damage(int attack)
    {

        //���Ă���Œ��͏������Ȃ�
        if (_isCrach) { return; }

        //���
         base.Damage(attack);


        if (_motionEnum == MotionEnum.DEATH)
        {

            //�{�̂̕����I��
            _thisRb.isKinematic = true;

            //�`��A���������
            _boxCollider.enabled = false;
            _meshRenderer.enabled = false;
            //���Ă���Œ�
            _isCrach = true;

            //����{�b�N�X��\��
            _boxChild.SetActive(true);
            //�j��I�[�f�B�I
            _audioManager.StartAudio(1, 0);

            //�j�Ђ̕������Ȃ����܂ł̃C���^�[�o��
            StartCoroutine("FalseRb");
            //���ɖ߂�܂ł̃C���^�[�o���J�n
            StartCoroutine("ReverseBox");
        }
    }

    /* [ �R���[�`���֘A ] */
    /// <summary>
    /// �j�Ђ̕������Ȃ����܂ł̃C���^�[�o��
    /// </summary>
    private IEnumerator FalseRb()
    {

        //�ҋ@
        yield return new WaitForSeconds(_falseRbInterval);

        try
        {

            for (int i = 0; i < _crachBoxObj.Count; i++)
            {

                //�R���C�_�[���A�N�e�B�u
                _crachBoxCollider[i].enabled = false;
                //�����������A�N�e�B�u
                _crachBoxRb[i].isKinematic = true;
            }
        }
        catch { print("GimmickCrashBoxController�ŃG���["); }
    }
    /// <summary>
    /// ���ɖ߂�܂ł̃C���^�[�o��
    /// </summary>
    private IEnumerator ReverseBox()
    {

        //�ҋ@
        yield return new WaitForSeconds(_reverseInterval);

        try
        {

            //�{�̂̏����ݒ�
            gameObject.transform.position = _thisPos;
            gameObject.transform.rotation = _thisRotate;

            for (int i = 0; i < _crachBoxObj.Count; i++)
            {

                //���W�p�x�����ɖ߂�
                _crachBoxObj[i].transform.position = _crachBoxPos[i];
                _crachBoxObj[i].transform.rotation = _crachBoxRotate[i];
                //�R���C�_�[���A�N�e�B�u
                _crachBoxCollider[i].enabled = true;
                //�����������A�N�e�B�u
                _crachBoxRb[i].isKinematic = false;
            }

            //�{�̂̏����ݒ�
            _thisRb.isKinematic = false;

            //����{�b�N�X��\��
            _boxChild.SetActive(false);

            //�`��A���������
            _boxCollider.enabled = true;
            _meshRenderer.enabled = true;
            //���Ă� �Ȃ�
            _isCrach = false;

            //�X�e�[�^�X�̏�����
            ResetState();
        }
        catch { print("GimmickCrashBoxController�ŃG���["); }
    }
}
