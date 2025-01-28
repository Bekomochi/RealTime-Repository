using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject character;

    Vector3 currentPos;//現在のカメラ位置
    Vector3 pastPos;//過去のカメラ位置
    Vector3 diff;//移動距離

    // Start is called before the first frame update
    void Start()
    {
        //最初のキャラクター位置の取得
        pastPos = character.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //
        //キャラクターの現在地の取得
        //

        currentPos = character.transform.position;
        diff = pastPos - currentPos;        
        transform.position = Vector3.Lerp(transform.position, transform.position + diff, 1.0f);//カメラをキャラクターの移動差分だけ動かす
        pastPos = currentPos;


        //
        //カメラの回転
        //


    }
}
