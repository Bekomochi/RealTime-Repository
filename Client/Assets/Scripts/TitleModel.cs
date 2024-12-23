using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    internal class TitleModel : BaseModel
    {
        ////オブジェクトと結びつける
        //public InputField IDinputField;

        public void Start()
        {
            //    //ユーザーIDを入力する入力フィールドをGetComponentする
            //    IDinputField = IDinputField.GetComponent<InputField>();
            //    string IDtext = IDinputField.text;
            //    int.TryParse(IDtext, out int id);
        }

        public void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
                //UserModelのLoadUserを呼び出す

                //if (/*userIDを読みこめたら*/)
                //{//既に登録済なら、何もせずにシーン移動
                //    //次のシーンへ
                //    SceneManager.LoadScene("Lobby");
                //}
                //else
                //{//UserModelのRegistUserで、User登録。

                //    /*
                //     *登録していなかったら、新規登録&UserID保存
                //     */

                //    //次のシーンへ
                //SceneManager.LoadScene("Lobby");
                //}
            //}
        }

        public void movementLobby()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
