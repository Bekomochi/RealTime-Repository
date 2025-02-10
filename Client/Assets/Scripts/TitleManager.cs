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

    //�T�E���h�Đ��p
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        //BGM���Đ�
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void movementLobby()
    {
        //SE���Đ�
        audioSource.PlayOneShot(ClickSE);

        //�{�^������������A�{�^����_�ł�����
        ButtonImage.DOFade(endValue: 0f, duration: 0.1f).SetLoops(-1, LoopType.Yoyo);

        //�V�[���ړ�
        //SceneManager.LoadScene("Lobby");
        Initiate.Fade("Lobby", Color.black, 1.5f);
    }
}
