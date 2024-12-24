using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRoomManager : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] int CountNum;

    GameDirector gameDirector;
    public Text CountDownText; //スタートまでのカウントダウン用のテキスト(3カウント)
    public Text StartText; //開始用テキスト([Start!!])
    public GameObject FinishButton;//終了用仮ボタン

    //接続IDをキーにして、キャラクターのオブジェクトを管理
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        roomModel.OnJoinedUser += this.OnJoinedUser;//入室
        roomModel.OnPreparationUser += this.OnPreparationUser; //準備完了
        roomModel.OnReadyGame += this.OnReadyGame; //ゲーム開始
        roomModel.OnFinishGame += this.OnFinishGame;//ゲーム終了

        /* Start関数内で、入室処理を呼び出す。
         * 入室処理内でインスタンス生成がされているので、インスタンス生成は記入しない。
         */
        JoinRoom();

        ////
        //CountDownText周りの設定
        ////
        CountNum = 3; //CountNumを初期化
        CountDownText.text = CountNum.ToString(); //CountDownTextを時間に反映させる
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void JoinRoom()
    {
        //入室
        await roomModel.JoinAsync(GameDirector.RoomName,GameDirector.Id);
        /*ルーム名とユーザーIDを渡して入室する。
         *最終的には、「ローカルに保存されたUserID」を指定する。
         */
    }

    private void OnJoinedUser(JoinedUser user)
    {//入室したらInstantiateする
        GameObject characterObject = Instantiate(characterPrefab);//インスタンス生成
        characterObject.transform.position = new Vector3(0, 0, 0);

        characterList[user.ConnectionID] = characterObject;//フィールドで保持

        //ルームモデルの接続IDとuserの接続IDが同じだったら
        if (roomModel.ConnectionId == user.ConnectionID)
        {
            //characterObjectからCharacterをGetして、Character内のbool変数isSelfをtrueにする
            characterObject.GetComponent<Character>().isSelf = true;

            //InvokeRepeatingでMovedUserasyncを定期的に呼び出して状態を更新
            InvokeRepeating("MovedUserasync", 0.1f, 0.1f);
        }
    }

    public async void OnPreparationUser()
    {//開始前のカウント
     //この中でawaitでカウントを進められる//

        //条件が満たされたらCountDownTextを表示して、カウントダウンしていく
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
            CancelInvoke();

            //CountDownTextを非表示
            CountDownText.gameObject.SetActive(false);

            //ReadyAsyncを呼び出す
            await roomModel.ReadyAsync();
        }
    }
    public void OnFinishGame()
    {//ゲーム終了(ボタンを押して終了するのは仮の動作)
        //リザルト画面に移動
        SceneManager.LoadScene("Result");
    }

    public async void FinishAsync()
    {//終了して退室
        //FinishAsyncを呼び出す
        await roomModel.FinishAsync();
    }

}
