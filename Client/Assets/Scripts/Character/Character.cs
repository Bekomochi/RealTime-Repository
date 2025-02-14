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
    //========================
    //�L�����N�^�[���Ǘ�����N���X
    //�쐬:�O�Y�L�H
    //========================

    [SerializeField] public Slider HPSlider;
    RoomModel roomModel;
    Animator animator;//�A�j���[�^�[�擾
    Rigidbody rigidbody;//RigitBody�擾
    FixedJoystick fixedJoystick;//�X�}�z�Ή��p�^�b�`�p�b�h(FixedJoyStick)

    public static int CharacterHP = 100;//�L�����N�^�[�̗̑́B�K�X����
    float x;//�L�[����(����)
    float z;//�L�[����(����,���s)

    float speed = 10;//�����X�s�[�h�̕ϐ�
    public bool isSelf { get; set; } = false;//�������g���ǂ����𔻒肷��ϐ�

    // Start is called before the first frame update
    void Start()
    {
        roomModel = GameObject.Find("RoomModel").GetComponent<RoomModel>();
        fixedJoystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        HPSlider.maxValue = CharacterHP;
        roomModel.OnValue += OnHPValue;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector3(1, 0, 0);

        //�ړ��̐ݒ�
        if (isSelf == true)
        {//����������������
            if (Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
            {//A�L�[��D�L�[�������ꂽ��
                x = Input.GetAxisRaw("Horizontal");//Horizontal��������
            }
            else
            {//JoyStick����̓��͂�������
                x = fixedJoystick.Horizontal;//FixedJoyStick��Horizontal��������
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {//����W�L�[��D�L�[�������ꂽ��
                z = Input.GetAxisRaw("Vertical");//Vertical��������
            }
            else
            {//JoyStick����̓��͂�������
                z = fixedJoystick.Vertical;//FixedJoyStick��Vertical��������
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
                CharacterHP = 0;
                HPSlider.value = CharacterHP;
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

    public void FixedUpdate()
    {
        //���x�ݒ�
        rigidbody.velocity = new Vector3(x, 0, z) * speed;
        Vector3 direction = transform.position + new Vector3(x, 0, z) * speed;

        //�����]��
        transform.LookAt(direction);

        //�A�j���[�V�����ݒ�
        animator.SetFloat("speed", rigidbody.velocity.magnitude);
    }

    public void OnShotButton()
    {//���˃{�^������������
        animator.SetBool("shot", true);//���˃A�j���[�V�������Đ�����
    }
}
