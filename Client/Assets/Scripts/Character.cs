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

    //�������g���ǂ����𔻒肷��ϐ�
    public bool isSelf { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        //floatingJoystick=GameObject.Find(/*"���O"*/).GetComponent<FloatingJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////
        //�ŏI�I��DOTween�ňړ��ł���悤�ɂ���//
        //////////////////////////////////

        //Vector3 move = (Camera.main.transform.forward * floatingJoystick.Vertical + Camera.main.transform.right * floatingJoystick.Horizontal) * speed;
        //move.y=rb.velocity.y;
        //rb.velocity = move;

        if (isSelf == true)
        {
            //���Ɉړ�
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += speed * transform.forward * Time.deltaTime;
            }

            //��O�Ɉړ�
            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= speed * transform.forward * Time.deltaTime;
            }

            //�E�Ɉړ�
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += speed * transform.right * Time.deltaTime;
            }

            //���Ɉړ�
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= speed * transform.right * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.G))
        {
            Debug.Log("�Q�[���J�n�I");
        }

    }
}