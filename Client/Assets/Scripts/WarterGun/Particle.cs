using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine.UI;

public class Particle : MonoBehaviour
{//���S�C�̃p�[�e�B�N���p�̃X�N���v�g

    [SerializeField] private ParticleSystem warterParticle;
    int CharacterHP = Character.HP;
    public float invincibilityDuration = 2.0f;//���G���ԁi�b�j
    private float invincibilityTimer = 0.0f;//�o�ߎ��Ԃ��i�[����^�C�}�[�ϐ�(�����l0�b)
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
    /// ���̓����蔻��
    /// </summary>

    public void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag=="character")
        {
            CharacterHP -= 10;//��������������AHP��10���炷
            Debug.Log(CharacterHP);
            hpSlider.value = CharacterHP;

            if (CharacterHP <= 0)
            {//�L�����N�^�[��HP��0�ȉ��ɂȂ�����
                CharacterHP = 0;//�L�����N�^�[��HP��0�Œ�ɂ���
                Debug.Log(CharacterHP);//�L�����N�^�[��HP���R���\�[���ɕ\�����Ċm�F
            }
        }
    }
}
