using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetLoader : MonoBehaviour
{
    //ローディングのスライダー
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

    //カタログ更新処理
    IEnumerator loading()
    {
        var handle = Addressables.UpdateCatalogs();//最新のカタログ(json)を取得
        yield return handle;

        //ダウンロード実行
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync("default", false);//"default"は、グループで設定したラベル

        //ダウンロード完了まで、スライダーのUIを更新
        while(downloadHandle.Status==AsyncOperationStatus.None)
        {
            loadingSlider.value=downloadHandle.GetDownloadStatus().Percent*100;//Percentは、0～1で取得。たぶんパーセント

            yield return null;//1フレーム待つ
        }

        loadingSlider.value=100;
        Addressables.Release(downloadHandle);

        //次のシーンに移動

    }
}
