using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Shared.Interfaces.StreamingHubs;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class LobbyCharacter : MonoBehaviour
{
    //================================
    //���r�[�ł̃L�����N�^�[���Ǘ�����N���X
    //�쐬:�O�Y�L�H
    //================================

    RoomModel roomModel;
    Rigidbody rigidbody;//RigitBody�擾

    float speed = 10;//�����X�s�[�h�̕ϐ�
    public bool isSelf { get; set; } = false;//�������g���ǂ����𔻒肷��ϐ�

    // Start is called before the first frame update
    void Start()
    {
        roomModel = GameObject.Find("RoomModel").GetComponent<RoomModel>();

        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
