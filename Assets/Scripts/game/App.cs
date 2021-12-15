using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class App : MonoBehaviour
{
    private Dictionary<string, BaseModel> modelMap = new Dictionary<string, BaseModel>();
    private static App instance;
    public WebSocketWrapper net;
    public static App Ins
    {
        get { return instance; }
    }
    
    void Start()
    {

        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        net = WebSocketWrapper.Create("ws://127.0.0.1:3000");
        net.Connect();
    }
    public T getModel<T>() where T : BaseModel, new()
    {
        if (modelMap[typeof(T).Name] != null)
        {
            return modelMap[typeof(T).Name] as T;
        }
        else
        {
            T t = new T();
            modelMap.Add(typeof(T).Name, t);
            return t;
        }
    }
    public void Update()
    {
        Timer.Update();
    }
}
