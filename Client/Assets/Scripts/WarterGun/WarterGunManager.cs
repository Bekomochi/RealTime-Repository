using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WarterGunManager : MonoBehaviour
{//���S�C�p�̃X�N���v�g

    [SerializeField] private ParticleSystem warterParticle;

    //�T�E���h�Đ��p
    AudioSource audioSource;

    void Start()
    {

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

        //warterParticle���Đ�����
        warterParticle.Play();
    }
}
