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

    public static int CharacterHP = 100;//�L�����N�^�[�̗̑́B�K�X����

    //FloatingJoystick floatingJoystick;//�X�}�z�Ή��p�X���C�h�p�b�h
    float speed = 0.6f;//�����X�s�[�h�̕ϐ��B0.6f�ɐݒ�
    public bool isSelf { get; set; } = false;//�������g���ǂ����𔻒肷��ϐ�
    Animator animator;//�A�j���[�^�[�擾

    // Start is called before the first frame update
    void Start()
    {
        roomModel= GameObject.Find("RoomModel").GetComponent<RoomModel>();

        animator = GetComponent<Animator>();
        HPSlider.maxValue = CharacterHP;
        roomModel.OnValue += OnHPValue;

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
                transform.DOLocalMoveZ(1f, speed).SetRelative();
                //���x�ݒ�
                animator.SetInteger("state", 1);//�ړ��������A�A�j���[�V������state��1�ɂ���

                if(Input.GetKeyUp(KeyCode.W))
                {
                    animator.SetInteger("state", 0);//�ړ��������A�A�j���[�V������state��1�ɂ���
                }
            }

            //��O�Ɉړ�
            if (Input.GetKey(KeyCode.S))
            {
                //���x�ݒ�
                animator.SetInteger("state", 1);//�ړ��������A�A�j���[�V������state��1�ɂ���

                transform.DOLocalMoveZ(-1f, speed).SetRelative();
            }

            //�E�Ɉړ�
            if (Input.GetKey(KeyCode.D))
            {
                //���x�ݒ�
                animator.SetInteger("state", 1);//�ړ��������A�A�j���[�V������state��1�ɂ���

                transform.DOLocalMoveX(1f, speed).SetRelative();
            }

            //���Ɉړ�
            if (Input.GetKey(KeyCode.A))
            {
                //���x�ݒ�
                animator.SetInteger("state", 1);//�ړ��������A�A�j���[�V������state��1�ɂ���

                transform.DOLocalMoveX(-1f, speed).SetRelative();
            }
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other.tag == "warter")
        {
            CharacterHP -= 10;//��������������AHP��10���炷
            HPSlider.value = CharacterHP;

            if (CharacterHP <= 0)
            {//�L�����N�^�[��HP��0�ȉ��ɂȂ�����
                CharacterHP= 0;
            }
        }
    }

    /// <summary>
    /// HP�X�V����
    /// </summary>

    public void OnHPValue(int hp)
    {
        CharacterHP = hp;
    }

    public async void HPValueAsync(int hp)
    {
        await roomModel.HPValueAsync(hp);
    }

}