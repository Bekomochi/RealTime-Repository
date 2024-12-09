using Server.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameDirector : MonoBehaviour
{//�Q�[���i�s���Ǘ�����N���X

    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] int CountNum;
    [SerializeField] int MasterTimer;

    public Text CountDownText; //�X�^�[�g�܂ł̃J�E���g�_�E���p�̃e�L�X�g(3�J�E���g)
    public Text StartText; //�J�n�p�e�L�X�g([Start!!])
    public Text FinishText; //�I���p�e�L�X�g([Finish!!])
    public GameObject FinishButton;//�I���p���{�^��

    //�I�u�W�F�N�g�ƌ��т���
    public InputField IDinputField;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();//�ڑ�ID���L�[�ɂ��āA�L�����N�^�[�̃I�u�W�F�N�g���Ǘ�

    // Start is called before the first frame update
    async void Start()
    {
        //�ŏ���CountDownText���\���ɂ���
        CountDownText.gameObject.SetActive(false);

        //�ŏ���StartText���\���ɂ���
        StartText.gameObject.SetActive(false);

        //�ŏ���FinishText���\���ɂ���
        FinishText.gameObject.SetActive(false);

        //�ŏ���FinishBuuton���\���ɂ���
        FinishButton.gameObject.SetActive(false);

        //CountDownText����̐ݒ�
        CountNum = 3; //CountNum��������
        CountDownText.text=CountNum.ToString(); //CountDownText�����Ԃɔ��f������

        //InvokeRepeating�ŁA1�b���Ƃ�CountDown�֐����Ăяo��

        //���f���ɓo�^����
        roomModel.OnJoinedUser += this.OnJoinedUser;//����
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
        await roomModel.JoinAsync("SampleRoom",id );
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
        characterList[user.ConnectionID]= characterObject;//�t�B�[���h�ŕێ�

        if (roomModel.ConnectionId == user.ConnectionID)
        {
            //
            //InvokeRepeating�Œ���I�ɏ�Ԃ𑗐M
            //

            //characterObject����Character��Get���āACharacter����bool�ϐ�isSelf��true�ɂ���
            characterObject.GetComponent<Character>().isSelf = true;

            //InvokeRepeating��MovedUserasync�����I�ɌĂяo���ď�Ԃ��X�V
            InvokeRepeating("MovedUserasync", 0.1f,0.1f);
        }
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

        //ReadyAsync���Ăяo��
        await roomModel.ReadyAsync();

        //FinishBuuton��\������
        FinishButton.gameObject.SetActive(true);
    }

    public void OnReadyGame()
    {
        //�L�����N�^�[�𓮂������Ԃɂ���
    }

    public async Task CountDown()
    {//�J�E���g�_�E������֐�
         CountNum--;
        CountDownText.text=CountNum.ToString();

        if (CountNum == 0)
        {
            //�J�E���g�_�E�����~�߂�
            CancelInvoke("CountDown");

            //CountDownText���\��
            CountDownText.gameObject.SetActive(false); 

            //StartText��\��������
            StartText.gameObject.SetActive(true);
        }
    }

    public void OnFinishGame()
    {//�Q�[���I��(�{�^���������ďI������̂̓A���t�@�ł̂�)
        //FinishText��\������
        FinishText.gameObject.SetActive(true);

        //StartText���\���ɂ���
        StartText.gameObject.SetActive(false);
    }

    public async void FinishAsync()
    {
        //FinishAsync���Ăяo��
        await roomModel.FinishAsync();
    }
}
