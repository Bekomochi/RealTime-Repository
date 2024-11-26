using Server.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameDirector : MonoBehaviour
{//ゲーム進行を管理するクラス
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;

    //オブジェクトと結びつける
    public InputField IDinputField;
    
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();//接続IDをキーにして、キャラクターのオブジェクトを管理

    // Start is called before the first frame update
    async void Start()
    {
        //ユーザーが入/退室した時にOnJoinedUserメソッドを実行できるように、モデルに登録しておく
        roomModel.OnJoinedUser += this.OnJoinedUser;//入室
        roomModel.OnLeavedUser += this.OnLeavedUser;//退室

        //ユーザーIDを入力する入力フィールドをGetComponentする
        IDinputField = IDinputField.GetComponent<InputField>();

        //接続
        await roomModel.ConnectAsync();
    }

    /// <summary>
    /// 入室処理
    /// </summary>

    public async void JoinRoom()
    {
        string IDtext = IDinputField.text;
        int.TryParse(IDtext, out int id);

        //入室
        await roomModel.JoinAsync("SampleRoom",id );
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
        await roomModel.LeaveAsync();
    }

    //ユーザーが切断した時の処理(切断したらDestroy)
    private void OnLeavedUser(LeavedUser user)
    {//退室したらDestroyする

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

    //characterlistから対象のGameobjectを取得
    void OnMoveCharacter(/*接続ID、位置、回転*/)
    {
        //characterlistから対象のGameobjectを取得
        //位置、回転を反映
    }

    // Update is called once per frame
    void Update()
    {

    }
}
