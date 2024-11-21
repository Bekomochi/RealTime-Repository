using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{//ゲーム進行を管理するクラス
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();//接続IDをキーにして、キャラクターのオブジェクトを管理

    // Start is called before the first frame update
    async void Start()
    {//ユーザーが入室した時にOnJoinedUserメソッドを実行できるように、モデルに登録しておく
        roomModel.OnJoinedUser += this.OnJoinedUser;

        //接続
        await roomModel.ConnectAsync();
    }

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
    {//入室したら、OnJoinedUserをInstantiateする
        GameObject characterObject = Instantiate(characterPrefab);//インスタンス生成
        characterObject.transform.position = new Vector3(0, 0, 0);
        characterList[user.ConnectionID]= characterObject;//フィールドで保持
    }

    // Update is called once per frame
    void Update()
    {

    }
}
