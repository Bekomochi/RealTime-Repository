using Assets.Scripts;
using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomModel :BaseModel, IRoomHubReciver //Reciver�̃C���^�[�t�F�[�X���p��(����)
{
    private GrpcChannel channel;
    private IRoomHub roomHub;

    /*==����==
     *Model�́A�f�[�^�݂̂���������N���X�B
     *���ׁ̈AUI����擾/UI�ɐݒ�Ȃǂ́A���̃N���X�ł͍s��Ȃ��B
     *���l�ɁA[SerializeField]��UI�͏����Ȃ��悤�ɋC��t����B
     */


    //�ڑ�ID
    public Guid ConnectionId { get; private set; }

    //���[�U�[�ڑ��ʒm
    public Action<JoinedUser> OnJoinedUser {  get; set; }//Model���g���N���X�ɂ́AAction���g���ăT�[�o�[����͂����f�[�^��n��

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

    //�j������
    async void OnDestroy()
    {
        DisConnectAsync();//�j������ۂɐڑ���ؒf����
    }

    //����
    public async UniTask JoinAsync(string roomName, int userID)
    {
        JoinedUser[] users=await roomHub.JoinAsync(roomName, userID);//RPCService�̌Ăяo���Ɠ���

        foreach(var user in users)
        {
            if (user.UserData.Id == userID) this.ConnectionId = user.ConnectionID;//�����̐ڑ�ID��ۑ�����
            OnJoinedUser(user);//Action��Model���g���N���X�ɒʒm
        }
    }

    //�����ʒm(IRoomHubReciver�C���^�t�F�C�X�̎���)
    public void OnJoin(JoinedUser user)
    {
        OnJoinedUser.Invoke(user);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
