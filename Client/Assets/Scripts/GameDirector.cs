using Server.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameDirector : MonoBehaviour
{//ゲーム進行を管理するクラス
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] InputField userIDField;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();//接続IDをキーにして、キャラクターのオブジェクトを管理

    // Start is called before the first frame update
    async void Start()
    {//ユーザーが入室した時にOnJoinedUserメソッドを実行できるように、モデルに登録しておく
        roomModel.OnJoinedUser += this.OnJoinedUser;

        roomModel.OnLeavedUser += this.OnLeavedUser;

        //接続
        await roomModel.ConnectAsync();
    }

    /// <summary>
    /// 入室処理
    /// </summary>

    public async void JoinRoom()
    {
        //入室
        await roomModel.JoinAsync("SampleRoom", 1);
        /*ルーム名とユーザーIDを渡して入室する。
         *ユーザーIDは、UIのinputfieldで入力できるようにしたい。
         *最終的には、「ローカルに保存されたUserID」を指定する。
         */
    }

    //ユーザーが入室した時の処理
    private void OnJoinedUser(JoinedUser user)
    {//入室したらInstantiateする
        GameObject characterObject = Instantiate(characterPrefab);//インスタンス生成
        characterObject.transform.position = new Vector3(0, 0, 0);
        characterList[user.ConnectionID]= characterObject;//フィールドで保持
    }

    /// <summary>
    /// 切断、退室処理
    /// </summary>

    //退室
    public async void LeaveRoom()
    {
        //退室
        await roomModel.LeaveAsync("SampleRoom", 1);
    }

    //ユーザーが切断した時の処理(切断したらDestroy)
    private void OnLeavedUser(LeavedUser user)
    {//退室したらDestroyする
        
        //Destroy(characterList[user.ConnectionID]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
