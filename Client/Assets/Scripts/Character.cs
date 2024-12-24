using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;

public class Character : MonoBehaviour
{
    //Rigidbody rb;
    FloatingJoystick floatingJoystick;//スマホ対応用スライドパッド

    //自分自身かどうかを判定する変数
    public bool isSelf { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
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
                transform.DOLocalMoveZ(1f, 0.6f).SetRelative();
            }

            //手前に移動
            if (Input.GetKey(KeyCode.S))
            {
                transform.DOLocalMoveZ(-1f, 0.6f).SetRelative();
            }

            //右に移動
            if (Input.GetKey(KeyCode.D))
            {
                //transform.position += speed * transform.right * Time.deltaTime;
                transform.DOLocalMoveX(1f, 0.6f).SetRelative();
            }

            //左に移動
            if (Input.GetKey(KeyCode.A))
            {
                transform.DOLocalMoveX(-1f, 0.6f).SetRelative();
            }
        }
    }
}