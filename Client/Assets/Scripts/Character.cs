using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;

public class Character : MonoBehaviour
{
    //FloatingJoystick floatingJoystick;//�X�}�z�Ή��p�X���C�h�p�b�h
    float speed = 0.6f;//�����X�s�[�h�̕ϐ��B0.6f�ɐݒ�
    public static int HP = 100;//�L�����N�^�[�̗̑́B�K�X����
    public bool isSelf { get; set; } = false;//�������g���ǂ����𔻒肷��ϐ�
    Animator animator;//�A�j���[�^�[�擾
    public static BoxCollider boxCollider;//�L�����N�^�[��BoxCollider�̕ϐ�

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();//BoxCollider���擾
        animator = GetComponent<Animator>();

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
                //���x�ݒ�
                animator.SetFloat("speed", speed);//�ړ��������Aspeed�b�̑����ŃX�e�[�^�X��"speed"�̂��̂��A�j���[�V��������

                transform.DOLocalMoveX(-1f, speed).SetRelative();
            }
        }
    }
}