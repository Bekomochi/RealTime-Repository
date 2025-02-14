using DG.Tweening;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRoomManager : MonoBehaviour
{
    [SerializeField] GameObject[] characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] WarterGunManager warterGunManager;
    [SerializeField] Transform[] initPosList;
    [SerializeField] int CountNum;
    [SerializeField] private ParticleSystem warterParticle;

    GameDirector gameDirector;
    public Text CountDownText; //�X�^�[�g�܂ł̃J�E���g�_�E���p�̃e�L�X�g(3�J�E���g)
    public Text StartText; //�J�n�p�e�L�X�g([Start!!])
    public GameObject FinishButton;//�I���p���{�^��

    //�T�E���h�Đ��p
    AudioSource audioSource;

    //�ڑ�ID���L�[�ɂ��āA�L�����N�^�[�̃I�u�W�F�N�g���Ǘ�
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //���f���ɓo�^����
        roomModel.OnJoinedUser += this.OnJoinedUser;//����
        roomModel.OnPreparationUser += this.OnPreparationUser;//��������
        roomModel.OnReadyGame += this.OnReadyGame; //�Q�[���J�n
        roomModel.OnFinishGame += this.OnFinishGame;//�Q�[���I��
        roomModel.OnMoveCharacter += OnMoveCharacter;//�ʒu����
        roomModel.OnShotWater += OnShotWater;

        /* Start�֐����ŁA�����������Ăяo���B
         * �����������ŃC���X�^���X����������Ă���̂ŁA�C���X�^���X�����͋L�����Ȃ��B
         */
        JoinRoom();//���L��JoinRoom���ŏ�����Ăяo��

        audioSource= GetComponent<AudioSource>();

        warterGunManager.GetComponent<WarterGunManager>();

        ////
        //CountDownText����̐ݒ�
        ////
        //CountNum = 3; //CountNum��������
        //CountDownText.text = CountNum.ToString(); //CountDownText�����Ԃɔ��f������
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void JoinRoom()
    {
        //����
        await roomModel.JoinAsync(GameDirector.RoomName, GameDirector.Id);
        /*���[�����ƃ��[�U�[ID��n���ē�������B
        // *�ŏI�I�ɂ́A�u���[�J���ɕۑ����ꂽUserID�v���w�肷��B
        // */
    }

    private void OnJoinedUser(JoinedUser user)
    {//����������Instantiate����
        GameObject characterObject = Instantiate(characterPrefab[user.JoinOrder]);//�C���X�^���X����
        characterObject.transform.position = initPosList[user.JoinOrder].position;

        characterList[user.ConnectionID] = characterObject;//�t�B�[���h�ŕێ�

        //���[�����f���̐ڑ�ID��user�̐ڑ�ID��������������
        if (roomModel.ConnectionId == user.ConnectionID)
        {
            //characterObject����Character��Get���āACharacter����bool�ϐ�isSelf��true�ɂ���
            characterObject.GetComponent<Character>().isSelf = true;

            //InvokeRepeating��MovedUserasync�����I�ɌĂяo���ď�Ԃ��X�V
            InvokeRepeating("MovedUserasync", 0.1f, 0.1f);
        }
    }

    public async void OnPreparationUser()
    {//�J�n�O�̃J�E���g
     //���̒���await�ŃJ�E���g��i�߂���//

        //�������������ꂽ��CountDownText��\�����āA�J�E���g�_�E�����Ă���
        CountDownText.gameObject.SetActive(true); //CountDownText��\��

        ////1�b���ƂɃJ�E���g�_�E��
        //InvokeRepeating("CountDown", 1, 1);
    }

    public async void OnReadyGame()
    {
        //StartText��\��������
        StartText.gameObject.SetActive(true); //�^�C�����O�������������ɒx�ꂽ���[�U�[���s���ɂȂ�̂ŁA�\��������^�C�~���O�����킹��

        //
        //�L�����N�^�[�𓮂������Ԃɂ���
        //

        //1000ms��(1�b��)��StartText���\���ɂ���ݒ�
        await Task.Delay(1000); //�ȉ��̏�����1�b��ɔ�\��/�\���Ƃ���
        StartText.gameObject.SetActive(false);�@//StartText���\���ɂ���

        //FinishBuuton��\������
        FinishButton.gameObject.SetActive(true);
    }

    public async Task CountDown()
    {//�J�E���g�_�E������֐�
        CountNum--;
        CountDownText.text = CountNum.ToString();

        if (CountNum == 0)
        {
            //�J�E���g�_�E�����~�߂�
            //CancelInvoke("");

            //CountDownText���\��
            CountDownText.gameObject.SetActive(false);

            //ReadyAsync���Ăяo��
            await roomModel.ReadyAsync();
        }
    }
    public void OnFinishGame()
    {//�Q�[���I��(�{�^���������ďI������͉̂��̓���)
        //���U���g��ʂɈړ�
        SceneManager.LoadScene("Result");
    }

    public async void FinishAsync()
    {//�I�����đގ�
        //FinishAsync���Ăяo��
        await roomModel.FinishAsync();
    }

    void OnMoveCharacter(MovedUser movedUser)
    {
        //characterList����Ώۂ�GameObject���擾�A�ʒu�E��]�𔽉f
        /* 2024/12/25�ύX
         * ���f�̍ہA�l�̑���ł͂Ȃ�DOLocalMove�ɕύX�B�������邱�ƂŁA�����ȊO�̉�ʂł����炩�ɓ����Č�����B
         * ���ۂɓ����X�s�[�h��0.6f�����A�����ȊO�̉�ʂ��ƒx���������̂ŁA0.3f�ɐݒ肵�Ă���B
         */
        characterList[movedUser.ConnectionID].gameObject.transform.DOLocalMove((movedUser.pos), 0.3f);
    }

    public async void MovedUserasync()
    {
        MovedUser movedUser = new MovedUser();
        movedUser.pos = characterList[roomModel.ConnectionId].gameObject.transform.position;
        movedUser.rot = characterList[roomModel.ConnectionId].gameObject.transform.rotation;
        movedUser.ConnectionID = roomModel.ConnectionId;

        //MoveAsync�Ăяo��
        await roomModel.MoveAsync(movedUser);
    }

    /// <summary>
    /// ���S�C���ˏ���
    /// </summary>

    public async void ShotAsync()
    {
        await roomModel.ShotAsync();
    }

    public void OnShotWater()
    {
        characterList[roomModel.ConnectionId].GetComponent<Character>().OnShotButton();
        warterGunManager.ShotWarter();
    }
}
