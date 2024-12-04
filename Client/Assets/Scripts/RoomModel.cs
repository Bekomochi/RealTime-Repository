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
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class RoomModel :BaseModel, IRoomHubReciver //Reciver�̃C���^�[�t�F�[�X���p��(����)
{
    private GrpcChannel channel;
    private IRoomHub roomHub;

    /*==����==
     *Model�́A�f�[�^�݂̂���������N���X�B
     *���ׁ̈AUI����擾/UI�ɐݒ�Ȃǂ́A���̃N���X�ł͍s��Ȃ��B
     */

    //�ڑ�ID
    public Guid ConnectionId { get; private set; }

    //���[�U�[�ڑ��ʒm
    public Action<JoinedUser> OnJoinedUser { get; set; }//Model���g���N���X�ɂ́AAction���g���ăT�[�o�[����͂����f�[�^��n��

    //���[�U�[�ގ��ʒm
    public Action<LeavedUser> OnLeavedUser { get; set; }

    //���[�U�[�ړ��ʒm
    public Action<MovedUser> OnMoveCharacter { get; set; }

    //���[�U�[���������ʒm
    public Action OnPreparationUser {  get; set; }

    //�Q�[���J�n�ʒm
    public Action OnReadyGame {  get; set; }

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
    }

    //�ގ��ʒm
    public void OnLeave(LeavedUser user)
    {
        OnLeavedUser(user);
    }

    /// <summary>
    /// �ʒu��������
    /// </summary>
    /// <returns></returns>

    //�ʒu�A��]�𑗐M����
    public async Task MoveAsync(MovedUser movedUser)
    {
        //�T�[�o�[�̊֐����Ăяo��
        await roomHub.MoveAsync(movedUser);
    }

    public void OnMove(MovedUser movedUser)
    {
        OnMoveCharacter(movedUser);
    }

    /// <summary>
    /// �Q�[���J�n����
    /// </summary>
    
    public void OnPreparation()
    {
        OnPreparationUser();
    }

    public void OnReady()
    {
        OnReadyGame();
    }

    public async Task ReadyAsync()
    {
        await roomHub.ReadyAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
