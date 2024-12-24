using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;

public class Character : MonoBehaviour
{
    //Rigidbody rb;
    FloatingJoystick floatingJoystick;//�X�}�z�Ή��p�X���C�h�p�b�h

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
        if (isSelf == true)
        {
            //���Ɉړ�
            if (Input.GetKey(KeyCode.W))
            {
                transform.DOLocalMoveZ(1f, 0.6f).SetRelative();
            }

            //��O�Ɉړ�
            if (Input.GetKey(KeyCode.S))
            {
                transform.DOLocalMoveZ(-1f, 0.6f).SetRelative();
            }

            //�E�Ɉړ�
            if (Input.GetKey(KeyCode.D))
            {
                //transform.position += speed * transform.right * Time.deltaTime;
                transform.DOLocalMoveX(1f, 0.6f).SetRelative();
            }

            //���Ɉړ�
            if (Input.GetKey(KeyCode.A))
            {
                transform.DOLocalMoveX(-1f, 0.6f).SetRelative();
            }
        }
    }
}