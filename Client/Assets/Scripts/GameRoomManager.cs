using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRoomManager : MonoBehaviour
{
    [SerializeField] int CountNum;
    [SerializeField] RoomModel roomModel;

    GameDirector gameDirector;
    public Text CountDownText; //�X�^�[�g�܂ł̃J�E���g�_�E���p�̃e�L�X�g(3�J�E���g)
    public Text StartText; //�J�n�p�e�L�X�g([Start!!])
    public GameObject FinishButton;//�I���p���{�^��

    // Start is called before the first frame update
    void Start()
    {
        roomModel.OnPreparationUser += this.OnPreparationUser; //��������
        roomModel.OnReadyGame += this.OnReadyGame; //�Q�[���J�n
        roomModel.OnFinishGame += this.OnFinishGame;//�Q�[���I��

        /* Start�֐����ŁA�����������Ăяo���B
         * �����������ŃC���X�^���X����������Ă���̂ŁA�C���X�^���X�����͋L�����Ȃ��B
         */

        //CountDownText����̐ݒ�
        CountNum = 3; //CountNum��������
        CountDownText.text = CountNum.ToString(); //CountDownText�����Ԃɔ��f������

        //InvokeRepeating�ŁA1�b���Ƃ�CountDown�֐����Ăяo��

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void JoinRoom()
    {
        //����
        await roomModel.JoinAsync(GameDirector.RoomName,GameDirector.Id);
        /*���[�����ƃ��[�U�[ID��n���ē�������B
         *�ŏI�I�ɂ́A�u���[�J���ɕۑ����ꂽUserID�v���w�肷��B
         */

    }
    public async void OnPreparationUser()
    {//�J�n�O�̃J�E���g
     //���̒���await�ŃJ�E���g��i�߂���//

        //3�l�W�܂�����CountDownText��\�����āA�J�E���g�_�E�����Ă���
        CountDownText.gameObject.SetActive(true); //CountDownText��\��

        //1�b���ƂɃJ�E���g�_�E��
        InvokeRepeating("CountDown", 1, 1);
    }

    public async void OnReadyGame()
    {
        //StartText��\��������
        StartText.gameObject.SetActive(true); //�^�C�����O�������������ɒx�ꂽ���[�U�[���s���ɂȂ�̂ŁA�\��������^�C�~���O�����킹��

        //
        //�L�����N�^�[�𓮂������Ԃɂ���
        //

        //1000ms��(1�b��)��StartText���\���ɂ���ݒ�
        await Task.Delay(1000); //�ȉ��̏�����1�b��ɔ�\��/�\���Ƃ���
        StartText.gameObject.SetActive(false);�@//StartText���\���ɂ���

        //FinishBuuton��\������
        FinishButton.gameObject.SetActive(true);
    }

    public async Task CountDown()
    {//�J�E���g�_�E������֐�
        CountNum--;
        CountDownText.text = CountNum.ToString();

        if (CountNum == 0)
        {
            //�J�E���g�_�E�����~�߂�
            CancelInvoke("CountDown");

            //CountDownText���\��
            CountDownText.gameObject.SetActive(false);

            //ReadyAsync���Ăяo��
            await roomModel.ReadyAsync();
        }
    }
    public void OnFinishGame()
    {//�Q�[���I��(�{�^���������ďI������͉̂��̓���)
        //���U���g��ʂɈړ�
        SceneManager.LoadScene("Result");
    }

    public async void FinishAsync()
    {//�I�����đގ�
        //FinishAsync���Ăяo��
        await roomModel.FinishAsync();
    }

}
