using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomManager : MonoBehaviour
{
    GameDirector gameDirector;

    // Start is called before the first frame update
    void Start()
    {
        /* Start関数内で、入室処理を呼び出す。
         * 入室処理内でインスタンス生成がされているので、インスタンス生成は記入しない。
         */

        gameDirector.JoinRoom();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
