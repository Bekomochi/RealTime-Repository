using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine.UI;

public class Particle : MonoBehaviour
{//水鉄砲のパーティクル用のスクリプト

    [SerializeField] private ParticleSystem warterParticle;
    private float invincibilityTimer = 0.0f;//経過時間を格納するタイマー変数(初期値0秒)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 水の当たり判定
    /// </summary>

}
