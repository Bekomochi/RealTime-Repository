using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Particle : MonoBehaviour
{//水鉄砲のパーティクル用のスクリプト
 //参考:https://prupru-prune.hatenablog.com/entry/2022/02/12/003726

    [SerializeField] private ParticleSystem warterParticle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 水が当たった時に時にHPを減らす
    /// </summary>

    public void ShotWarter()
    {//引数としてキャラクターを受け取り、キャラクターのHPを減らす
     //後ほど攻撃処理と同時に実装
        warterParticle.Play();
    }
}
