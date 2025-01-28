using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject character;

    Vector3 currentPos;//���݂̃J�����ʒu
    Vector3 pastPos;//�ߋ��̃J�����ʒu
    Vector3 diff;//�ړ�����

    // Start is called before the first frame update
    void Start()
    {
        //�ŏ��̃L�����N�^�[�ʒu�̎擾
        pastPos = character.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //
        //�L�����N�^�[�̌��ݒn�̎擾
        //

        currentPos = character.transform.position;
        diff = pastPos - currentPos;        
        transform.position = Vector3.Lerp(transform.position, transform.position + diff, 1.0f);//�J�������L�����N�^�[�̈ړ���������������
        pastPos = currentPos;


        //
        //�J�����̉�]
        //


    }
}
