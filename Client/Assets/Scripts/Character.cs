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
    [SerializeField] public Slider HPSlider;
    RoomModel roomModel;
    Animator animator;//アニメーター取得
    Rigidbody rigidbody;//RigitBody取得
    FixedJoystick fixedJoystick;//スマホ対応用タッチパッド(FixedJoyStick)
    Button shotButton;

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

        shotButton = GetComponent<Button>();
        animator = GetComponent<Animator>();
        HPSlider.maxValue = CharacterHP;
        roomModel.OnValue += OnHPValue;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector3(1, 0, 0);

        //タッチパッドの設定

        /* floatingJoystick.Verticalで上下、Horizontalで左右の入力値。
         * メインカメラの向きとかけることで、カメラ進行方向に対する移動量にできる。
         */


        //速度設定
        if (isSelf == true)
        {

            if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
            {
                x = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                x = fixedJoystick.Horizontal;
            }

            if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S))
            {
                z = Input.GetAxisRaw("Vertical");
            }
            else { z = fixedJoystick.Vertical; }
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
