using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Particle : MonoBehaviour
{//���S�C�̃p�[�e�B�N���p�̃X�N���v�g
 //�Q�l:https://prupru-prune.hatenablog.com/entry/2022/02/12/003726

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
    /// ���������������Ɏ���HP�����炷
    /// </summary>

    public void ShotWarter()
    {//�����Ƃ��ăL�����N�^�[���󂯎��A�L�����N�^�[��HP�����炷
     //��قǍU�������Ɠ����Ɏ���
        warterParticle.Play();
    }
}
