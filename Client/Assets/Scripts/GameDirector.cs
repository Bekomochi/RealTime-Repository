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
    [SerializeField] int CountNum=3;

    public Text CountDownText;

    //�I�u�W�F�N�g�ƌ��т���
    public InputField IDinputField;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();//�ڑ�ID���L�[�ɂ��āA�L�����N�^�[�̃I�u�W�F�N�g���Ǘ�

    // Start is called before the first frame update
    async void Start()
    {
        //�ŏ���CountDownText���\���ɂ���
        CountDownText.gameObject.SetActive(false);

        //���f���ɓo�^����
        roomModel.OnJoinedUser += this.OnJoinedUser;//����
        roomModel.OnLeavedUser += this.OnLeavedUser;//�ގ�
        roomModel.OnMoveCharacter += OnMoveCharacter;//�ʒu����
        roomModel.OnPreparationUser += this.OnPreparationUser; //��������
        roomModel.OnReadyGame += this.OnReadyGame; //�Q�[���J�n

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

        //ReadyAsync���Ăяo��
        await roomModel.ReadyAsync();
    }

    public void OnReadyGame()
    {
        //�L�����N�^�[�𓮂������Ԃɂ���


    }
}
