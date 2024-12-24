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
    //[SerializeField] int MasterTimer;

    private static string roomName;
    public static string RoomName {  get { return roomName; } }

    private static int id;
    public static int Id { get { return id; } }

    //�I�u�W�F�N�g�ƌ��т���
    public InputField IDinputField;

    //�ڑ�ID���L�[�ɂ��āA�L�����N�^�[�̃I�u�W�F�N�g���Ǘ�
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();

    // Start is called before the first frame update
    async void Start()
    {
        //���f���ɓo�^����
        roomModel.OnJoinedUser += this.OnJoinedUser;//����
        roomModel.OnMatchingUser += this.OnMatchingUser;//�}�b�`���O
        roomModel.OnLeavedUser += this.OnLeavedUser;//�ގ�
        roomModel.OnMoveCharacter += OnMoveCharacter;//�ʒu����

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
        string IDtext=IDinputField.text;
        int.TryParse(IDtext, out int id);
        GameDirector.id = id;

        //����
        await roomModel.LobbyAsync(id);
        /*���[�����ƃ��[�U�[ID��n���ē�������B
         *�ŏI�I�ɂ́A�u���[�J���ɕۑ����ꂽUserID�v���w�肷��B
         */
    }

    //���[�U�[�������������̏���
    private void OnJoinedUser(JoinedUser user)
    {//����������Instantiate����
        GameObject characterObject = Instantiate(characterPrefab);//�C���X�^���X����
        characterObject.transform.position = new Vector3(0, 0, 0);
        
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

    /// <summary>
    /// �}�b�`���O����
    /// </summary>
    /// <param name="roomName">�n�������̖��O</param>
    /// <param name="userID">���[�U�[��ID</param>

    public void OnMatchingUser(string roomName)
    {
        CancelInvoke();//��ʑJ�ڂ̃^�C�~���O��Invoke���~�߂�
        Initiate.Fade("GameRoom",Color.black,1.0f);
        GameDirector.roomName = roomName;
    }

    /// <summary>
    /// �ؒf�A�ގ�����
    /// </summary>

    //�ގ�
    public async void LeaveRoom()
    {
        //�ʒu�����̒���I���M���I������
        CancelInvoke();

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
}
