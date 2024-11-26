using Server.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameDirector : MonoBehaviour
{//�Q�[���i�s���Ǘ�����N���X
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;
    [SerializeField] InputField userIDField;

    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();//�ڑ�ID���L�[�ɂ��āA�L�����N�^�[�̃I�u�W�F�N�g���Ǘ�

    // Start is called before the first frame update
    async void Start()
    {//���[�U�[��������������OnJoinedUser���\�b�h�����s�ł���悤�ɁA���f���ɓo�^���Ă���
        roomModel.OnJoinedUser += this.OnJoinedUser;

        roomModel.OnLeavedUser += this.OnLeavedUser;

        //�ڑ�
        await roomModel.ConnectAsync();
    }

    /// <summary>
    /// ��������
    /// </summary>

    public async void JoinRoom()
    {
        //����
        await roomModel.JoinAsync("SampleRoom", 1);
        /*���[�����ƃ��[�U�[ID��n���ē�������B
         *���[�U�[ID�́AUI��inputfield�œ��͂ł���悤�ɂ������B
         *�ŏI�I�ɂ́A�u���[�J���ɕۑ����ꂽUserID�v���w�肷��B
         */
    }

    //���[�U�[�������������̏���
    private void OnJoinedUser(JoinedUser user)
    {//����������Instantiate����
        GameObject characterObject = Instantiate(characterPrefab);//�C���X�^���X����
        characterObject.transform.position = new Vector3(0, 0, 0);
        characterList[user.ConnectionID]= characterObject;//�t�B�[���h�ŕێ�
    }

    /// <summary>
    /// �ؒf�A�ގ�����
    /// </summary>

    //�ގ�
    public async void LeaveRoom()
    {
        //�ގ�
        await roomModel.LeaveAsync("SampleRoom", 1);
    }

    //���[�U�[���ؒf�������̏���(�ؒf������Destroy)
    private void OnLeavedUser(LeavedUser user)
    {//�ގ�������Destroy����
        
        //Destroy(characterList[user.ConnectionID]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
