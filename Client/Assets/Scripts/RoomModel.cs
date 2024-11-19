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

public class RoomModel :BaseModel, IRoomHubReciver //Reciverのインターフェースを継承(実装)
{
    private GrpcChannel channel;
    private IRoomHub roomHub;

    /*==注意==
     *Modelは、データのみを処理するクラス。
     *その為、UIから取得/UIに設定などは、このクラスでは行わない。
     *同様に、[SerializeField]のUIは書かないように気を付ける。
     */


    //接続ID
    public Guid ConnectionId { get; private set; }

    //ユーザー接続通知
    public Action<JoinedUser> OnJoinedUser {  get; set; }//Modelを使うクラスには、Actionを使ってサーバーから届いたデータを渡す

    //MagicOnion接続処理
    public async UniTask ConnectAsync()
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        roomHub = await StreamingHubClient.ConnectAsync<IRoomHub, IRoomHubReciver>(channel, this);
    }

    //MagicOnion切断処理
    public async UniTask DisConnectAsync()
    {
        if(roomHub != null) await roomHub.DisposeAsync();
        if(channel != null) await channel.ShutdownAsync();
        roomHub = null;
        channel = null;
    }

    //破棄処理
    async void OnDestroy()
    {
        DisConnectAsync();//破棄する際に接続を切断する
    }

    //入室
    public async UniTask JoinAsync(string roomName, int userID)
    {
        JoinedUser[] users=await roomHub.JoinAsync(roomName, userID);//RPCServiceの呼び出しと同じ

        foreach(var user in users)
        {
            if (user.UserData.Id == userID) this.ConnectionId = user.ConnectionID;//自分の接続IDを保存する
            OnJoinedUser(user);//ActionでModelを使うクラスに通知
        }
    }

    //入室通知(IRoomHubReciverインタフェイスの実装)
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
