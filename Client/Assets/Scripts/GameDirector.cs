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

public class GameDirector : MonoBehaviour
{//ゲーム進行を管理するクラス

    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] int CountNum;
    [SerializeField] int MasterTimer;

    public Text CountDownText; //スタートまでのカウントダウン用のテキスト(3カウント)
    public Text StartText; //開始用テキスト([Start!!])
    public GameObject FinishButton;//終了用仮ボタン

    //オブジェクトと結びつける
    public InputField IDinputField;

    //接続IDをキーにして、キャラクターのオブジェクトを管理
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();

    // Start is called before the first frame update
    async void Start()
    {
        //最初はCountDownTextを非表示にする
        CountDownText.gameObject.SetActive(false);

        //最初はStartTextを非表示にする
        StartText.gameObject.SetActive(false);

        //最初はFinishBuutonを非表示にする
        FinishButton.gameObject.SetActive(false);

        //CountDownText周りの設定
        CountNum = 3; //CountNumを初期化
        CountDownText.text = CountNum.ToString(); //CountDownTextを時間に反映させる

        //InvokeRepeatingで、1秒ごとにCountDown関数を呼び出す

        //モデルに登録する
        roomModel.OnJoinedUser += this.OnJoinedUser;//入室
        roomModel.OnMatchingUser += this.OnMatchingUser;//マッチング
        roomModel.OnLeavedUser += this.OnLeavedUser;//退室
        roomModel.OnMoveCharacter += OnMoveCharacter;//位置同期
        roomModel.OnPreparationUser += this.OnPreparationUser; //準備完了
        roomModel.OnReadyGame += this.OnReadyGame; //ゲーム開始
        roomModel.OnFinishGame += this.OnFinishGame;//ゲーム終了

        //ユーザーIDを入力する入力フィールドをGetComponentする
        IDinputField = IDinputField.GetComponent<InputField>();

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
        string IDtext = IDinputField.text;
        int.TryParse(IDtext, out int id);

        //入室
        await roomModel.JoinAsync("Lobby", id);
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
        characterList[user.ConnectionID] = characterObject;//フィールドで保持

        if (roomModel.ConnectionId == user.ConnectionID)
        {
            //
            //InvokeRepeatingで定期的に状態を送信
            //

            //characterObjectからCharacterをGetして、Character内のbool変数isSelfをtrueにする
            characterObject.GetComponent<Character>().isSelf = true;

            //InvokeRepeatingでMovedUserasyncを定期的に呼び出して状態を更新
            InvokeRepeating("MovedUserasync", 0.1f, 0.1f);
        }
    }

    //マッチング

    public async void MatchingUser(string roomName,int userID)
    {
        await roomModel.MatchingAsync(roomName,userID);
        
    }

    public void OnMatchingUser(string roomName)
    {
        OnMatchingUser(roomName);
    }

    /// <summary>
    /// 切断、退室処理
    /// </summary>

    //退室
    public async void LeaveRoom()
    {
        //位置同期の定期的送信を終了する
        CancelInvoke("MovedUserasync");

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

    void OnMoveCharacter(MovedUser movedUser)
    {
        //characterListから対象のGameObjectを取得、対象に位置・回転を反映
        characterList[movedUser.ConnectionID].gameObject.transform.position = movedUser.pos;
        characterList[movedUser.ConnectionID].gameObject.transform.rotation = movedUser.rot;
    }

    public async void MovedUserasync()
    {
        MovedUser movedUser = new MovedUser();
        movedUser.pos = characterList[roomModel.ConnectionId].gameObject.transform.position;
        movedUser.rot = characterList[roomModel.ConnectionId].gameObject.transform.rotation;
        movedUser.ConnectionID = roomModel.ConnectionId;

        //MoveAsync呼び出し
        await roomModel.MoveAsync(movedUser);
    }

    public async void OnPreparationUser()
    {//開始前のカウント
     //この中でawaitでカウントを進められる//

        //3人集まったらCountDownTextを表示して、カウントダウンしていく
        CountDownText.gameObject.SetActive(true); //CountDownTextを表示

        //1秒ごとにカウントダウン
        InvokeRepeating("CountDown", 1, 1);
    }

    public async void OnReadyGame()
    {
        //StartTextを表示させる
        StartText.gameObject.SetActive(true); //タイムラグが発生した時に遅れたユーザーが不利になるので、表示させるタイミングを合わせる

        //
        //キャラクターを動かせる状態にする
        //

        //1000ms後(1秒後)にStartTextを非表示にする設定
        await Task.Delay(1000); //以下の処理は1秒後に非表示/表示とする
        StartText.gameObject.SetActive(false);　//StartTextを非表示にする

        //FinishBuutonを表示する
        FinishButton.gameObject.SetActive(true);
    }

    public async Task CountDown()
    {//カウントダウンする関数
        CountNum--;
        CountDownText.text = CountNum.ToString();

        if (CountNum == 0)
        {
            //カウントダウンを止める
            CancelInvoke("CountDown");

            //CountDownTextを非表示
            CountDownText.gameObject.SetActive(false);

            //ReadyAsyncを呼び出す
            await roomModel.ReadyAsync();
        }
    }

    public void OnFinishGame()
    {//ゲーム終了(ボタンを押して終了するのはアルファ版のみ)
        //リザルト画面に移動
        SceneManager.LoadScene("Result");
    }

    public async void FinishAsync()
    {//終了して退室
        //FinishAsyncを呼び出す
        await roomModel.FinishAsync();
    }
}
