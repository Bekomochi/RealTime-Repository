using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WarterGunManager : MonoBehaviour
{//���S�C�p�̃X�N���v�g

    [SerializeField] private ParticleSystem warterParticle;

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

    /* ��قǁA���������������Ƀ_���[�W��^���鏈����ǉ�
     * �����蔻�莩�̂̓p�[�e�B�N���̃X�N���v�g������p�H
     */

    public void ShotWarter()
    {//�{�^���������Đ��𕬎˂���ׂ́A�N���b�N���Ɏg���֐�
        warterParticle.Play();
    }
}
