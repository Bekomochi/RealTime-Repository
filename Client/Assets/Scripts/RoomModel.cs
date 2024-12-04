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

public class RoomModel :BaseModel, IRoomHubReciver //Reciverのインターフェースを継承(実装)
{
    private GrpcChannel channel;
    private IRoomHub roomHub;

    /*==注意==
     *Modelは、データのみを処理するクラス。
     *その為、UIから取得/UIに設定などは、このクラスでは行わない。
     */

    //接続ID
    public Guid ConnectionId { get; private set; }

    //ユーザー接続通知
    public Action<JoinedUser> OnJoinedUser { get; set; }//Modelを使うクラスには、Actionを使ってサーバーから届いたデータを渡す

    //ユーザー退室通知
    public Action<LeavedUser> OnLeavedUser { get; set; }

    //ユーザー移動通知
    public Action<MovedUser> OnMoveCharacter { get; set; }

    //ユーザー準備完了通知
    public Action OnPreparationUser {  get; set; }

    //ゲーム開始通知
    public Action OnReadyGame {  get; set; }

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

    //破棄処理(アプリ終了など、破棄する段階で切断する)
    async void OnDestroy()
    {
        DisConnectAsync();//破棄する際に接続を切断する
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// 入室処理
    /// </summary>
    /// <param name="roomName">入室する部屋の名前</param>
    /// <param name="userID">ユーザーID</param>
    /// <returns></returns>

    //入室
    public async UniTask JoinAsync(string roomName, int userID)
    {
        JoinedUser[] users = await roomHub.JoinAsync(roomName, userID);//RPCServiceの呼び出しと同じ

        foreach (var user in users)
        {
            if (user.UserData.Id == userID) this.ConnectionId = user.ConnectionID;//自分の接続IDを保存する

            OnJoinedUser(user);//ActionでModelを使うクラスに通知
        }
    }

    //入室通知(IRoomHubReciverインターフェイスの実装)
    public void OnJoin(JoinedUser user)
    {
        OnJoinedUser.Invoke(user);
    }

    /// <summary>
    /// 退室処理
    /// </summary>
    /// <param name="roomName">入室する部屋の名前</param>
    /// <param name="userID">ユーザーID</param>
    /// <returns></returns>

    //退室
    public async UniTask LeaveAsync()
    {
        LeavedUser user = await roomHub.LeaveAsync();
    }

    //退室通知
    public void OnLeave(LeavedUser user)
    {
        OnLeavedUser(user);
    }

    /// <summary>
    /// 位置同期処理
    /// </summary>
    /// <returns></returns>

    //位置、回転を送信する
    public async Task MoveAsync(MovedUser movedUser)
    {
        //サーバーの関数を呼び出す
        await roomHub.MoveAsync(movedUser);
    }

    public void OnMove(MovedUser movedUser)
    {
        OnMoveCharacter(movedUser);
    }

    /// <summary>
    /// ゲーム開始処理
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
