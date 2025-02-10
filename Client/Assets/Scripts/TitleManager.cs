using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [SerializeField] AudioClip ClickSE;

    public float DurationSeconds;
    public Ease EaseType;

    public Image ButtonImage;

    //サウンド再生用
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        //BGMを再生
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void movementLobby()
    {
        //SEを再生
        audioSource.PlayOneShot(ClickSE);

        //ボタンを押したら、ボタンを点滅させる
        ButtonImage.DOFade(endValue: 0f, duration: 0.1f).SetLoops(-1, LoopType.Yoyo);

        //シーン移動
        //SceneManager.LoadScene("Lobby");
        Initiate.Fade("Lobby", Color.black, 1.5f);
    }
}
