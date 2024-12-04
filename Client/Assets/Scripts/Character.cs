using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;
using UnityEditor.Networking.PlayerConnection;

public class Character : MonoBehaviour
{
    float speed = 3.0f;
    Rigidbody rb;
    FloatingJoystick floatingJoystick;

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
        ////////////////////////////////////
        //最終的にDOTweenで移動できるようにする//
        //////////////////////////////////

        //Vector3 move = (Camera.main.transform.forward * floatingJoystick.Vertical + Camera.main.transform.right * floatingJoystick.Horizontal) * speed;
        //move.y=rb.velocity.y;
        //rb.velocity = move;

        if (isSelf == true)
        {
            //奥に移動
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += speed * transform.forward * Time.deltaTime;
            }

            //手前に移動
            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= speed * transform.forward * Time.deltaTime;
            }

            //右に移動
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += speed * transform.right * Time.deltaTime;
            }

            //左に移動
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= speed * transform.right * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.G))
        {
            Debug.Log("ゲーム開始！");
        }

    }
}