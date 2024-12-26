using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;

public class Character : MonoBehaviour
{
    FloatingJoystick floatingJoystick;//�X�}�z�Ή��p�X���C�h�p�b�h
    float speed = 0.6f;//�����X�s�[�h�̕ϐ��B0.6f�ɐݒ�
    Animator animator;

    //�������g���ǂ����𔻒肷��ϐ�
    public bool isSelf { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        animator=GetComponent<Animator>();

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
                //���x�ݒ�
                animator.SetFloat("speed", speed);//�ړ��������Aspeed�b�̑����ŃX�e�[�^�X��"speed"�̂��̂��A�j���[�V��������

                transform.DOLocalMoveZ(1f, speed).SetRelative();

            }

            //��O�Ɉړ�
            if (Input.GetKey(KeyCode.S))
            {
                //���x�ݒ�
                animator.SetFloat("speed", speed);//�ړ��������Aspeed�b�̑����ŃX�e�[�^�X��"speed"�̂��̂��A�j���[�V��������

                transform.DOLocalMoveZ(-1f, speed).SetRelative();
            }

            //�E�Ɉړ�
            if (Input.GetKey(KeyCode.D))
            {
                //���x�ݒ�
                animator.SetFloat("speed", speed);//�ړ��������Aspeed�b�̑����ŃX�e�[�^�X��"speed"�̂��̂��A�j���[�V��������

                transform.DOLocalMoveX(1f, speed).SetRelative();
            }

            //���Ɉړ�
            if (Input.GetKey(KeyCode.A))
            {
                transform.DOLocalMoveX(-1f, speed).SetRelative();
            }
        }
    }
}