using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WarterGunManager : MonoBehaviour
{//���S�C�p�̃X�N���v�g

    [SerializeField] private ParticleSystem warterParticle;
    [SerializeField] AudioClip WaterSE;

    //�T�E���h�Đ��p
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
    /// ���𔭎˂��鏈��
    /// </summary>

    public void ShotWarter()
    {//�{�^���������Đ��𕬎˂���ׂ́A�N���b�N���Ɏg���֐�
        //WaterSE��炷
        audioSource.PlayOneShot(WaterSE);

        //warterParticle���Đ�����
        warterParticle.Play();
    }
}
