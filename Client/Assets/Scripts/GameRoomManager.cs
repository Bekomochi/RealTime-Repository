using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomManager : MonoBehaviour
{
    GameDirector gameDirector;

    // Start is called before the first frame update
    void Start()
    {
        /* Start�֐����ŁA�����������Ăяo���B
         * �����������ŃC���X�^���X����������Ă���̂ŁA�C���X�^���X�����͋L�����Ȃ��B
         */

        gameDirector.JoinRoom();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
