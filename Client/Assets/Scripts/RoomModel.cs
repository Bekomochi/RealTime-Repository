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

public class RoomModel :BaseModel, IRoomHubReciver //Reciverのインターフェースを継承(実装)
{
    //==============================
    //サーバーと通信するRoomModelクラス
    //作成:三浦有稀
    //==============================

    private GrpcChannel channel;
    private IRoomHub roomHub;

    /*==注意==
     *Modelは、データのみを処理するクラス。
     *その為、UIから取得/UIに設定などは、このクラスでは行わない。
     */

    //Modelを使うクラスには、Actionを使ってサーバーから届いたデータを渡す

    //接続ID
    public Guid ConnectionId { get; private set; }

    //ユーザー接続通知
    public Action<JoinedUser> OnJoinedUser { get; set; }

    //ユーザーマッチング通知
    public Action<string> OnMatchingUser {  get; set; }

    //ユーザー退室通知
    public Action<LeavedUser> OnLeavedUser { get; set; }

    //ユーザー移動通知
    public Action<MovedUser> OnMoveCharacter { get; set; }

    //ユーザー準備完了通知
    public Action OnPreparationUser {  get; set; }

    //ゲーム開始通知
    public Action OnReadyGame {  get; set; }

    //ゲーム終了通知
    public Action OnFinishGame {  get; set; }

    //HP更新通知
    public Action<int> OnValue { get; set; }

    //水鉄砲発射通知
    public Action OnShotWater { get; set; }

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
        await ConnectAsync();
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

    public async UniTask LobbyAsync(int userID)
    {
        await ConnectAsync();
        JoinedUser[] users = await roomHub.JoinLobbyAsync(userID);

        foreach (var user in users)
        {
            if (user.UserData.Id == userID) this.ConnectionId = user.ConnectionID;//自分の接続IDを保存する

            OnJoinedUser(user);//ActionでModelを使うクラスに通知
        }
    }

    public void OnMatching(string roomName)
    {
        OnMatchingUser(roomName);
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
        await DisConnectAsync();
    }

    //退室通知
    public void OnLeave(LeavedUser user)
    {
        OnLeavedUser(user);
    }

    /// <summary>
    /// 位置同期処理
    /// </summary>

    //位置、回転を送信する
    public async Task MoveAsync(MovedUser movedUser)
    {
        //サーバーの関数を呼び出す
        await roomHub.MoveAsync(movedUser);
    }

    public void OnMove(MovedUser movedUser)
    {//MovedUserクラスに接続ID、位置、回転の情報が入っている
        OnMoveCharacter(movedUser);
    }

    /// <summary>
    /// ゲーム開始処理
    /// </summary>

    /// <summary>
    /// プレイヤー準備段階の通信
    /// </summary>

    public void OnPreparation()
    {
        OnPreparationUser();
    }

    /// <summary>
    /// 準備ができたことの通信
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
    /// ゲーム終了
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
    /// HP更新処理
    /// </summary>
    /// <param name="hp">HPの引数</param>
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
    /// 水鉄砲発射の同期
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
