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
    int CharacterHP = Character.HP;
    public float invincibilityDuration = 2.0f;//無敵時間（秒）
    private float invincibilityTimer = 0.0f;//経過時間を格納するタイマー変数(初期値0秒)
    public Slider hpSlider;
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

    public void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag=="character")
        {
            CharacterHP -= 10;//水が当たったら、HPを10減らす
            Debug.Log(CharacterHP);
            hpSlider.value = CharacterHP;

            if (CharacterHP <= 0)
            {//キャラクターのHPが0以下になったら
                CharacterHP = 0;//キャラクターのHPを0固定にする
                Debug.Log(CharacterHP);//キャラクターのHPをコンソールに表示して確認
            }
        }
    }
}
