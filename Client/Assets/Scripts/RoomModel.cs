using Assets.Scripts;
using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Net.Client;
using MagicOnion.Client;
using Server.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class RoomModel :BaseModel, IRoomHubReciver //Reciver�̃C���^�[�t�F�[�X���p��(����)
{
    //==============================
    //�T�[�o�[�ƒʐM����RoomModel�N���X
    //�쐬:�O�Y�L�H
    //==============================

    private GrpcChannel channel;
    private IRoomHub roomHub;

    /*==����==
     *Model�́A�f�[�^�݂̂���������N���X�B
     *���ׁ̈AUI����擾/UI�ɐݒ�Ȃǂ́A���̃N���X�ł͍s��Ȃ��B
     */

    //Model���g���N���X�ɂ́AAction���g���ăT�[�o�[����͂����f�[�^��n��

    //�ڑ�ID
    public Guid ConnectionId { get; private set; }

    //���[�U�[�ڑ��ʒm
    public Action<JoinedUser> OnJoinedUser { get; set; }

    //���[�U�[�}�b�`���O�ʒm
    public Action<string> OnMatchingUser {  get; set; }

    //���[�U�[�ގ��ʒm
    public Action<LeavedUser> OnLeavedUser { get; set; }

    //���[�U�[�ړ��ʒm
    public Action<MovedUser> OnMoveCharacter { get; set; }

    //���[�U�[���������ʒm
    public Action OnPreparationUser {  get; set; }

    //�Q�[���J�n�ʒm
    public Action OnReadyGame {  get; set; }

    //�Q�[���I���ʒm
    public Action OnFinishGame {  get; set; }

    //HP�X�V�ʒm
    public Action<int> OnValue { get; set; }

    //���S�C���˒ʒm
    public Action OnShotWater { get; set; }

    //MagicOnion�ڑ�����
    public async UniTask ConnectAsync()
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        roomHub = await StreamingHubClient.ConnectAsync<IRoomHub, IRoomHubReciver>(channel, this);
    }

    //MagicOnion�ؒf����
    public async UniTask DisConnectAsync()
    {
        if(roomHub != null) await roomHub.DisposeAsync();
        if(channel != null) await channel.ShutdownAsync();
        roomHub = null;
        channel = null;
    }

    //�j������(�A�v���I���ȂǁA�j������i�K�Őؒf����)
    async void OnDestroy()
    {
        DisConnectAsync();//�j������ۂɐڑ���ؒf����
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="roomName">�������镔���̖��O</param>
    /// <param name="userID">���[�U�[ID</param>
    /// <returns></returns>

    //����
    public async UniTask JoinAsync(string roomName, int userID)
    {
        await ConnectAsync();
        JoinedUser[] users = await roomHub.JoinAsync(roomName, userID);//RPCService�̌Ăяo���Ɠ���

        foreach (var user in users)
        {
            if (user.UserData.Id == userID) this.ConnectionId = user.ConnectionID;//�����̐ڑ�ID��ۑ�����

            OnJoinedUser(user);//Action��Model���g���N���X�ɒʒm
        }
    }

    //�����ʒm(IRoomHubReciver�C���^�[�t�F�C�X�̎���)
    public void OnJoin(JoinedUser user)
    {
        OnJoinedUser.Invoke(user);
    }

    public async UniTask LobbyAsync(int userID)
    {
        await ConnectAsync();
        JoinedUser[] users = await roomHub.JoinLobbyAsync(userID);

        foreach (var user in users)
        {
            if (user.UserData.Id == userID) this.ConnectionId = user.ConnectionID;//�����̐ڑ�ID��ۑ�����

            OnJoinedUser(user);//Action��Model���g���N���X�ɒʒm
        }
    }

    public void OnMatching(string roomName)
    {
        OnMatchingUser(roomName);
    }

    /// <summary>
    /// �ގ�����
    /// </summary>
    /// <param name="roomName">�������镔���̖��O</param>
    /// <param name="userID">���[�U�[ID</param>
    /// <returns></returns>

    //�ގ�
    public async UniTask LeaveAsync()
    {
        LeavedUser user = await roomHub.LeaveAsync();
        await DisConnectAsync();
    }

    //�ގ��ʒm
    public void OnLeave(LeavedUser user)
    {
        OnLeavedUser(user);
    }

    /// <summary>
    /// �ʒu��������
    /// </summary>

    //�ʒu�A��]�𑗐M����
    public async Task MoveAsync(MovedUser movedUser)
    {
        //�T�[�o�[�̊֐����Ăяo��
        await roomHub.MoveAsync(movedUser);
    }

    public void OnMove(MovedUser movedUser)
    {//MovedUser�N���X�ɐڑ�ID�A�ʒu�A��]�̏�񂪓����Ă���
        OnMoveCharacter(movedUser);
    }

    /// <summary>
    /// �Q�[���J�n����
    /// </summary>

    /// <summary>
    /// �v���C���[�����i�K�̒ʐM
    /// </summary>

    public void OnPreparation()
    {
        OnPreparationUser();
    }

    /// <summary>
    /// �������ł������Ƃ̒ʐM
    /// </summary>
 
    public void OnReady()
    {
        OnReadyGame();
    }

    public async Task ReadyAsync()
    {
        await roomHub.ReadyAsync();
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    public void OnFinish()
    {
        OnFinishGame();
    }

    public async Task FinishAsync()
    {
        await roomHub.FinishAsync();
    }

    /// <summary>
    /// HP�X�V����
    /// </summary>
    /// <param name="hp">HP�̈���</param>
    /// <returns></returns>

    public async Task HPValueAsync(int hp)
    {
        await roomHub.HPValueAsync(hp);
    }

    public void OnHPValue(int hp)
    {
        OnValue(hp);
    }

    /// <summary>
    /// ���S�C���˂̓���
    /// </summary>
    /// <returns></returns>

    public async Task ShotAsync()
    {
        await roomHub.ShotAsync();
    }

    public void OnShot()
    {
        OnShotWater();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
