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
{//ゲーム進行を管理するクラス

    [SerializeField] GameObject[] characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] Transform[] initPosList;
    private static string roomName;
    public static string RoomName {  get { return roomName; } }

    private static int id;
    public static int Id { get { return id; } }

    //オブジェクトと結びつける
    public InputField IDinputField;
    public GameObject LoseText;

    //接続IDをキーにして、キャラクターのオブジェクトを管理
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();

    // Start is called before the first frame update
    async void Start()
    {
        //モデルに登録する
        roomModel.OnJoinedUser += this.OnJoinedUser;//入室
        roomModel.OnMatchingUser += this.OnMatchingUser;//マッチング
        roomModel.OnLeavedUser += this.OnLeavedUser;//退室
        //roomModel.OnMoveCharacter += OnMoveCharacter;//位置同期
        //roomModel.OnValue += OnHPValue;
        //roomModel.OnShotWater += OnShotWater;

        //接続
        await roomModel.ConnectAsync();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 入室処理
    /// </summary>

    public async void JoinRoom()
    {
        string IDtext=IDinputField.text;
        int.TryParse(IDtext, out int id);
        GameDirector.id = id;

        //入室
        await roomModel.LobbyAsync(id);
        /*ルーム名とユーザーIDを渡して入室する。
         *最終的には、「ローカルに保存されたUserID」を指定する。
         */
    }

    //ユーザーが入室した時の処理
    private void OnJoinedUser(JoinedUser user)
    {//入室したらInstantiateする
        GameObject characterObject = Instantiate(characterPrefab[user.JoinOrder]);//インスタンス生成
        characterObject.transform.position = initPosList[user.JoinOrder].position;
        
        characterList[user.ConnectionID] = characterObject;//フィールドで保持

        //ルームモデルの接続IDとuserの接続IDが同じだったら
        if (roomModel.ConnectionId == user.ConnectionID)
        {
            //characterObjectからCharacterをGetして、Character内のbool変数isSelfをtrueにする
            characterObject.GetComponent<LobbyCharacter>().isSelf = true;

            ////InvokeRepeatingでMovedUserasyncを定期的に呼び出して状態を更新
            //InvokeRepeating("MovedUserasync", 0.1f, 0.1f);
        }
    }

    /// <summary>
    /// マッチング処理
    /// </summary>
    /// <param name="roomName">渡す部屋の名前</param>
    /// <param name="userID">ユーザーのID</param>

    public void OnMatchingUser(string roomName)
    {
        CancelInvoke();//画面遷移のタイミングでInvokeを止める
        Initiate.Fade("GameRoom",Color.black,1.5f);
        GameDirector.roomName = roomName;
    }

    /// <summary>
    /// 切断、退室処理
    /// </summary>

    //退室
    public async void LeaveRoom()
    {
        //位置同期の定期的送信を終了する
        CancelInvoke();

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

    /// <summary>
    /// 位置同期処理
    /// </summary>

    //void OnMoveCharacter(MovedUser movedUser)
    //{//MovedUserクラスに接続ID、位置、回転の情報が入っている

    //    //characterListから対象のGameObjectを取得、位置・回転を反映
    //    /* 2024/12/25変更
    //     * 反映の際、値の代入ではなくDOLocalMoveに変更。こうすることで、自分以外の画面でも滑らかに動いて見える。
    //     */
    //    characterList[movedUser.ConnectionID].gameObject.transform.DOLocalMove(movedUser.pos, 0.3f);
    //}

    //public async void MovedUserasync()
    //{
    //    MovedUser movedUser = new MovedUser();
    //    movedUser.pos = characterList[roomModel.ConnectionId].gameObject.transform.position;
    //    movedUser.rot = characterList[roomModel.ConnectionId].gameObject.transform.rotation;
    //    movedUser.ConnectionID = roomModel.ConnectionId;

    //    //MoveAsync呼び出し
    //    await roomModel.MoveAsync(movedUser);
    //}

    /// <summary>
    /// HP更新処理
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
    /// 水鉄砲発射処理
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
