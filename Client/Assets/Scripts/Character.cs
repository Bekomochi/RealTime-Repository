using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    [SerializeField] public Slider HPSlider;
    RoomModel roomModel;
    Animator animator;//アニメーター取得
    Rigidbody rigidbody;//RigitBody取得

    public static int CharacterHP = 100;//キャラクターの体力。適宜調整
    float x;//キー方向(水平)
    float z;//キー方向(垂直,奥行)
    float moveSpeed=6;

    //FloatingJoystick floatingJoystick;//スマホ対応用スライドパッド
    float speed = 0.6f;//歩くスピードの変数。0.6fに設定
    public bool isSelf { get; set; } = false;//自分自身かどうかを判定する変数

    // Start is called before the first frame update
    void Start()
    {
        roomModel= GameObject.Find("RoomModel").GetComponent<RoomModel>();

        animator = GetComponent<Animator>();
        HPSlider.maxValue = CharacterHP;
        roomModel.OnValue += OnHPValue;
        rigidbody = GetComponent<Rigidbody>();

        //floatingJoystick=GameObject.Find(/*"名前"*/).GetComponent<FloatingJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity=new Vector3(1,0,0);
        //速度設定

        if (isSelf == true)
        {
            x= Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");

            ////奥に移動
            //if (Input.GetKey(KeyCode.W))
            //{
            //    //transform.DOLocalMoveZ(1f, speed).SetRelative();
            //    animator.SetInteger("state", 1);//移動した時、アニメーションのstateを1にする
            //}

            ////手前に移動
            //if (Input.GetKey(KeyCode.S))
            //{
            //    ////速度設定
            //    animator.SetInteger("state", 1);//移動した時、アニメーションのstateを1にする
            //    //transform.DOLocalMoveZ(-1f, speed).SetRelative();
            //}

            ////右に移動
            //if (Input.GetKey(KeyCode.D))
            //{
            //    ////速度設定
            //    //animator.SetInteger("state", 1);//移動した時、アニメーションのstateを1にする

            //    transform.DOLocalMoveX(1f, speed).SetRelative();
            //}

            ////左に移動
            //if (Input.GetKey(KeyCode.A))
            //{
            //    ////速度設定
            //    //animator.SetInteger("state", 1);//移動した時、アニメーションのstateを1にする

            //    transform.DOLocalMoveX(-1f, speed).SetRelative();
            //}
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
                CharacterHP= 0;
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
        rigidbody.velocity = new Vector3(x, 0, z) * moveSpeed;
        Vector3 direction = transform.position + new Vector3(x, 0, z) * moveSpeed;

        //方向転換
        transform.LookAt(direction);

        //アニメーション設定
        animator.SetFloat("speed", rigidbody.velocity.magnitude);
    }
}