using Server.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameDirector : MonoBehaviour
{//�Q�[���i�s���Ǘ�����N���X
    [SerializeField] GameObject characterPrefab;
    [SerializeField] RoomModel roomModel;

    //�I�u�W�F�N�g�ƌ��т���
    public InputField IDinputField;
    
    Dictionary<Guid, GameObject> characterList = new Dictionary<Guid, GameObject>();//�ڑ�ID���L�[�ɂ��āA�L�����N�^�[�̃I�u�W�F�N�g���Ǘ�

    // Start is called before the first frame update
    async void Start()
    {
        //���[�U�[����/�ގ���������OnJoinedUser���\�b�h�����s�ł���悤�ɁA���f���ɓo�^���Ă���
        roomModel.OnJoinedUser += this.OnJoinedUser;//����
        roomModel.OnLeavedUser += this.OnLeavedUser;//�ގ�

        //���[�U�[ID����͂�����̓t�B�[���h��GetComponent����
        IDinputField = IDinputField.GetComponent<InputField>();

        //�ڑ�
        await roomModel.ConnectAsync();
    }

    /// <summary>
    /// ��������
    /// </summary>

    public async void JoinRoom()
    {
        string IDtext = IDinputField.text;
        int.TryParse(IDtext, out int id);

        //����
        await roomModel.JoinAsync("SampleRoom",id );
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
        await roomModel.LeaveAsync();
    }

    //���[�U�[���ؒf�������̏���(�ؒf������Destroy)
    private void OnLeavedUser(LeavedUser user)
    {//�ގ�������Destroy����

        if (roomModel.ConnectionId == user.ConnectionID)
        {
            foreach (var charaList in characterList)
            {
                Destroy(charaList.Value);
            }
        }
        else
        {
            Destroy(characterList[user.ConnectionID]);
        }
    }

    //characterlist����Ώۂ�Gameobject���擾
    void OnMoveCharacter(/*�ڑ�ID�A�ʒu�A��]*/)
    {
        //characterlist����Ώۂ�Gameobject���擾
        //�ʒu�A��]�𔽉f
    }

    // Update is called once per frame
    void Update()
    {

    }
}
