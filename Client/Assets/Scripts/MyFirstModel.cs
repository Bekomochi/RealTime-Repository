using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstmodel : MonoBehaviour
{
    const string ServerURL = "http://localhost:7000";

    // Start is called before the first frame update
    async void Start()
    {
        int result = await Sum(100, 230);
        Debug.Log(result);

        int numList = await SumList(new int[2] {1,2});
        Debug.Log(numList);
        
        float numFloat = await SumFloat(3.4f,5.6f);
        Debug.Log(numFloat);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public async UniTask<int> Sum(int x, int y/*Action<int> callback*/)
    {
        using var handler = new YetAnotherHttpHandler(){Http2Only = true};
        var channel = GrpcChannel.ForAddress(ServerURL,new GrpcChannelOptions() { HttpHandler = handler });
        var cliant=MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await cliant.SumAsync(x, y);
        return result;
    }

    public async UniTask<int> SumList(int[] numList/*Action<int> callback*/)
    {
        using var handler = new YetAnotherHttpHandler() { Http2Only = true };
        var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
        var cliant = MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await cliant.SumAllAsync(numList);
        return result;
    }

    public async UniTask<float> SumFloat(float x,float y/*Action<int> callback*/)
    {
        using var handler = new YetAnotherHttpHandler() { Http2Only = true };
        var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions(){ HttpHandler = handler });
        var numArray = new Number();
        numArray.x = x;
        numArray.y = y;
        var cliant = MagicOnionClient.Create<IMyFirstService>(channel);
        var result = await cliant.SumAllNumberAsync(numArray);
        return result;
    }
}
