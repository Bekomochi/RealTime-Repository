using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] AudioClip ClickSE;

    public GameObject ReturnButton;//タイトルに戻るボタン

    //サウンド再生用
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Transitions()
    {
        //シーン移動
        //SceneManager.LoadScene("Title");
        Initiate.Fade("Title", Color.black, 1.5f);
        audioSource.PlayOneShot(ClickSE);
    }
}
