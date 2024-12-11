using Server.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GameDirector : MonoBehaviour
{//�Q�[���i�s���Ǘ�����N���X

    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] int CountNum;
    [SerializeField] int MasterTimer;

    public Text CountDownText; //�X�^�[�g�܂ł̃J�E���g�_�E���p�̃e�L�X�g(3�J�E���g)
    public Text StartText; //�J�n�p�e�L�X�g([Start!!])
    public GameObject FinishButton;//�I���p���{�^��

    //�I�u�W�F�N�g�ƌ��т���
    public InputField IDinputField;

    //�ڑ�ID���L�[�ɂ��āA�L�����N�^�[�̃I�u�W�F�N�g���Ǘ�
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();

    // Start is called before the first frame update
    async void Start()
    {
        //�ŏ���CountDownText���\���ɂ���
        CountDownText.gameObject.SetActive(false);

        //�ŏ���StartText���\���ɂ���
        StartText.gameObject.SetActive(false);

        //�ŏ���FinishBuuton���\���ɂ���
        FinishButton.gameObject.SetActive(false);

        //CountDownText����̐ݒ�
        CountNum = 3; //CountNum��������
        CountDownText.text = CountNum.ToString(); //CountDownText�����Ԃɔ��f������

        //InvokeRepeating�ŁA1�b���Ƃ�CountDown�֐����Ăяo��

        //���f���ɓo�^����
        roomModel.OnJoinedUser += this.OnJoinedUser;//����
        roomModel.OnMatchingUser += this.OnMatchingUser;//�}�b�`���O
        roomModel.OnLeavedUser += this.OnLeavedUser;//�ގ�
        roomModel.OnMoveCharacter += OnMoveCharacter;//�ʒu����
        roomModel.OnPreparationUser += this.OnPreparationUser; //��������
        roomModel.OnReadyGame += this.OnReadyGame; //�Q�[���J�n
        roomModel.OnFinishGame += this.OnFinishGame;//�Q�[���I��

        //���[�U�[ID����͂�����̓t�B�[���h��GetComponent����
        IDinputField = IDinputField.GetComponent<InputField>();

        //�ڑ�
        await roomModel.ConnectAsync();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ��������
    /// </summary>

    public async void JoinRoom()
    {
        string IDtext = IDinputField.text;
        int.TryParse(IDtext, out int id);

        //����
        await roomModel.JoinAsync("Lobby", id);
        /*���[�����ƃ��[�U�[ID��n���ē�������B
         *���[�U�[ID�́AUI��inputfield�œ��͂ł���悤�ɂ������B
         *�ŏI�I�ɂ́A�u���[�J���ɕۑ����ꂽUserID�v���w�肷��B
         */
    }

    //���[�U�[�������������̏���
    private void OnJoinedUser(JoinedUser user)
    {//����������Instantiate����
        GameObject characterObject = Instantiate(characterPrefab);//�C���X�^���X����
        characterObject.transform.position = new Vector3(0, 0, 0);
        characterList[user.ConnectionID] = characterObject;//�t�B�[���h�ŕێ�

        if (roomModel.ConnectionId == user.ConnectionID)
        {
            //
            //InvokeRepeating�Œ���I�ɏ�Ԃ𑗐M
            //

            //characterObject����Character��Get���āACharacter����bool�ϐ�isSelf��true�ɂ���
            characterObject.GetComponent<Character>().isSelf = true;

            //InvokeRepeating��MovedUserasync�����I�ɌĂяo���ď�Ԃ��X�V
            InvokeRepeating("MovedUserasync", 0.1f, 0.1f);
        }
    }

    //�}�b�`���O

    public async void MatchingUser(string roomName,int userID)
    {
        await roomModel.MatchingAsync(roomName,userID);
        
    }

    public void OnMatchingUser(string roomName)
    {
        OnMatchingUser(roomName);
    }

    /// <summary>
    /// �ؒf�A�ގ�����
    /// </summary>

    //�ގ�
    public async void LeaveRoom()
    {
        //�ʒu�����̒���I���M���I������
        CancelInvoke("MovedUserasync");

        //�ގ�
        await roomModel.LeaveAsync();
    }

    //���[�U�[���ؒf�������̏���(�ؒf������Destroy)
    private void OnLeavedUser(LeavedUser user)
    {//�ގ�������Destroy����

        if (roomModel.ConnectionId == user.ConnectionID)
        {
            foreach (var charaList in characterList)
            {
                Destroy(charaList.Value);
            }
        }
        else
        {
            Destroy(characterList[user.ConnectionID]);
        }
    }


    /// <summary>
    /// �ʒu��������
    /// </summary>

    void OnMoveCharacter(MovedUser movedUser)
    {
        //characterList����Ώۂ�GameObject���擾�A�ΏۂɈʒu�E��]�𔽉f
        characterList[movedUser.ConnectionID].gameObject.transform.position = movedUser.pos;
        characterList[movedUser.ConnectionID].gameObject.transform.rotation = movedUser.rot;
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

    public async void OnPreparationUser()
    {//�J�n�O�̃J�E���g
     //���̒���await�ŃJ�E���g��i�߂���//

        //3�l�W�܂�����CountDownText��\�����āA�J�E���g�_�E�����Ă���
        CountDownText.gameObject.SetActive(true); //CountDownText��\��

        //1�b���ƂɃJ�E���g�_�E��
        InvokeRepeating("CountDown", 1, 1);
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
            CancelInvoke("CountDown");

            //CountDownText���\��
            CountDownText.gameObject.SetActive(false);

            //ReadyAsync���Ăяo��
            await roomModel.ReadyAsync();
        }
    }

    public void OnFinishGame()
    {//�Q�[���I��(�{�^���������ďI������̂̓A���t�@�ł̂�)
        //���U���g��ʂɈړ�
        SceneManager.LoadScene("Result");
    }

    public async void FinishAsync()
    {//�I�����đގ�
        //FinishAsync���Ăяo��
        await roomModel.FinishAsync();
    }
}
