using System.Collections;
using UnityEngine;



public class GimmickArtilleryBall : GimmickState
{

    [Header("�R���|�[�l���g�֘A")]
    private Rigidbody _rb = default;//Rigidbody
    private SphereCollider _colllider = default;//CapsuleCollider
    private MeshRenderer _meshRenderer = default;//MeshRenderer
    private AudioManager _audioManager = default;//�I�[�f�B�I�Ǘ��N���X


    [Header("�A�N�e�B�u�֘A")]
    [SerializeField, Tooltip("������܂ł̎���")] private float _activeInterval = default;
    [SerializeField, Tooltip("�g���̃R���C�_�[")] private float _scaleMax = default;
    [SerializeField, Tooltip("����Effect")] private GameObject _exObj = default;
    private float _scaleDef = default;//�f�t�H���g�T�C�Y�̃R���C�_�[


    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected override void Start()
    {

        //���
        base.Start();


        //�R���|�[�l���g�擾
        _rb = this.gameObject.GetComponent<Rigidbody>();
        _colllider = this.gameObject.GetComponent<SphereCollider>();
        _meshRenderer = this.gameObject.GetComponent<MeshRenderer>();

        //�f�t�H���g�T�C�Y�̃R���C�_�[��⊮
        _scaleDef = _colllider.radius;
        //����Effect���A�N�e�B�u
        _exObj.SetActive(false);
    }



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// AudioManager���Z�b�g
    /// </summary>
    /// <param name="audioManager"></param>
    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }


    private void OnTriggerEnter(Collider other)
    {

        try
        {

            switch (_motionEnum)
            {

                case MotionEnum.NOMAL:

                    //���������I��
                    _rb.isKinematic = true;
                    //�g���̃R���C�_�[
                    _colllider.radius = _scaleMax;
                    //�`�������
                    _meshRenderer.enabled = false;

                    //����Effect���A�N�e�B�u
                    if (_exObj) { _exObj.SetActive(true); }

                    //������
                    if (_audioManager) { _audioManager.StartAudio(2, 1); }
                    //����Ƀ_���[�W
                    Damage(_maxHp);
                    break;


                case MotionEnum.DAMAGE:
                    break;
                case MotionEnum.DOWN_WEAK:
                    break;
                case MotionEnum.DOWN_STRONG:
                    break;


                case MotionEnum.DEATH:

                    //�ՓˑΏۂ��v���C���[�Ȃ�U��
                    if (other.gameObject.tag == _playerTag)
                    {

                        print("��C��" + other.gameObject + "��" + _attack + "�̃_���[�W");
                        _playerManager.GetComponent<CharacterState>().Damage(_attack);
                    }
                    else
                    {

                        print("��C��" + other.gameObject + "��" + _attack + "�̃_���[�W");
                        if (other.gameObject.GetComponent<CharacterState>()) { other.gameObject.GetComponent<CharacterState>().Damage(_attack); }
                    }

                    //���Ԍo�ߌ��A�N�e�B�u
                    StartCoroutine("ActiveInterval");
                    break;


                default:
                    break;
            }
        }
        catch { print("GimmickArtilleryBall�ŃG���["); }
    }

    /// <summary>
    /// ���Ԍo�ߌ��A�N�e�B�u
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActiveInterval()
    {

        //�ҋ@
        yield return new WaitForSeconds(_activeInterval);

        try
        {

            //���������J�n
            _rb.isKinematic = false;
            //�f�t�H���g�T�C�Y�̃R���C�_�[
            _colllider.radius = _scaleDef;
            //�`�������
            _meshRenderer.enabled = true;

            //�X�e�[�^�X�̏�����
            ResetState();

            //����Effect���A�N�e�B�u
            _exObj.SetActive(false);

            //���̃I�u�W�F�N�g���A�N�e�B�u
            this.gameObject.SetActive(false);
        }
        catch { print("GimmickArtilleryBall�ŃG���["); }
    }
}
