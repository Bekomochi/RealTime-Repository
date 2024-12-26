using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;

public class Character : MonoBehaviour
{
    FloatingJoystick floatingJoystick;//スマホ対応用スライドパッド
    float speed = 0.6f;//歩くスピードの変数。0.6fに設定
    Animator animator;

    //自分自身かどうかを判定する変数
    public bool isSelf { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        animator=GetComponent<Animator>();

        //floatingJoystick=GameObject.Find(/*"名前"*/).GetComponent<FloatingJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelf == true)
        {
            //奥に移動
            if (Input.GetKey(KeyCode.W))
            {
                //速度設定
                animator.SetFloat("speed", speed);//移動した時、speed秒の速さでステータスが"speed"のものをアニメーションする

                transform.DOLocalMoveZ(1f, speed).SetRelative();

            }

            //手前に移動
            if (Input.GetKey(KeyCode.S))
            {
                //速度設定
                animator.SetFloat("speed", speed);//移動した時、speed秒の速さでステータスが"speed"のものをアニメーションする

                transform.DOLocalMoveZ(-1f, speed).SetRelative();
            }

            //右に移動
            if (Input.GetKey(KeyCode.D))
            {
                //速度設定
                animator.SetFloat("speed", speed);//移動した時、speed秒の速さでステータスが"speed"のものをアニメーションする

                transform.DOLocalMoveX(1f, speed).SetRelative();
            }

            //左に移動
            if (Input.GetKey(KeyCode.A))
            {
                transform.DOLocalMoveX(-1f, speed).SetRelative();
            }
        }
    }
}