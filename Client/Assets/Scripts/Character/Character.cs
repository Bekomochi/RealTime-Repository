using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class Character : MonoBehaviour
{
    //========================
    //キャラクターを管理するクラス
    //作成:三浦有稀
    //========================

    [SerializeField] public Slider HPSlider;
    RoomModel roomModel;
    Animator animator;//アニメーター取得
    Rigidbody rigidbody;//RigitBody取得
    FixedJoystick fixedJoystick;//スマホ対応用タッチパッド(FixedJoyStick)

    public static int CharacterHP = 100;//キャラクターの体力。適宜調整
    float x;//キー方向(水平)
    float z;//キー方向(垂直,奥行)

    float speed = 10;//歩くスピードの変数
    public bool isSelf { get; set; } = false;//自分自身かどうかを判定する変数

    // Start is called before the first frame update
    void Start()
    {
        roomModel = GameObject.Find("RoomModel").GetComponent<RoomModel>();
        fixedJoystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        HPSlider.maxValue = CharacterHP;
        roomModel.OnValue += OnHPValue;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector3(1, 0, 0);

        //移動の設定
        if (isSelf == true)
        {//もし自分だったら
            if (Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
            {//AキーかDキーが押されたら
                x = Input.GetAxisRaw("Horizontal");//Horizontalを代入する
            }
            else
            {//JoyStickからの入力だったら
                x = fixedJoystick.Horizontal;//FixedJoyStickのHorizontalを代入する
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {//もしWキーかDキーが押されたら
                z = Input.GetAxisRaw("Vertical");//Verticalを代入する
            }
            else
            {//JoyStickからの入力だったら
                z = fixedJoystick.Vertical;//FixedJoyStickのVerticalを代入する
            }
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.tag == "warter")
        {
            CharacterHP -= 10;//水が当たったら、HPを10減らす
            HPSlider.value = CharacterHP;

            if (CharacterHP <= 0)
            {//キャラクターのHPが0以下になったら
                CharacterHP = 0;
                HPSlider.value = CharacterHP;
            }
        }
    }

    /// <summary>
    /// HP更新処理
    /// </summary>

    public void OnHPValue(int hp)
    {
        CharacterHP = hp;
    }

    public async void HPValueAsync(int hp)
    {
        await roomModel.HPValueAsync(hp);
    }

    public void FixedUpdate()
    {
        //速度設定
        rigidbody.velocity = new Vector3(x, 0, z) * speed;
        Vector3 direction = transform.position + new Vector3(x, 0, z) * speed;

        //方向転換
        transform.LookAt(direction);

        //アニメーション設定
        animator.SetFloat("speed", rigidbody.velocity.magnitude);
    }

    public void OnShotButton()
    {//発射ボタンを押したら
        animator.SetBool("shot", true);//発射アニメーションを再生する
    }
}
