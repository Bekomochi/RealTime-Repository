using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WarterGunManager : MonoBehaviour
{//水鉄砲用のスクリプト

    [SerializeField] private ParticleSystem warterParticle;
    [SerializeField] AudioClip WaterSE;

    //サウンド再生用
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 水を発射する処理
    /// </summary>

    public void ShotWarter()
    {//ボタンを押して水を噴射する為の、クリック時に使う関数
        //WaterSEを鳴らす
        audioSource.PlayOneShot(WaterSE);

        //warterParticleを再生する
        warterParticle.Play();
    }
}
