using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetLoader : MonoBehaviour
{
    //���[�f�B���O�̃X���C�_�[
    [SerializeField] Slider loadingSlider;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loading());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�J�^���O�X�V����
    IEnumerator loading()
    {
        var handle = Addressables.UpdateCatalogs();//�ŐV�̃J�^���O(json)���擾
        yield return handle;

        //�_�E�����[�h���s
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync("default", false);//"default"�́A�O���[�v�Őݒ肵�����x��

        //�_�E�����[�h�����܂ŁA�X���C�_�[��UI���X�V
        while(downloadHandle.Status==AsyncOperationStatus.None)
        {
            loadingSlider.value=downloadHandle.GetDownloadStatus().Percent*100;//Percent�́A0�`1�Ŏ擾�B���Ԃ�p�[�Z���g

            yield return null;//1�t���[���҂�
        }

        loadingSlider.value=100;
        Addressables.Release(downloadHandle);

        //���̃V�[���Ɉړ�

    }
}
