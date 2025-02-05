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
    //ロビーでのキャラクターを管理するクラス
    //作成:三浦有稀
    //================================

    RoomModel roomModel;
    Rigidbody rigidbody;//RigitBody取得

    float speed = 10;//歩くスピードの変数
    public bool isSelf { get; set; } = false;//自分自身かどうかを判定する変数

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
