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
using DG.Tweening;

public class GameDirector : MonoBehaviour
{
    //=======================
    //�Q�[���i�s���Ǘ�����N���X
    //�쐬:�O�Y�L�H
    //=======================

    [SerializeField] GameObject[] characterPrefab; //�L�����N�^�[�̃v���n�u
    [SerializeField] GameObject JoinButton; //�����{�^�� 
    [SerializeField] GameObject LeaveButton; //�ގ��{�^��
    [SerializeField] RoomModel roomModel; //RoomModel
    [SerializeField] Transform[] initPosList; //�����ʒu

    [SerializeField] AudioClip ClickSE; //�{�^���N���b�N��SE
    [SerializeField] AudioClip JoinSE; //������SE
    [SerializeField] AudioClip LeaveSE; //�ގ���SE

    private static string roomName;
    public static string RoomName {  get { return roomName; } }

    private static int id;
    public static int Id { get { return id; } }

    //�T�E���h�Đ��p
    AudioSource audioSource;

    //�I�u�W�F�N�g�ƌ��т���
    public InputField IDinputField;
    public GameObject LoseText;

    //�ڑ�ID���L�[�ɂ��āA�L�����N�^�[�̃I�u�W�F�N�g���Ǘ�
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();

    // Start is called before the first frame update
    async void Start()
    {
        //���f���ɓo�^����
        roomModel.OnJoinedUser += this.OnJoinedUser;//����
        roomModel.OnMatchingUser += this.OnMatchingUser;//�}�b�`���O
        roomModel.OnLeavedUser += this.OnLeavedUser;//�ގ�
        //roomModel.OnMoveCharacter += OnMoveCharacter;//�ʒu����
        //roomModel.OnValue += OnHPValue;
        //roomModel.OnShotWater += OnShotWater;

        //�ڑ�
        await roomModel.ConnectAsync();

        //BGM���Đ�
        audioSource = GetComponent<AudioSource>();

        //�ŏ���ReaveButton���\���ɂ���
        LeaveButton.gameObject.SetActive(false);
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

        audioSource.PlayOneShot(JoinSE);

        //LeaveButton��\������
        LeaveButton.gameObject.SetActive(true);

        //JoinButton���\���ɂ���
        JoinButton.gameObject.SetActive(false);

        //IDinputField���\���ɂ���
        IDinputField.gameObject.SetActive(false);
    }

    //���[�U�[�������������̏���
    private void OnJoinedUser(JoinedUser user)
    {//����������Instantiate����
        GameObject characterObject = Instantiate(characterPrefab[user.JoinOrder]);//�C���X�^���X����
        characterObject.transform.position = initPosList[user.JoinOrder].position;
        
        characterList[user.ConnectionID] = characterObject;//�t�B�[���h�ŕێ�

        //���[�����f���̐ڑ�ID��user�̐ڑ�ID��������������
        if (roomModel.ConnectionId == user.ConnectionID)
        {
            //characterObject����Character��Get���āACharacter����bool�ϐ�isSelf��true�ɂ���
            characterObject.GetComponent<LobbyCharacter>().isSelf = true;

            ////InvokeRepeating��MovedUserasync�����I�ɌĂяo���ď�Ԃ��X�V
            //InvokeRepeating("MovedUserasync", 0.1f, 0.1f);
        }
    }

    //���������Ƃ͕ʂ́AUI����ʉ��̊֐�
    public void JoinButtonSetting()
    {
        audioSource.PlayOneShot(ClickSE);
    }

    /// <summary>
    /// �}�b�`���O����
    /// </summary>
    /// <param name="roomName">�n�������̖��O</param>
    /// <param name="userID">���[�U�[��ID</param>

    public void OnMatchingUser(string roomName)
    {
        CancelInvoke();//��ʑJ�ڂ̃^�C�~���O��Invoke���~�߂�
        Initiate.Fade("GameRoom",Color.black,1.5f);
        GameDirector.roomName = roomName;

        //LeaveButton���\���ɂ���
        LeaveButton.gameObject.SetActive(false);

        //JoinButton���\���ɂ���
        JoinButton.gameObject.SetActive(false);

        //IDinputField���\���ɂ���
        IDinputField.gameObject.SetActive(false);
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

        //LeaveButton���\���ɂ���
        LeaveButton.gameObject.SetActive(false);

        //JoinButton��\������
        JoinButton.gameObject.SetActive(true);

        //IDinputField��\������
        IDinputField.gameObject.SetActive(true);

        //LeaveSE��炷
        audioSource.PlayOneShot(LeaveSE);
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

    //void OnMoveCharacter(MovedUser movedUser)
    //{//MovedUser�N���X�ɐڑ�ID�A�ʒu�A��]�̏�񂪓����Ă���

    //    //characterList����Ώۂ�GameObject���擾�A�ʒu�E��]�𔽉f
    //    /* 2024/12/25�ύX
    //     * ���f�̍ہA�l�̑���ł͂Ȃ�DOLocalMove�ɕύX�B�������邱�ƂŁA�����ȊO�̉�ʂł����炩�ɓ����Č�����B
    //     */
    //    characterList[movedUser.ConnectionID].gameObject.transform.DOLocalMove(movedUser.pos, 0.3f);
    //}

    //public async void MovedUserasync()
    //{
    //    MovedUser movedUser = new MovedUser();
    //    movedUser.pos = characterList[roomModel.ConnectionId].gameObject.transform.position;
    //    movedUser.rot = characterList[roomModel.ConnectionId].gameObject.transform.rotation;
    //    movedUser.ConnectionID = roomModel.ConnectionId;

    //    //MoveAsync�Ăяo��
    //    await roomModel.MoveAsync(movedUser);
    //}

    /// <summary>
    /// HP�X�V����
    /// </summary>

    //public void OnHPValue(int hp)
    //{
    //    CharacterHP=hp;
    //}

    //public async void HPValueAsync(int hp)
    //{
    //    await roomModel.HPValueAsync(hp);
    //}

    /// <summary>
    /// ���S�C���ˏ���
    /// </summary>

    //public async void ShotAsync()
    //{
    //    await roomModel.ShotAsync();
    //}

    //public void OnShotWater()
    //{
    //    characterList[roomModel.ConnectionId].GetComponent<Character>().OnShotButton();
    //}
}
