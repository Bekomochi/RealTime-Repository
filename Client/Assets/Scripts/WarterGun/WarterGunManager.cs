using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WarterGunManager : MonoBehaviour
{//水鉄砲用のスクリプト

    [SerializeField] private ParticleSystem warterParticle;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 水を発射する処理
    /// </summary>

    /* 後ほど、水が当たった時にダメージを与える処理を追加
     * 当たり判定自体はパーティクルのスクリプトから引用？
     */

    public void ShotWarter()
    {//ボタンを押して水を噴射する為の、クリック時に使う関数
        warterParticle.Play();
    }
}
